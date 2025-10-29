using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ExecutableFileFinder
{
    public static string? FindPathFromDirectory(string dir, string target)
    {
        foreach (var filePath in Directory.EnumerateFiles(dir))
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            if (fileName.Equals(target))
            {
                if (!FilePermissionChecker.CanExecute($"{filePath}"))
                {
                    continue;
                }
                return filePath;
            }
        }
        return null;
    }

    public static string? FindPathFromDirectories(List<string> directories, string target)
    {
        foreach (var dir in directories)
        {
            if (!Directory.Exists(dir))
            {
                continue;
            }

            var filePath = FindPathFromDirectory(dir, target);

            if (filePath != null)
            {
                return filePath;
            }
        }

        return null;

    }

}