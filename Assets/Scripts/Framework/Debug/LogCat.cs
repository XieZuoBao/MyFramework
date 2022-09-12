using UnityEngine;

public class LogCat : MonoBehaviour
{
    private bool m_showLog = false;
    private string m_logStr = "";

    private Rect m_scrollViewRect;
    private GUIStyle m_lblStyle;
    private Vector2 m_scrollViewPos;

    public static void Init()
    {
        GameObject go = new GameObject("LogCat");
        go.AddComponent<LogCat>();
    }

    private void Start()
    {
        Application.logMessageReceivedThreaded += LogCallBack;

        m_scrollViewRect = new Rect(0, 0, Screen.width, Screen.height * 0.9f);
        m_lblStyle = new GUIStyle();
        m_lblStyle.normal.textColor = Color.white;
        m_lblStyle.wordWrap = true;
        m_lblStyle.fontSize = 25;
    }

    private void LogCallBack(string condition, string stackTrace, LogType type)
    {
        //if(type ==LogType.Error || type == LogType.Exception || type == LogType.Assert)
        {
            m_logStr += condition + "\n";
        }
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.F4))
#else
        if (Input.GetMouseButtonDown(0) && Input.touchCount == 4)
#endif
        {
            m_showLog = !m_showLog;
        }
    }

    public void OnGUI()
    {
        if (!m_showLog)
            return;

        GUILayout.BeginArea(m_scrollViewRect);
        GUILayout.Box("", GUILayout.Width(m_scrollViewRect.width), GUILayout.Height(m_scrollViewRect.height));
        GUILayout.EndArea();

        GUILayout.BeginArea(m_scrollViewRect);
        m_scrollViewPos = GUILayout.BeginScrollView(m_scrollViewPos);
        GUILayout.Label(m_logStr, m_lblStyle);
        GUILayout.EndScrollView();

        if (GUILayout.Button("Clear", GUILayout.Height(80)))
        {
            m_logStr = "";
        }
        GUILayout.EndArea();
    }
}