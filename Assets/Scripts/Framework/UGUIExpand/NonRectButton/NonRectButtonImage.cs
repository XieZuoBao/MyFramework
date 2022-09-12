using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/// <summary>
/// 不规则按钮
/// <para>1.UICamera必须是正交模式</para>
/// <para>2.screenPoint要用eventCamera.ScreenToWorldPoint进行转换</para>
/// </summary>
public class NonRectButtonImage : Image
{
    /// <summary>
    /// 自定义事件响应区域:光标选中的Graphic
    /// </summary>
    /// <param name="screenPoint"></param>
    /// <param name="eventCamera"></param>
    /// <returns></returns>
    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return GetComponent<PolygonCollider2D>().OverlapPoint(eventCamera.ScreenToWorldPoint(screenPoint));
    }
}