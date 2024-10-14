using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviour
{

    private Transform maskTransform;
    private Transform behindTransform;
    private Transform nowShownPanelTransform;

    private Button btnGameStart;

    private void Awake()
    {
        btnGameStart = this.GetComponent<Button>();
        
        maskTransform = GameObject.Find("FakeMask").transform;
        behindTransform = GameObject.Find("FakeBehindMask").transform;
    }

    private void OnEnable()
    {
        btnGameStart.interactable = false;
        LeanTween.delayedCall(1f, () =>
        {
            btnGameStart.interactable = true;
        });
    }
    private void Start()
    {
        btnGameStart.onClick.AddListener(() =>
        {
            nowShownPanelTransform = UIManager.Instance.ShowPanel<ChoosePanel>().gameObject.transform;
            nowShownPanelTransform.SetParent(maskTransform);
            EventHub.Instance.EventTrigger<UnityAction>("MaskTransformation", BackButtonAfterMask);
        });
    }

    private void BackButtonAfterMask()
    {
        nowShownPanelTransform.SetParent(behindTransform);
        UIManager.Instance.InstantHidePanel<BeginPanel>();
    }

 




}
