using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{

    [SerializeField]
    private Button btnGameStart;
    [SerializeField]
    private Button btnSetting;
    [SerializeField]
    private Button btnAbout;
    [SerializeField]
    private Button btnQuit;

    protected override void Init()
    {
        btnSetting.onClick.AddListener(() =>
        {
            PanelEffects.Instance.PanelPopUp(UIManager.Instance.ShowPanel<SettingPanel>().gameObject.transform);
        });
    }

  
}
