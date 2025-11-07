namespace codecrafters_shell.Tests;

public class CommandParserTests
{
    [Fact]
    public void Parse_SimpleCommand_ReturnsSingleArgument()
    {
        // Arrange
        var input = "echo";

        // Act
        var result = CommandParser.Parse(input);

        // Assert
        Assert.Single(result);
        Assert.Equal("echo", result[0]);
    }

    [Fact]
    public void Parse_CommandWithArguments_ReturnsMultipleArguments()
    {
        // Arrange
        var input = "echo hello world";

        // Act
        var result = CommandParser.Parse(input);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("echo", result[0]);
        Assert.Equal("hello", result[1]);
        Assert.Equal("world", result[2]);
    }

    [Fact]
    public void Parse_SingleQuotedString_PreservesSpaces()
    {
        // Arrange
        var input = "echo 'hello   world'";

        // Act
        var result = CommandParser.Parse(input);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("echo", result[0]);
        Assert.Equal("hello   world", result[1]);
    }

    [Fact]
    public void Parse_DoubleQuotedString_HandlesEscapedCharacters()
    {
        // Arrange
        var input = "echo \"hello \\\"world\\\"\"";

        // Act
        var result = CommandParser.Parse(input);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("echo", result[0]);
        Assert.Equal("hello \"world\"", result[1]);
    }

    [Fact]
    public void Parse_EscapedBackslash_HandlesCorrectly()
    {
        // Arrange
        var input = "echo \"\\\\test\"";

        // Act
        var result = CommandParser.Parse(input);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("\\test", result[1]);
    }

    [Fact]
    public void Parse_BackslashEscape_OutsideQuotes()
    {
        // Arrange
        var input = "echo hello\\ world";

        // Act
        var result = CommandParser.Parse(input);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("echo", result[0]);
        Assert.Equal("hello world", result[1]);
    }

    [Fact]
    public void Parse_EmptyString_ReturnsEmptyList()
    {
        // Arrange
        var input = "";

        // Act
        var result = CommandParser.Parse(input);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Parse_MultipleSpaces_IgnoresExtraSpaces()
    {
        // Arrange
        var input = "echo    hello    world";

        // Act
        var result = CommandParser.Parse(input);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("echo", result[0]);
        Assert.Equal("hello", result[1]);
        Assert.Equal("world", result[2]);
    }

    [Fact]
    public void Parse_MixedQuotes_HandlesBothTypes()
    {
        // Arrange
        var input = "echo 'single quote' \"double quote\"";

        // Act
        var result = CommandParser.Parse(input);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal("echo", result[0]);
        Assert.Equal("single quote", result[1]);
        Assert.Equal("double quote", result[2]);
    }

    [Fact]
    public void Parse_ConcatenatedQuotes_CombinesIntoOneArgument()
    {
        // Arrange
        var input = "echo \"hello\"world";

        // Act
        var result = CommandParser.Parse(input);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("helloworld", result[1]);
    }
}
