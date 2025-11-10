using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

public class PipelineExecuter
{

    public static async Task Execute(List<List<string>> commands)
    {


        List<Process> processes = [];
        List<Task> pipelineTasks = [];

        // プロセスを実行する
        for (int i = 0; i < commands.Count; i++)
        {
            List<string> command = commands[i];

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command[0],
                    Arguments = string.Join(" ", command.Skip(1)),
                    UseShellExecute = false,
                    RedirectStandardInput = i > 0,              // 最初以外は入力をリダイレクト
                    RedirectStandardOutput = i < commands.Count - 1,  // 最後以外は出力をリダイレクト
                }
            };

            process.Start();
            processes.Add(process);
        }


        for (int i = 0; i < processes.Count - 1; i++)
        {
            var source = processes[i];
            var target = processes[i + 1];

            var task = Task.Run(async () =>
            {
                await source.StandardOutput.BaseStream.CopyToAsync(
                    target.StandardInput.BaseStream);
                target.StandardInput.Close();
            });
            pipelineTasks.Add(task);
        }

        await Task.WhenAll(pipelineTasks);

        // 全プロセスの終了を待つ
        foreach (var process in processes)
        {
            await process.WaitForExitAsync();
        }
    }
}