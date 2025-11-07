using System.Collections;
using System.Data;

class Program
{
    static void Main()
    {


        var commandExecuter = new CommandExecuter();
        var outputWriter = new ConsoleOutputWriter();
        var commandController = new CommandController(executer: commandExecuter, writer: outputWriter);
        var commandDispatcher = new CommandDispatcher(controller: commandController);
        var commandHandler = new CommandHandler(commandDispatcher, outputWriter);

        commandHandler.handle();
        // validation -> C#
        // input -> 
        // output = ロジック

    }

}
