using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            return;
        }
        else
        {
            // PATHの環境変数の値を取得
            string path = Environment.GetEnvironmentVariable("PATH");

            if (path == null)
            {
                Console.WriteLine($"{target}: not found");
                return;
            }

            // splitで対象のディレクトリをリスト化する
            List<string> directories = [.. path.Split(Path.PathSeparator)];

            var filePath = ExecutableFileFinder.FindPathFromDirectories(directories, target);

            if (filePath != null)
            {
                Console.WriteLine($"{target} is {filePath}");
                return;
            }
        }

        Console.WriteLine($"{target}: not found");
    }

    public bool ExecuteBy(string target, string[] args)
    {
        // PATHの環境変数の値を取得
        string path = Environment.GetEnvironmentVariable("PATH");


        if (path == null)
        {
            return false;
        }

        // splitで対象のディレクトリをリスト化する
        List<string> directories = [.. path.Split(Path.PathSeparator)];
        var filePath = ExecutableFileFinder.FindPathFromDirectories(directories, target);

        if (filePath == null)
        {
            return false;
        }
        Process process = new Process();
        process.StartInfo.FileName = Path.GetFileNameWithoutExtension(filePath);
        process.StartInfo.Arguments = string.Join(" ", args);

        process.Start();
        process.WaitForExit();

        return true;
    }

    public void Cd(string targetPath)
    {

        if (targetPath[0].Equals(Path.DirectorySeparatorChar) || targetPath[0].Equals(Path.AltDirectorySeparatorChar))
        {
            if (!Directory.Exists(targetPath))
            {
                Console.WriteLine($"cd: {targetPath}: No such file or directory");
                return;
            }

            Directory.SetCurrentDirectory(targetPath);
        }

        // ..の場合
        // .の場合
        // ~の場合 or targetPathが空の場合
    }

}