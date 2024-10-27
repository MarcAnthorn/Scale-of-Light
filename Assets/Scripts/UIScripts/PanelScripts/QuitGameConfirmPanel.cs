using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameConfirmPanel : BasePanel
{
    [SerializeField]
    private Button btnQuitConfirm;
    [SerializeField]
    private Button btnClose;

    protected override void Init()
    {
        btnQuitConfirm.onClick.AddListener(() =>
        {
            Application.Quit();

        });

        btnClose.onClick.AddListener(() =>
        {
            
            PanelEffects.Instance.PanelPopDown(this.transform);
        });
    }
}
