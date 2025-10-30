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
            var targetArg = StringUtils.RemoveSingleQuotes(arg);
            if (idx == 0)
            {
                Console.Write(targetArg);
                idx++;
                continue;
            }
            Console.Write($" {targetArg}");
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
            string? path = Environment.GetEnvironmentVariable("PATH");

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
        string? path = Environment.GetEnvironmentVariable("PATH");


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

        try
        {


            Process process = new Process();
            process.StartInfo.FileName = Path.GetFileNameWithoutExtension(filePath);
            
            foreach (var arg in args)
            {
                var cleanArg = StringUtils.RemoveSingleQuotes(arg);
                process.StartInfo.ArgumentList.Add(cleanArg);
            }

            process.Start();
            process.WaitForExit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }


        return true;

    }

    public void cd(string targetPath)
    {
        if (targetPath == "" || targetPath == "~")
        {
            string? homeDir = Environment.GetEnvironmentVariable("HOME");

            if (homeDir == null)
            {
                Console.WriteLine($"cd: {targetPath}: No such file or directory");
                return;
            }

            Directory.SetCurrentDirectory(homeDir);

            return;
        }

            if (!Directory.Exists(targetPath))
            {
                Console.WriteLine($"cd: {targetPath}: No such file or directory");
                return;
            }
            Directory.SetCurrentDirectory(targetPath);

        // ~の場合 or targetPathが空の場合
    }

}