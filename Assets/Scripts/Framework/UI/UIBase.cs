/*
 * 
 *      Title:  基础框架
 * 
 *             
 *      Description: 
 *              UI管理模块
 *                       
 ***/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 面板基类
/// 帮助我们便捷找到控件
/// </summary>
public class UIBase : MonoBehaviour
{
    //通过里氏转换原则,来存储所有的控件
    private Dictionary<string, List<UIBehaviour>> componentDict = new Dictionary<string, List<UIBehaviour>>();

    /// <summary>
    /// 继承BasePanel的子类,如果需要在Awake中处理相关的逻辑时,必须先执行BasePanel的Awake函数(一定不可缺!!!!!)
    /// 基类BasePanel的Awake函数有查找控件,添加事件等逻辑
    /// </summary>
    protected virtual void Awake()
    {
        FindComponentsInChildren<Button>();
        FindComponentsInChildren<Image>();
        FindComponentsInChildren<Text>();
        FindComponentsInChildren<InputField>();
        FindComponentsInChildren<Slider>();
        FindComponentsInChildren<ScrollRect>();
        FindComponentsInChildren<Toggle>();
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    public virtual void ShowPanel()
    {

    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    public virtual void HidePanel()
    {

    }

    /// <summary>
    /// 按钮点击事件回调,具体逻辑在子类中重写
    /// </summary>
    /// <param name="btnName"></param>
    protected virtual void OnClick(string btnName)
    {

    }

    /// <summary>
    /// Toggle状态发生改变时的回调,具体逻辑在子类中重写
    /// </summary>
    /// <param name="toggleName"></param>
    /// <param name="value"></param>
    protected virtual void OnToggleValueChanged(string toggleName, bool value)
    {

    }

    /// <summary>
    /// 滑动条的值发生改变时的回调,具体逻辑在子类中重写
    /// </summary>
    /// <param name="sliderName"></param>
    /// <param name="value"></param>
    protected virtual void OnSliderValueChanged(string sliderName, float value)
    {

    }


    /// <summary>
    /// 遍历子类,从子类中获取所有的一类组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindComponentsInChildren<T>() where T : UIBehaviour
    {
        T[] components = this.GetComponentsInChildren<T>();
        //string objName;
        for (int i = 0; i < components.Length; i++)
        {
            string objName = components[i].gameObject.name;

            if (componentDict.ContainsKey(objName))
                componentDict[objName].Add(components[i]);
            else
                componentDict.Add(objName, new List<UIBehaviour>() { components[i] });

            //按钮控件
            if (components[i] is Button)
            {
                //(components[i] as Button).onClick.AddListener(OnClick);
                (components[i] as Button).onClick.AddListener(() =>
                {
                    OnClick(objName);
                });
            }
            else if (components[i] is Slider)//Slider
            {
                (components[i] as Slider).onValueChanged.AddListener((value) =>
                {
                    OnSliderValueChanged(objName, value);
                });
            }
            else if (components[i] is Toggle)//Toggle
            {
                (components[i] as Toggle).onValueChanged.AddListener((value) =>
                {
                    OnToggleValueChanged(objName, value);
                });
            }
        }
    }

    /// <summary>
    /// 根据名字获取对应的控件
    /// </summary>
    /// <typeparam name="T">控件类型</typeparam>
    /// <param name="componentName">控件名称</param>
    /// <returns></returns>
    protected T GetComponentAtChildren<T>(string componentName) where T : UIBehaviour
    {
        if (componentDict.ContainsKey(componentName))
        {
            for (int i = 0; i < componentDict[componentName].Count; i++)
            {
                if (componentDict[componentName][i] is T)
                    return componentDict[componentName][i] as T;
            }
        }
        return null;
    }
}