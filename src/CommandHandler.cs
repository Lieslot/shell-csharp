using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;


public class CommandHandler(CommandDispatcher commandDispatcher, IOutputWriter outputWriter)


{

    private readonly CommandDispatcher dispatcher = commandDispatcher;
    private readonly IOutputWriter writer = outputWriter;

    public void handle()
    {
        while (true)
        {
            writer.Write("$ ", isError: false);

            // ユーザーのinputを受け取る
            string? inputStr = Console.ReadLine();

            if (inputStr == null)
            {
                writer.WriteLine($": command not found", isError: true);
                continue;
            }

            List<string> parsedCommand = CommandParser.Parse(inputStr);

            if (parsedCommand.Count == 0)
            {
                writer.WriteLine($": command not found", isError: true);
                continue;
            }

            var command = parsedCommand[0];

            if (command == "exit")
            {
                break;
            }

            dispatcher.Dispatch(parsedCommand);

        }
    }

    private bool isNoArgs(List<string> commands)
    {
        return commands.Count == 1;
    }



}