using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
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

            // PATHの環境変数の値を取得
            string path = Environment.GetEnvironmentVariable("PATH");

            if (path == null)
            {
                Console.WriteLine($"{target}: not found");

            }

            // splitで対象のディレクトリをリスト化する
            List<string> directories = [.. path.Split(Path.PathSeparator)];
            // ディレクトリをリスト探索する]
            foreach (var dir in directories)
            {
                if (!Directory.Exists(dir))
                {
                    continue;
                }

                foreach (var filePath in Directory.EnumerateFiles(dir))
                {

                    var fileName = Path.GetFileNameWithoutExtension(filePath);

                    if (fileName.Equals(target))
                    {

                        if (!FilePermissionChecker.CanExecute($"{filePath}"))
                        {
                            return;
                        }

                        Console.WriteLine($"{target} is {filePath}");
                        return;
                    }
                }
            }
            Console.WriteLine($"{target}: not found");
        }
    }
    

    
    
}