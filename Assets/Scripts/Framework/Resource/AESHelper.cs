using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 对Lua和Proto文件加密
/// <para>创建临时文件夹,用来临时存放每个模块需要加密的明文文件</para>
/// <para>1.首先把Src/文件夹和Res/Proto/文件夹都复制到临时文件夹</para>
/// <para>2.接着把Src/文件夹和Res/Proto/文件夹进行就地加密(.bytes)</para>
/// <para>3.然后把Src/文件夹和Res/Proto/文件夹的明文文件就地删除</para>
/// <para>打包完成后,还需要对“现场”进行恢复,就是加密文件再删除掉,然后把临时文件夹的明文文件放回各个模块的原始位置</para>
/// <para>1.删除GAssets的各个模块中的加密文件</para>
/// <para>2.然后把存放在临时文件夹的明文文件再拷贝回GAssets的各个模块中</para>
/// <para>3.最后要删除临时文件夹</para>
/// </summary>
public class AESHelper
{
    /// <summary>
    /// AES加密的密钥 必须是32位
    /// </summary>
    public static string keyValue = "12345678123456781234567812345678";

    /// <summary>
    /// AES 算法加密
    /// </summary>
    /// <param name="content">明文</param>
    /// <param name="key">密钥</param>
    /// <returns>加密后的密文</returns>
    public static string Encrypt(string content, string key)
    {
        try
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyBytes;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            byte[] resultBytes = cTransform.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
            string result = Convert.ToBase64String(resultBytes, 0, resultBytes.Length);
            return result;
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("加密出错: " + e.ToString());
            return null;
        }
    }

    /// <summary>
    /// AES 算法解密
    /// </summary>
    /// <param name="content">加密后的密文</param>
    /// <param name="key">密钥</param>
    /// <returns>明文</returns>
    public static string Decipher(string content, string key)
    {
        try
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            RijndaelManaged rm = new RijndaelManaged();
            rm.Key = keyBytes;
            rm.Mode = CipherMode.ECB;
            rm.Padding = PaddingMode.PKCS7;
            ICryptoTransform ict = rm.CreateDecryptor();
            byte[] contentBytes = Convert.FromBase64String(content);
            byte[] resultBytes = ict.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
            return Encoding.UTF8.GetString(resultBytes);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("解密出错: " + e.ToString());
            return null;
        }
    }
}
