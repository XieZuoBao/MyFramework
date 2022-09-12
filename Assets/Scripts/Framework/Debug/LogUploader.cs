using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// 实现日志上传到服务器的功能
/// <para>用小皮web服务器实现</para>
/// </summary>
public class LogUploader : MonoBehaviour
{
    private static string LOG_UPLOAD_URL = "http://127.0.0.1:7890/upload_log.php";

    public static void StartUploadLog(string logFilePath, string desc)
    {
        var go = new GameObject("LogUploader");
        var bhv = go.AddComponent<LogUploader>();
        bhv.StartCoroutine(bhv.UploadLog(logFilePath, LOG_UPLOAD_URL, desc));
    }

    /// <summary>
    /// 上报日志到服务端
    /// </summary>
    /// <param name="url">http接口</param>
    /// <param name="desc">描述</param>
    private IEnumerator UploadLog(string logFilePath, string url, string desc)
    {
        var fileName = Path.GetFileName(logFilePath);
        var data = ReadLogFile(logFilePath);
        WWWForm form = new WWWForm();
        // 塞入描述字段，字段名与服务端约定好
        form.AddField("desc", desc);
        // 塞入日志字节流字段，字段名与服务端约定好
        form.AddBinaryData("logfile", data, fileName, "application/x-gzip");
        // 使用UnityWebRequest
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        var result = request.SendWebRequest();

        while (!result.isDone)
        {
            yield return null;
            //Debug.Log ("上传进度: " + request.uploadProgress);
        }
        if (!string.IsNullOrEmpty(request.error))
        {
            GameLogger.LogError(request.error);
        }
        else
        {
            GameLogger.Log("日志上传完毕, 服务器返回信息: " + request.downloadHandler.text);
        }
        request.Dispose();
    }

    private byte[] ReadLogFile(string logFilePath)
    {
        byte[] data = null;

        using (FileStream fs = File.OpenRead(logFilePath))
        {
            int index = 0;
            long len = fs.Length;
            data = new byte[len];
            // 根据你的需求进行限流读取
            int offset = data.Length > 1024 ? 1024 : data.Length;
            while (index < len)
            {
                int readByteCnt = fs.Read(data, index, offset);
                index += readByteCnt;
                long leftByteCnt = len - index;
                offset = leftByteCnt > offset ? offset : (int)leftByteCnt;
            }
        }
        return data;
    }
}
