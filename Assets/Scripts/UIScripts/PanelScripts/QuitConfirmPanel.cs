using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitConfirmPanel : BasePanel
{
    [SerializeField]
    private Button btnQuitConfirm;
    [SerializeField]
    private Button btnClose;

    protected override void Init()
    {
        btnQuitConfirm.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<GameDurationPanel>();
            UIManager.Instance.HidePanel<QuitConfirmPanel>(LoadStartScene);

        });

        btnClose.onClick.AddListener(() =>
        {
            PanelEffects.Instance.PanelPopDown(this.transform);
        });
    }

    private void LoadStartScene()
    {
        LoadSceneManager.Instance.LoadSceneAsync("StartScene");
    }
}
