# C# xUnit テストセットアップガイド

このドキュメントでは、このプロジェクトで行ったテストセットアップの各ステップとその理由を説明します。

## 目次
1. [テストプロジェクトの作成](#1-テストプロジェクトの作成)
2. [プロジェクト参照の追加](#2-プロジェクト参照の追加)
3. [ソリューションへの追加](#3-ソリューションへの追加)
4. [メインプロジェクトの設定修正](#4-メインプロジェクトの設定修正)
5. [テストコードの作成](#5-テストコードの作成)
6. [テストの実行](#6-テストの実行)

---

## 1. テストプロジェクトの作成

### 実行したコマンド
```bash
mkdir tests
dotnet new xunit -n codecrafters-shell.Tests -o tests/codecrafters-shell.Tests
```

### 目的
- **testsディレクトリの作成**: テスト関連のファイルをメインコードと分離して管理するため
- **xUnitテンプレートの使用**: .NET で最も人気のあるテストフレームワークを使用
  - xUnitは.NET Core公式でサポートされている
  - モダンで拡張性が高い
  - `[Fact]`や`[Theory]`などのシンプルな属性でテストを記述できる

### 自動的にインストールされるパッケージ
- `xunit` - テストフレームワーク本体
- `xunit.runner.visualstudio` - Visual Studioとの統合
- `Microsoft.NET.Test.Sdk` - テスト実行に必要なSDK

---

## 2. プロジェクト参照の追加

### 実行したコマンド
```bash
dotnet add tests/codecrafters-shell.Tests/codecrafters-shell.Tests.csproj reference codecrafters-shell.csproj
```

### 目的
テストプロジェクトからメインプロジェクトのコード（`CommandParser`など）にアクセスできるようにするため。

この参照がないと：
- テストコードから`CommandParser.Parse()`などのメソッドを呼び出せない
- コンパイルエラーが発生する

### 結果
`codecrafters-shell.Tests.csproj`に以下が追加されます：
```xml
<ItemGroup>
  <ProjectReference Include="..\..\codecrafters-shell.csproj" />
</ItemGroup>
```

---

## 3. ソリューションへの追加

### 実行したコマンド
```bash
dotnet sln codecrafters-shell.sln add tests/codecrafters-shell.Tests/codecrafters-shell.Tests.csproj
```

### 目的
- ソリューション全体でテストプロジェクトを管理できるようにする
- `dotnet test`コマンドを実行した際に、テストプロジェクトが自動的に検出される
- Visual StudioやRiderなどのIDEでプロジェクト全体を開いたときに、テストプロジェクトも表示される

### メリット
```bash
# ソリューション全体のテストを実行（追加した全テストプロジェクトが対象）
dotnet test

# ソリューション全体をビルド
dotnet build
```

---

## 4. メインプロジェクトの設定修正

### 実行した編集
`codecrafters-shell.csproj`に以下を追加：
```xml
<ItemGroup>
  <Compile Remove="tests/**" />
  <EmbeddedResource Remove="tests/**" />
  <None Remove="tests/**" />
</ItemGroup>
```

### 目的
メインプロジェクトのビルド時に`tests/`ディレクトリ配下のファイルをコンパイル対象から除外するため。

### なぜ必要だったか
- .NET SDKはデフォルトでプロジェクトディレクトリ配下の全`.cs`ファイルをコンパイルしようとする
- テストファイル（xUnitの`[Fact]`属性など）がメインプロジェクトからコンパイルされると、xUnitへの参照がないためエラーになる
- テストコードはテストプロジェクトでのみコンパイルされるべき

### トラブルシューティング
もし設定を追加しないと以下のようなエラーが発生します：
```
error CS0246: 型または名前空間の名前 'FactAttribute' が見つかりませんでした
```

また、ビルドキャッシュの問題を解決するために以下を実行：
```bash
rm -rf obj bin
dotnet clean
```

---

## 5. テストコードの作成

### 作成したファイル
`tests/codecrafters-shell.Tests/UnitTest1.cs`

### テストの構造（AAA パターン）
各テストメソッドは以下の構造に従っています：

```csharp
[Fact]
public void TestMethodName()
{
    // Arrange（準備）- テストデータの準備
    var input = "echo hello";

    // Act（実行）- テスト対象のメソッドを実行
    var result = CommandParser.Parse(input);

    // Assert（検証）- 結果を検証
    Assert.Equal(2, result.Count);
}
```

### 作成したテストケース

| テスト名 | 目的 |
|---------|------|
| `Parse_SimpleCommand_ReturnsSingleArgument` | 単純なコマンド（引数なし）のパース |
| `Parse_CommandWithArguments_ReturnsMultipleArguments` | スペース区切りの複数引数のパース |
| `Parse_SingleQuotedString_PreservesSpaces` | シングルクォート内のスペース保持 |
| `Parse_DoubleQuotedString_HandlesEscapedCharacters` | ダブルクォート内のエスケープ文字処理 |
| `Parse_EscapedBackslash_HandlesCorrectly` | バックスラッシュのエスケープ処理 |
| `Parse_BackslashEscape_OutsideQuotes` | クォート外のバックスラッシュエスケープ |
| `Parse_EmptyString_ReturnsEmptyList` | 空文字列の処理 |
| `Parse_MultipleSpaces_IgnoresExtraSpaces` | 連続するスペースの処理 |
| `Parse_MixedQuotes_HandlesBothTypes` | シングルとダブルクォートの混在 |
| `Parse_ConcatenatedQuotes_CombinesIntoOneArgument` | クォートの連結 |

### よく使うxUnit アサーション

```csharp
// 等価性の検証
Assert.Equal(expected, actual);

// コレクションの要素数検証
Assert.Single(collection);        // 要素が1つ
Assert.Empty(collection);         // 空
Assert.Equal(3, collection.Count); // 特定の数

// 真偽値の検証
Assert.True(condition);
Assert.False(condition);

// null検証
Assert.Null(obj);
Assert.NotNull(obj);

// 例外の検証
Assert.Throws<ArgumentException>(() => SomeMethod());
```

---

## 6. テストの実行

### 基本的なコマンド

```bash
# 全テストを実行
dotnet test

# 特定のテストプロジェクトのみ実行
dotnet test tests/codecrafters-shell.Tests/

# 詳細な出力で実行
dotnet test --verbosity detailed

# 特定のテストメソッドのみ実行
dotnet test --filter "FullyQualifiedName~Parse_SimpleCommand"

# テスト結果をログファイルに出力
dotnet test --logger "trx;LogFileName=test-results.trx"
```

### テスト実行結果の見方

```
成功!   -失敗:     0、合格:    10、スキップ:     0、合計:    10、期間: 417 ms
```

- **失敗**: テストが失敗した数
- **合格**: テストが成功した数
- **スキップ**: `[Fact(Skip = "理由")]`で一時的にスキップしたテスト
- **合計**: 全テスト数
- **期間**: 実行時間

---

## プロジェクト構造

最終的なディレクトリ構造：

```
codecrafters-shell-csharp/
├── codecrafters-shell.csproj          # メインプロジェクト
├── codecrafters-shell.sln             # ソリューションファイル
├── src/                               # ソースコード
│   ├── main.cs
│   ├── CommandPaser.cs                # テスト対象
│   ├── CommandHandler.cs
│   └── ...
├── tests/                             # テストディレクトリ
│   └── codecrafters-shell.Tests/      # テストプロジェクト
│       ├── codecrafters-shell.Tests.csproj
│       └── UnitTest1.cs               # テストコード
├── bin/                               # ビルド出力（git無視）
└── obj/                               # ビルド中間ファイル（git無視）
```

---

## 追加のテストを書く方法

### 新しいテストクラスを追加する場合

```bash
# tests/codecrafters-shell.Tests/ ディレクトリ内に新しいファイルを作成
# 例: CommandHandlerTests.cs
```

```csharp
namespace codecrafters_shell.Tests;

public class CommandHandlerTests
{
    [Fact]
    public void YourTestMethod()
    {
        // Arrange
        // Act
        // Assert
    }
}
```

### パラメータ化テストを書く場合

同じロジックで異なる入力をテストする場合は`[Theory]`を使用：

```csharp
[Theory]
[InlineData("echo", 1)]
[InlineData("echo hello", 2)]
[InlineData("echo hello world", 3)]
public void Parse_ReturnsCorrectCount(string input, int expectedCount)
{
    // Act
    var result = CommandParser.Parse(input);

    // Assert
    Assert.Equal(expectedCount, result.Count);
}
```

---

## まとめ

このセットアップにより：

1. テストコードがメインコードから分離されている
2. xUnitの強力な機能を使ってテストを書ける
3. `dotnet test`コマンドで簡単にテストを実行できる
4. CI/CDパイプラインに統合しやすい
5. 他の開発者が同じ環境でテストを実行できる

### 次のステップ
- 他のクラス（`CommandHandler`、`ExecutableFileFinder`など）のテストを追加
- テストカバレッジを計測（`dotnet test --collect:"XPlat Code Coverage"`）
- モックライブラリ（Moq）を導入して、依存関係のあるクラスをテスト
