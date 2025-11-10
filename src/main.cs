using System.Collections;
using System.Data;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {

        var outputWriter = new ConsoleOutputWriter();
        var commandController = new CommandController(writer: outputWriter);
        var commandDispatcher = new CommandDispatcher(controller: commandController);
        var commandHandler = new CommandHandler(commandDispatcher, outputWriter);

        await commandHandler.handle();
        // validation -> C#
        // input -> 
        // output = ロジック

    }

}
