using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AboutPanel : BasePanel
{

    [SerializeField]
    private Button btnClose;
    protected override void Init()
    {
        btnClose.onClick.AddListener(() =>
        {
            SoundEffectManager.Instance.PlaySoundEffect("Sound/PanelRevealVer1");
            PanelEffects.Instance.PanelPopDown(this.transform);
        });
    }


}
