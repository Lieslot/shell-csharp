using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;


public class CommandHandler

{

    public readonly CommandExecuter executer;

    public CommandHandler()
    {
        executer = new CommandExecuter();
    }

    public void handle()
    {
        while (true)
        {
            Console.Write("$ ");

            // ユーザーのinputを受け取る
            string inputStr = Console.ReadLine();

            if (inputStr == "")
            {
                Console.WriteLine($"{inputStr}: command not found");
                continue;
            }

            List<string> parsedCommand = parse(inputStr);

            var command = parsedCommand[0];

            if (command == "exit")
            {
                break;
            }

            if (command == "echo")
            {

                List<string> args = [.. parsedCommand.Skip(1)];

                executer.echo(args);
                continue;
            }

            if (command == "type")
            {
                if (isNoArgs(parsedCommand))
                {
                    parsedCommand.Add("");
                }

                List<string> args = [.. parsedCommand.Skip(1)];


                executer.type(args);
                continue;
            }

            if (command == "pwd")
            {
                Console.WriteLine(Directory.GetCurrentDirectory());
                continue;
            }
            
            if (command == "cd")
            {
                if (isNoArgs(parsedCommand))
                {
                    parsedCommand.Add("");
                }
                string targetPath = parsedCommand[1];
                executer.Cd(targetPath);

                continue;
            }

            var isExecuted = executer.ExecuteBy(parsedCommand[0], [.. parsedCommand.Skip(1)]);

            if (isExecuted)
            {
                continue;
            } 

            


            Console.WriteLine($"{inputStr}: command not found");
        }
    }

    private List<string> parse(string command)
    {

        var parsedCommand = command.Split(" ");
        if (parsedCommand.Length == 1)
        {
            return [.. parsedCommand];
        }
        return [.. parsedCommand];
    }
    
    private bool isNoArgs(List<string> commands)
    {
        return commands.Count == 1;
    }



}