using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

public static class CommandExecuter
{
    public static Result Echo(List<string> args)
    {
        /*
        quoteは一つの引数として扱う
        単純なスペースは全部つぶす

        'の場合stackに入れる single quote flag　= true
        次の
        "の場合
        */

        string target = string.Join(" ", args);

        return new Result(IsSuccess: true, target);
    }
    public static Result Type(List<string> args)
    {
        string target = args[0];

        if (Enum.TryParse<BuiltinCommands>(target, ignoreCase: true, out _))
        {

            return new Result(IsSuccess: true, $"{target} is a shell builtin");
        }
        else
        {
            // PATHの環境変数の値を取得
            string? path = Environment.GetEnvironmentVariable("PATH");

            if (path == null)
            {
                return new Result(IsSuccess: false, $"{target} is a shell builtin");
            }

            // splitで対象のディレクトリをリスト化する
            List<string> directories = [.. path.Split(Path.PathSeparator)];

            var filePath = ExecutableFileFinder.FindPathFromDirectories(directories, target);

            if (filePath != null)
            {
                return new Result(IsSuccess: true, $"{target} is {filePath}");
            }
        }

        return new Result(IsSuccess: false, $"{target}: not found");
    }

    public static Result ExecuteBy(string target, string[] args)
    {
        // PATHの環境変数の値を取得
        string? path = Environment.GetEnvironmentVariable("PATH");


        if (path == null)
        {
            return new Result(false, $"{target}: command not found");
        }

        // splitで対象のディレクトリをリスト化する
        List<string> directories = [.. path.Split(Path.PathSeparator)];
        var filePath = ExecutableFileFinder.FindPathFromDirectories(directories, target);

        if (filePath == null)
        {
            return new Result(false, $"{target}: command not found");
        }

        try
        {
            Process process = new Process();
            process.StartInfo.FileName = Path.GetFileNameWithoutExtension(filePath);

            foreach (var arg in args)
            {
                process.StartInfo.ArgumentList.Add(arg);
            }

            process.Start();
            process.WaitForExit();

            return new Result(true, "");

        }
        catch (Exception e)
        {
            // TODO: ファイルパスを探せなかった場合とプロセスが失敗した場合を分ける
            return new Result(false, e.Message);
        }



    }

    public static Result Cd(string targetPath)
    {
        if (targetPath == "" || targetPath == "~")
        {
            string? homeDir = Environment.GetEnvironmentVariable("HOME");

            Directory.SetCurrentDirectory(homeDir);
            return new Result(IsSuccess: true, "");

        }

        if (!Directory.Exists(targetPath))
        {
            return new Result(false, $"cd: {targetPath}: No such file or directory");
        }

        Directory.SetCurrentDirectory(targetPath);

        return new Result(IsSuccess: true, "");


        // ~の場合 or targetPathが空の場合
    }

}