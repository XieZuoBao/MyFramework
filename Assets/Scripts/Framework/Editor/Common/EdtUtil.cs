using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdtUtil
{
    /// <summary>
    /// 打开文件夹
    /// </summary>
    /// <param name="absPath">文件夹的绝对路径</param>
    public static void OpenFolderInExplorer(string absPath)
    {
#if UNITY_EDITOR
        if (Application.platform == RuntimePlatform.WindowsEditor)
            RunCmdNoErr("explorer.exe", absPath);
        else if (Application.platform == RuntimePlatform.OSXEditor)
            RunCmdNoErr("open", absPath.Replace("\\", "/"));
#endif
    }

    /// <summary>
    /// 运行shell命令,不返回stderr版本
    /// </summary>
    /// <param name="cmd">命令(exe的文件名)</param>
    /// <param name="args">命令的参数</param>
    /// <returns>命令的stdout输出</returns>
    public static string RunCmdNoErr(string cmd, string args)
    {
        var p = CreateCmdProcess(cmd, args);
        var res = p.StandardOutput.ReadToEnd();
        p.Close();
        return res;
    }

    private static System.Diagnostics.Process CreateCmdProcess(string cmd, string args)
    {
        var pStartInfo = new System.Diagnostics.ProcessStartInfo(cmd);
        pStartInfo.Arguments = args;
        pStartInfo.CreateNoWindow = false;
        pStartInfo.UseShellExecute = false;
        pStartInfo.RedirectStandardError = true;
        pStartInfo.RedirectStandardInput = true;
        pStartInfo.RedirectStandardOutput = true;
        pStartInfo.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
        pStartInfo.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
        return System.Diagnostics.Process.Start(pStartInfo);
    }
}
