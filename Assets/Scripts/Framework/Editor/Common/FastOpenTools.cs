using UnityEngine;
using UnityEditor;

public class FastOpenTools
{
    [MenuItem("Tools/命令/打开I18配置 &i")]
    public static void OpenI18NCfg()
    {
        OpenFileOrDirectory("/GAssets/Launch/Res/Config/i18nAppStrings.bytes");
    }

    [MenuItem("Tools/命令/打开资源配置 &r")]
    public static void OpenResCfg()
    {
        OpenFileOrDirectory("/GAssets/Launch/Res/Config/resources.bytes");
    }

    public static void OpenFileOrDirectory(string url, string workingDir = "")
    {
        string path = Application.dataPath + url;
        path = path.Replace("/", "\\");
        EdtUtil.OpenFolderInExplorer(path);
    }
}