using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameDurationPanel : BasePanel
{
    [SerializeField]
    private Button btnQuit;
    protected override void Init()
    {
        btnQuit.onClick.AddListener(() =>
        {
            PanelEffects.Instance.PanelPopUp(UIManager.Instance.ShowPanel<QuitConfirmPanel>().gameObject.transform);
        });
    }
}
