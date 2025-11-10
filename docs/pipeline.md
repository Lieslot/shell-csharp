
## 定義

プロセスの間の入力と出力をつなげるコマンド
IPC(Inter-Process-Connection)を可能とする一つの手法ともいえる

## なぜ使うのか
結論から言うと一つの役割を果たすいろんなプログラムを組み合わせて、多様な処理ができるようにするため。
UNIXの下記の設計思想とつながっているように見える

Make each program do one thing well. To do a new job, build afresh rather than complicate old programs by adding new "features".(各プログラムが一つのことをうまくやる、新しいことをするため、複雑な古いプログラムに新たな機能を追加を追加するより、新しいものを作るようにする。)
Expect the output of every program to become the input to another, as yet unknown, program.（すべてのプログラムの出力は、まだ未知の別のプログラムへの入力となることを想定せよ。)


## どうやって実現するのか

UNIXではpipe()というシステムコールがあって、READ-ONLYのFDとWRITE-ONLYのFDを返す。
そしてFORK()で新たなプロセスを作って、一つのWRITE-ONLYのFDに入力したら、READ-ONLY FDを持っているプロセス側でデータを読み込んで処理するみたいな感じ。

シェルでPIPEを使ってコマンドを実行する際にはすべてのプロセスが一斉に実行されるらしい

EX)

```shell
ls | grep file.txt
```

質問1.どうせ前のコマンドの出力を次のコマンドに渡すだけなのに全てのprogramを一斉に起動させるか？各プロセスが終わり次第次のprogramを実行すればいいのではないか？

-> なぜなら、pipelineのbufferの容量に限界あるためで、すべてのプロセスを実行させるとストリムな処理でデータの損失を防げるから。

pipelineを用いてプロセス間の通信を行う際にはbufferが存在する
もし一つのプロセスの出力がこのbufferの容量を超えてしまうと、データが損失
そのためすべてのプロセスを起動させてストリム処理で少しずつ処理していくことで、データの損失予防するため。

