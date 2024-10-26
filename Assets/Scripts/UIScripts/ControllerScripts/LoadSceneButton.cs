using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadSceneButton : MonoBehaviour
{
    private EventTrigger trigger;
    private EventTrigger.Entry entryEnter = new EventTrigger.Entry();
    private Button btnSelf;
    private void Awake()
    {
        EventHub.Instance.AddEventListener<float>("LoadSceneProgress", LoadSceneProgress);
        btnSelf = this.GetComponent<Button>();


        trigger = this.GetComponent<EventTrigger>();
        entryEnter.eventID = EventTriggerType.PointerEnter;
        entryEnter.callback.AddListener((data) =>
        {
            SoundEffectManager.Instance.PlaySoundEffect("Sound/ButtonEntryVer2");
        });
        trigger.triggers.Add(entryEnter);
    }

    private void Start()
    {
        btnSelf.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChoosePanel>(ConfirmScene);
            
        });
    }
    public void LoadSceneProgress(float process)
    {
        Debug.Log(process);
    }

    public void ConfirmScene()
    {
        switch (btnSelf.name)
        {
            case "LevelOneButton":
                LoadSceneManager.Instance.LoadSceneAsync("GameScene1");
                break;
            case "LevelTwoButton":
                LoadSceneManager.Instance.LoadSceneAsync("GameScene2");
                break;
            case "LevelThreeButton":
                LoadSceneManager.Instance.LoadSceneAsync("GameScene3");
                break;
            case "LevelFourButton":
                LoadSceneManager.Instance.LoadSceneAsync("GameScene4");
                break;
            case "LevelFiveButton":
                LoadSceneManager.Instance.LoadSceneAsync("GameScene5");
                break;
        }
    }
}
