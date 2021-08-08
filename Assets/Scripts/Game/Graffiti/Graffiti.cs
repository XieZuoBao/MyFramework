using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画符涂鸦
/// 1.保存手指接触过的点(Update)
/// 2.将点练成线
/// 3.将屏幕上的点线映射到图片上
/// </summary>
public class Graffiti : MonoBehaviour
{
    private float count = 100.0f;
    private List<Vector2> allPoints = new List<Vector2>();
    //划线使用的材质球
    Material lineMaterial;

    Texture2D texture2D;

    private void Start()
    {
        texture2D = new Texture2D(400, 200);
        transform.GetComponent<MeshRenderer>().material.mainTexture = texture2D;
    }

    /// <summary>
    /// 使用GL画线的回调
    /// </summary>
    public void OnRenderObject()
    {
        //创建材质球
        CreateLineMaterial();
        //激活第一个着色器通过（在本例中，我们知道它是唯一的通过）
        lineMaterial.SetPass(0);
        //设置正交
        GL.LoadOrtho();
        // 开始画线  在Begin——End之间写画线方式
        //GL.LINES 画线
        GL.Begin(GL.LINES);
        //设置颜色
        GL.Color(Color.red);
        for (int i = 1; i < allPoints.Count; i++)
        {
            //将点练成线
            GL.Vertex3(allPoints[i - 1].x, allPoints[i - 1].y, 0);
            GL.Vertex3(allPoints[i].x, allPoints[i].y, 0);
        }
        GL.End();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            //1.保存手指接触过的点
            allPoints.Add(Camera.main.ScreenToViewportPoint(Input.mousePosition));
        }
        if (Input.GetMouseButtonUp(0))
        {
            GenerateTextureWithGraffitiPoints();
            allPoints.Clear();
        }
        if (Input.GetMouseButtonDown(0))
        {
            //重置Texture2D
            ResetTexture2D();
        }
    }

    /// <summary>
    /// 重置Texture2D
    /// </summary>
    void ResetTexture2D()
    {
        for (int i = 0; i < texture2D.width; i++)
        {
            for (int j = 0; j < texture2D.height; j++)
                texture2D.SetPixel(i, j, Color.white);
        }
        texture2D.Apply();
    }

    /// <summary>
    /// 创建一个材质球
    /// </summary>
    void CreateLineMaterial()
    {
        //如果材质球不存在
        if (!lineMaterial)
        {
            //用代码的方式实例一个材质球
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            //设置参数
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            //设置参数
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            //设置参数
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    /// <summary>
    /// 将涂鸦的点生成图片
    /// </summary>
    void GenerateTextureWithGraffitiPoints()
    {
        for (int i = 1; i < allPoints.Count; i++)
        {
            //上一个点
            Vector2 lastPoint = allPoints[i - 1];
            float lastPosX = texture2D.width * lastPoint.x;
            float lastPosY = texture2D.height * lastPoint.y;
            //当前点
            Vector2 currentPoint = allPoints[i];
            float currentPosX = texture2D.width * currentPoint.x;
            float currentPosY = texture2D.height * currentPoint.y;
            //在上一个点与当前点中间平滑插值其他的点,保证涂鸦的点之间的连续性
            for (int j = 0; j < count; j++)
            {
                int pixelX = (int)Mathf.Lerp(lastPosX, currentPosX, j / count);
                int pixelY = (int)Mathf.Lerp(lastPosY, currentPosY, j / count);
                texture2D.SetPixel(pixelX, pixelY, Color.red);
            }
        }
        texture2D.Apply();
    }
}
