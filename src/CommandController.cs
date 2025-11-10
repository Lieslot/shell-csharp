using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


[Controller]
public class CommandController(IOutputWriter writer)
{

    private readonly IOutputWriter writer = writer;

    [CommandRoute(command: "echo")]
    public void Echo(List<string> command)
    {
        List<string> args = [.. command.Skip(1)];
        Result result = CommandExecuter.Echo(args);


        writer.WriteLine(result.Data, !result.IsSuccess);

    }

    [CommandRoute(command: "type")]
    public void Type(List<string> command)
    {
        if (isNoArgs(command))
        {
            command.Add("");
        }

        List<string> args = [.. command.Skip(1)];


        Result result = CommandExecuter.Type(args);

        writer.WriteLine(result.Data, !result.IsSuccess);

    }

    [CommandRoute(command: "cd")]
    public void Cd(List<string> command)
    {
        if (isNoArgs(command))
        {
            command.Add("");
        }
        string targetPath = command[1];
        Result result = CommandExecuter.Cd(targetPath);

        writer.WriteLine(result.Data, !result.IsSuccess);

    }

    [CommandRoute(command: "pwd")]
    public void Pwd(List<string> command)
    {

        writer.WriteLine(Directory.GetCurrentDirectory(), false);

    }

    [CommandRoute(isDefault: true)]
    public void executeBy(List<string> command)
    {
        Result result = CommandExecuter.ExecuteBy(command[0], [.. command.Skip(1)]);

        if (result.IsSuccess)
        {
            return;
        }

        writer.WriteLine(result.Data, !result.IsSuccess);

    }

    private bool isNoArgs(List<string> commands)
    {
        return commands.Count == 1;
    }


}