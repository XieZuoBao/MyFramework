using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 不规则按钮
/// <para>图片在导入时需要开启Readable/Write Enable,不建议使用</para>
/// </summary>
public class AlphaButton : MonoBehaviour
{
    public float alphaThreshold = 0.1f;

    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = alphaThreshold;
    }
}
