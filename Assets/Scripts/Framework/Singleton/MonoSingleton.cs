using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承自MonoBehaviour的"单例模式",直接"类名.Instance"即可(不需要我们手动去拖到对象上,或者动态添加到对象上)
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                //设置对象的名字为脚本名
                obj.name = typeof(T).ToString();
                //过场景,不移除
                DontDestroyOnLoad(obj);
                instance = obj.AddComponent<T>();
            }
            return instance;
        }
    }
}