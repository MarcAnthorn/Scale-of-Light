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
            Time.timeScale = 1;
            UIManager.Instance.HidePanel<GameDurationPanel>();
            UIManager.Instance.HidePanel<QuitConfirmPanel>(LoadStartScene);

        });

        btnClose.onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            MoveController.isInputLockedStatic = false;
            PanelEffects.Instance.PanelPopDown(this.transform);
        });
    }

    private void LoadStartScene()
    {
    
        MoveController.isInputLockedStatic = false;
        LoadSceneManager.Instance.LoadSceneAsync("StartScene");
    }
}
