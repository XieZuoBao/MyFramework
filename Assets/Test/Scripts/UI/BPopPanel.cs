using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BPopPanel : UIBasePanel
{
    private Button btnClose;
    private void Awake()
    {
        btnClose = transform.GetChildByName("BtnClose").GetComponent<Button>();
    }

    private void Start()
    {
        btnClose.onClick.AddListener(OnBtnExitClick);
    }

    void OnBtnExitClick()
    {
        UIMgr.Instance.HidePanel<BPopPanel>();
    }
}
