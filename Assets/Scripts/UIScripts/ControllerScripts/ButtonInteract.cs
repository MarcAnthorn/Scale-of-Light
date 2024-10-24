using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(EventTrigger))]
public class ButtonInteract : MonoBehaviour
{
    private EventTrigger trigger;
    private EventTrigger.Entry entryEnter = new EventTrigger.Entry();
    private EventTrigger.Entry entryExit = new EventTrigger.Entry();

    private Vector3 transformVector = new Vector3(1.1f, 1.1f, 1.1f);
    private Vector3 originalVector = new Vector3(1f, 1f, 1f);

    private Button btnSelf;

    private void Awake()
    {
        EnterExitTransform();
        ClickTransform();
    }


    //鼠标移除按钮范围之后的动态效果
    private void EnterExitTransform()
    {
        trigger = this.GetComponent<EventTrigger>();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) =>
        {
            SoundEffectManager.Instance.PlaySoundEffect("Sound/ButtonEntry");
            transform.LeanScale(transformVector, 0.4f);
        });
        trigger.triggers.Add(entryEnter);

        entryExit.eventID = EventTriggerType.PointerExit;
        entryExit.callback.AddListener((data) =>
        {
            transform.LeanScale(originalVector, 0.4f);
        });
        trigger.triggers.Add(entryExit);
    }


    //点击按钮之后的动态反馈
    private void ClickTransform()
    {
      
        btnSelf = this.GetComponent<Button>();
        btnSelf.onClick.AddListener(() =>
        {
            SoundEffectManager.Instance.PlaySoundEffect("Sound/ButtonClickConfirm");
            if (!btnSelf.gameObject.name.Equals("GameStartButton") && !btnSelf.gameObject.name.Equals("BackButton"))
            {
                btnSelf.interactable = false;
                LeanTween.delayedCall(0.5f, () => {
                    btnSelf.interactable = true;
                });
            }
            transform.LeanScale(originalVector, 0.13f).setLoopPingPong(1).setOnComplete(() =>
            {
                //使用事件中心进行事件分发；
                switch (this.gameObject.name)
                {
                    case "SettingButton":
                        SoundEffectManager.Instance.PlaySoundEffect("Sound/PanelRevealVer1");
                        Debug.Log("SettingButtonlogic executed");
                        EventHub.Instance.EventTrigger("SettingPanelPopUp");
                        break;
                    case "QuitButton":
                        Debug.Log("QuitButtonlogic executed");
                        break;
                    case "AboutButton":
                        SoundEffectManager.Instance.PlaySoundEffect("Sound/PanelRevealVer1");
                        Debug.Log("AboutButtonlogic executed");
                        break;
                    default:
                        break;
                }

            });

        });
    }




}
