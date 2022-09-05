/*
 * 
 *      Title:ʵ����־�ϴ����������Ĺ���
 * 
 *             
 *      Description: ��СƤweb������ʵ��
 *           
 *              
 ***/
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

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
    /// �ϱ���־�������
    /// </summary>
    /// <param name="url">http�ӿ�</param>
    /// <param name="desc">����</param>
    private IEnumerator UploadLog(string logFilePath, string url, string desc)
    {
        var fileName = Path.GetFileName(logFilePath);
        var data = ReadLogFile(logFilePath);
        WWWForm form = new WWWForm();
        // ���������ֶΣ��ֶ���������Լ����
        form.AddField("desc", desc);
        // ������־�ֽ����ֶΣ��ֶ���������Լ����
        form.AddBinaryData("logfile", data, fileName, "application/x-gzip");
        // ʹ��UnityWebRequest
        UnityWebRequest request = UnityWebRequest.Post(url, form);
        var result = request.SendWebRequest();

        while (!result.isDone)
        {
            yield return null;
            //Debug.Log ("�ϴ�����: " + request.uploadProgress);
        }
        if (!string.IsNullOrEmpty(request.error))
        {
            GameLogger.LogError(request.error);
        }
        else
        {
            GameLogger.Log("��־�ϴ����, ������������Ϣ: " + request.downloadHandler.text);
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
            // ��������������������ȡ
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
