using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CommandExecuter
{
    public void echo(List<string> args)
    {

        var idx = 0;
        foreach (var arg in args)
        {
            if (idx == 0)
            {
                Console.Write(arg);
                idx++;
                continue;
            }
            Console.Write($" {arg}");
            idx++;
        }

        Console.WriteLine();
    }
    public void type(List<string> args)
    {
        string target = args[0];

        if (Enum.TryParse<BuiltinCommands>(target, ignoreCase: true, out _))
        {
            Console.WriteLine($"{target} is a shell builtin");
        }

        else
        {
            Console.WriteLine($"{target}: not found");
        }
    }
    
}