using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MaskTransform : MonoBehaviour
{
    private Vector2 transformVector = new Vector2(1, 1);
    private RectTransform rect;
    private UnityAction action;
    private void OnEnable()
    {
        EventHub.Instance.AddEventListener<UnityAction>("MaskTransformation", MaskTransformation);
    }

    private void OnDisable()
    {
        EventHub.Instance.RemoveEventListener<UnityAction>("MaskTransformation", MaskTransformation);
    }

    /// <summary>
    /// 执行遮罩变化的方法
    /// </summary>
    /// <param name="_action">执行完遮罩变化之后的函数，用于处理遮罩结束之后的逻辑</param>
    public void MaskTransformation(UnityAction _action)
    {
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
