using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;


public class CommandHandler(CommandDispatcher commandDispatcher, IOutputWriter outputWriter)


{

    private readonly CommandDispatcher dispatcher = commandDispatcher;
    private readonly IOutputWriter writer = outputWriter;

    public async Task handle()
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

            // pipeがあるかをチェック
            if (inputStr.Contains('|'))
            {
                List<string> commands = [.. inputStr.Split('|')];
                List<List<string>> parsedCommands = [.. commands.Select(CommandParser.Parse)];

                await PipelineExecuter.Execute(parsedCommands);
                continue;
            }

            List<string> parsedCommand = CommandParser.Parse(inputStr);

            // pipeがあったら入出力のストリムをFDに変える

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