using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatNote : MonoBehaviour
{

    private Color currentColor;
    private SpriteRenderer sr;

    [SerializeField]
    [Range(0, 1)]
    private float existTime;

    [SerializeField]
    [Range(0, 3)]
    private float floatHeight;


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
        LeanTween.value(this.gameObject, currentColor.a, 0.4f, 0.2f)
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
                  LeanTween.value(this.gameObject, currentColor.a, 0, 0.2f)
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


        this.transform.LeanMoveLocalY(this.transform.position.y + floatHeight, existTime + 0.4f);


    }
}
