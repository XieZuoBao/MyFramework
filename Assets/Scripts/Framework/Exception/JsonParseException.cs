using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理Json配置文件解析异常的情况
/// </summary>
public class JsonParseException : Exception
{
    public JsonParseException() : base()
    {

    }

    public JsonParseException(string msg) : base(msg)
    {

    }
}
