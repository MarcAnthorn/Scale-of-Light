using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MaskTransform : MonoBehaviour
{
    private Vector2 transformVector = new Vector2(1, 1);
    private RectTransform rect;
    private UnityAction action;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //加载场景的时候调用的方法：
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 检查加载的场景名称
        if (scene.name == "StartScene")
        {
            // 仅在加载特定场景时执行逻辑
            //因为每次切换场景EventHub都会清空事件，但是mask是过场景不移除的，因此如果不这样写事件就加不回来，就不能被调用；
            EventHub.Instance.AddEventListener<UnityAction>("MaskTransformation", MaskTransformation);
            
        }
    }


    /// <summary>
    /// 执行遮罩变化的方法
    /// </summary>
    /// <param name="_action">执行完遮罩变化之后的函数，用于处理遮罩结束之后的逻辑</param>
    public void MaskTransformation(UnityAction _action)
    {
        Debug.Log("MaskTransform Is Called!");
        rect = this.GetComponent<RectTransform>();
        action = _action;
        LeanTween.value(gameObject, rect.sizeDelta.x, 2700, 0.35f).setOnUpdate((float value) =>
        {
            rect.sizeDelta = transformVector * value;
        }).setOnComplete(CompleteAction);
    }

    private void CompleteAction()
    {
        LeanTween.delayedCall(0.1f, () =>
        {
            action?.Invoke();
            rect.sizeDelta = Vector2.zero;
        });
    }

}
