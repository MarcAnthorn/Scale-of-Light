using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchNotice : MonoBehaviour
{
 

    private Color currentColor;
    private SpriteRenderer sr;

    [SerializeField]
    [Range(0, 1)]
    private float existTime;

    private void Awake()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        //重置alpha值为0；
        currentColor = sr.color;
        currentColor.a = 0;
        sr.color = currentColor;
        LeanTween.value(this.gameObject, currentColor.a, 1, 0.15f)
          .setOnUpdate((float alpha) =>
          {
              // 在插值过程中更新 SpriteRenderer 的 Alpha 值
              currentColor.a = alpha;
              sr.color = currentColor;
          }).setOnComplete(() =>
          {
              LeanTween.delayedCall(existTime, () =>
              {
                  //指定时间后开始消失：
                  LeanTween.value(this.gameObject, currentColor.a, 0, 0.15f)
                   .setOnUpdate((float alpha) =>
                   {
                       // 在插值过程中更新 SpriteRenderer 的 Alpha 值
                       currentColor.a = alpha;
                       sr.color = currentColor;
                   }).setOnComplete(() =>
                   {
                       PoolManager.Instance.ReturnToPool(this.gameObject.name, this.gameObject);
                   });
              });
             
          });

      

    }
}
