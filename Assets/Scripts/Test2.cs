using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    void Start()
    {
        Invoke("LogTest", 2f);
    }

    void LogTest()
    {
        GameLogger.Log("哈哈哈哈哈");
        GameLogger.LogCyan("11111");
        GameLogger.LogError("22222");
        GameLogger.LogFormat("{0}", "33333");
        GameLogger.LogFormationColor("{0}", "#000000", "4444");
        GameLogger.LogGreen("55555");
        GameLogger.LogRed("66666");
        GameLogger.LogWarning("77777");
        GameLogger.Log("哈哈哈哈哈");
        GameLogger.LogCyan("11111");
        GameLogger.LogError("22222");
        GameLogger.LogFormat("{0}", "33333");
        GameLogger.LogFormationColor("{0}", "#000000", "4444");
        GameLogger.LogGreen("55555");
        GameLogger.LogRed("66666");
        GameLogger.LogWarning("77777");

        //GameLogger.UploadLog("上传日志测试");

    }
}