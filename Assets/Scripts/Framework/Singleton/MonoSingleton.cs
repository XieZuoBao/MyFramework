/*
 * 
 *      Title:  基础框架
 *              
 *      Description: 
 *              继承自MonoBehaviour的单例模式,需要我们自己保证它的唯一性
 *              1.继承自Mono的脚本,不能够直接new
 *              2.只能通过拖拽到对象身上   或者  动态添加到对象身上(gameobject.AddComponent)  
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 继承自这种自动创建的"单例模式",不需要我们手动去拖到对象上,或者动态添加到对象上,想用他,直接"类名.Instance"即可
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