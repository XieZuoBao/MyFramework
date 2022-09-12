/*
 * 
 *      Title:  基础框架
 * 
 *             
 *      Description: 
 *              自定义摇杆(固定摇杆,可移动的摇杆,跟随的摇杆)
 *              
 ***/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum JoyStick_Type
{
    /// <summary>
    /// 固定的
    /// </summary>
    FIXED,
    /// <summary>
    /// 可移动的
    /// </summary>
    MOVABLE,
    /// <summary>
    /// 跟随的
    /// </summary>
    FOLLOW,
}

public class JoyStickPanel : MonoBehaviour
{
    public JoyStick_Type joyStick_Type = JoyStick_Type.FIXED;
    private float maxRadius = 240;
    //用户监听鼠标按下,抬起,拖拽三个事件的监听者,控制其作用范围区域
    private Image imgTouchRect;
    //摇杆半透明背景
    private Image imgBg;
    //摇杆圆圈
    private Image imgControl;

    private void Start()
    {
        imgTouchRect = transform.GetChildByName("ImgTouchRect").GetComponent<Image>();
        imgBg = transform.GetChildByName("ImgBg").GetComponent<Image>();
        imgControl = transform.GetChildByName("ImgControl").GetComponent<Image>();
        //添加鼠标监听事件---按下,抬起,拖拽三个事件
        UIMgr.AddCustomEventListener(imgTouchRect, EventTriggerType.PointerDown, PointerDown);
        UIMgr.AddCustomEventListener(imgTouchRect, EventTriggerType.PointerUp, PointerUp);
        UIMgr.AddCustomEventListener(imgTouchRect, EventTriggerType.Drag, Drag);
        //非固定摇杆初始隐藏
        if (joyStick_Type != JoyStick_Type.FIXED)
            imgBg.gameObject.SetActive(false);
    }

    /// <summary>
    /// 鼠标按下的回调
    /// </summary>
    /// <param name="data"></param>
    private void PointerDown(BaseEventData data)
    {
        //GameLogger.Log("PinterDown");
        //按下显示
        imgBg.gameObject.SetActive(true);
        //非固定摇杆,手指按下时,将摇杆设置在手指按下点
        if (joyStick_Type != JoyStick_Type.FIXED)
        {
            Vector2 localPos;
            //屏幕坐标转相对本地坐标
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                imgTouchRect.rectTransform,                                        //要改变位置对象的父节点
                (data as PointerEventData).position,                        //当前屏幕鼠标位置(也可用Input.mousePosition)
                (data as PointerEventData).pressEventCamera,                //UI用的摄像机
                out localPos);                                              //转换后得到的相对坐标

            imgBg.transform.localPosition = localPos;
        }
    }

    /// <summary>
    /// 鼠标抬起的回调
    /// </summary>
    /// <param name="data"></param>
    private void PointerUp(BaseEventData data)
    {
        //GameLogger.Log("PinterUp");
        imgControl.transform.localPosition = Vector2.zero;

        //分发摇杆方向向量
        EventCenter.Instance.EventTrigger<Vector2, bool>("JoyStick", Vector2.zero, false);

        //非固定摇杆,松开隐藏
        if (joyStick_Type != JoyStick_Type.FIXED)
            imgBg.gameObject.SetActive(false);
    }

    /// <summary>
    /// 鼠标拖拽的回调
    /// </summary>
    /// <param name="data"></param>
    private void Drag(BaseEventData data)
    {
        //GameLogger.Log("Drag");
        Vector2 localPos;
        //屏幕坐标转相对本地坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgBg.rectTransform,                                        //要改变位置对象的父节点
            (data as PointerEventData).position,                        //当前屏幕鼠标位置(也可用Input.mousePosition)
            (data as PointerEventData).pressEventCamera,                //UI用的摄像机
            out localPos);                                              //转换后得到的相对坐标
        //更新摇杆圆圈位置信息
        imgControl.transform.localPosition = localPos;
        //对摇杆圆圈位置进行限定
        if (localPos.magnitude >= maxRadius)
        {
            //跟随摇杆,更新摇杆半透明背景
            if (joyStick_Type == JoyStick_Type.FOLLOW)
                imgBg.transform.localPosition += (Vector3)(localPos.normalized * (localPos.magnitude - maxRadius));
            imgControl.transform.localPosition = localPos.normalized * maxRadius;
        }

        //分发摇杆方向向量
        EventCenter.Instance.EventTrigger<Vector2, bool>("JoyStick", localPos.normalized, true);
    }
}