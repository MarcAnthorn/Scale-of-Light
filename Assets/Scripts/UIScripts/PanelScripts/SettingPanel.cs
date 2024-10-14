using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{

    [SerializeField]
    private Button btnClose;
    protected override void Init()
    {
        btnClose.onClick.AddListener(() =>
        {
            PanelEffects.Instance.PanelPopDown(this.transform);
        });
    }

   
}
