using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseBackButton : MonoBehaviour
{
    private Transform maskTransform;
    private Transform behindTransform;
    private Transform nowShownPanelTransform;

    private Button btnChooseBack;

    private void Awake()
    {
        btnChooseBack = this.GetComponent<Button>();
       
        maskTransform = GameObject.Find("FakeMask").transform;
        behindTransform = GameObject.Find("FakeBehindMask").transform;
    }

    private void OnEnable()
    {
        btnChooseBack.interactable = false;
        LeanTween.delayedCall(1f, () =>
        {
            btnChooseBack.interactable = true;
        });

    }

    private void Start()
    {
        btnChooseBack.onClick.AddListener(() =>
        {
            nowShownPanelTransform = UIManager.Instance.ShowPanel<BeginPanel>().gameObject.transform;
            nowShownPanelTransform.SetParent(maskTransform);
            EventHub.Instance.EventTrigger<UnityAction>("MaskTransformation", StartButtonAfterMask);
        });
    }

    private void StartButtonAfterMask()
    {
        nowShownPanelTransform.SetParent(behindTransform);
        UIManager.Instance.InstantHidePanel<ChoosePanel>();
    }


}
