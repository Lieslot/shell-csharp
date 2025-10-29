using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public static class FilePermissionChecker
{

    public static bool CanExecute(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                return false;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return CanExecuteOnWindows(filePath);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                     RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return CanExecuteOnUnix(filePath);
            }

            // 不明なOSの場合はスキップしない
            return true;
        }
        catch
        {
            return false;
        }
    }


    private static bool CanExecuteOnWindows(string filePath)
    {
        // Windowsでは拡張子ベースで判断
        string ext = Path.GetExtension(filePath).ToLower();
        string[] executableExtensions = { ".exe", ".bat", ".cmd", ".com", ".ps1" };
        return Array.Exists(executableExtensions, e => e == ext);
    }

        private static bool CanExecuteOnUnix(string filePath)
    {
        try
        {
            // .NET 6以降
            UnixFileMode mode = File.GetUnixFileMode(filePath);
            
            // いずれかの実行権限があればOK
            return (mode & (UnixFileMode.UserExecute | 
                           UnixFileMode.GroupExecute | 
                           UnixFileMode.OtherExecute)) != 0;
        }
        catch
        {
            return false;
        }
    }

}