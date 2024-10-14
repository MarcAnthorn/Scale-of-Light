using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BasicBallLogic))]
public abstract class BaseChild : MonoBehaviour
{
    protected  SpriteRenderer sr;
    protected Color currentColor;

    [SerializeField]
    protected int gravity;
   

    [SerializeField]
    protected int ballIndex = 0;
   

    protected virtual void Awake()
    {
        sr = this.GetComponentInChildren<SpriteRenderer>();
        currentColor = sr.color;

        LeanTween.value(this.gameObject, currentColor.a, 1, 0.55f)
           .setOnUpdate((float alpha) =>
           {
               // 在插值过程中更新 SpriteRenderer 的 Alpha 值
               currentColor.a = alpha;
               sr.color = currentColor;
           });

        ChangeChildName();
    }
    protected virtual void OnEnable()
    {
        Color color = sr.color;
        color.a = 255f;
        sr.color = color;
    }


    protected virtual void OnDisable()
    {
        SwitchManager.Instance.DeleteBallFromQueue(this.gameObject);
    }

    protected abstract void ChangeChildName();
    
}
