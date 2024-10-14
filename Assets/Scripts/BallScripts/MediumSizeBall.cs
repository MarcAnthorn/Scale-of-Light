using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediumSizeBall : BaseDivideBall
{
    private SpriteRenderer sr;
    private Color currentColor;

    [SerializeField]
    private int gravity;

    private GameObject childLeft;
    private GameObject childRight;

    //[SerializeField]
    //private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private bool divideLock = false;

    public void LockDivideOrNot(bool isLock)
    {
        divideLock = isLock;
    }


    private void Awake()
    {
        ChangeChildName();
        sr = this.GetComponentInChildren<SpriteRenderer>();
        currentColor = sr.color;

        LeanTween.value(this.gameObject, currentColor.a, 1, 0.55f)
           .setOnUpdate((float alpha) =>
           {
               // 在插值过程中更新 SpriteRenderer 的 Alpha 值
               currentColor.a = alpha;
               sr.color = currentColor;
           });

        EventHub.Instance.EventTrigger<Transform>("SwitchControlled", this.transform);
    }

    private void ChangeChildName()
    {
        this.gameObject.name = "MediumSizeBall" + SwitchManager.Instance.ballIndex;
    }



    private void OnEnable()
    {
        divideLock = true;
        Color color = sr.color;
        color.a = 255f;
        sr.color = color;

        EventHub.Instance.AddEventListener("MediumDivide", MediumDivide);
    }

    private void OnDisable()
    {
        SwitchManager.Instance.DeleteBallFromQueue(this.gameObject);
        EventHub.Instance.RemoveEventListener("MediumDivide", MediumDivide);
    }

   

    private void MediumDivide()
    {
        if (divideLock)
            return;
        divideLock = true;
        LeanTween.value(gameObject, currentColor.a, 0, 0.55f)
           .setOnUpdate((float alpha) =>
           {
               // 在插值过程中更新 SpriteRenderer 的 Alpha 值
               currentColor.a = alpha;
               sr.color = currentColor;
           }).setOnComplete((arg) =>
           {
               childLeft = SwitchManager.Instance.AddNewMinToQueue(this.transform.position);
               childRight = SwitchManager.Instance.AddNewMinToQueue(this.transform.position);

               childLeft.transform.LeanMoveX(this.transform.position.x - 0.7f, this.transform.position.y + 0.8f);
               childRight.transform.LeanMoveX(this.transform.position.x + 0.7f, this.transform.position.y + 0.8f);
               EventHub.Instance.EventTrigger<Transform>("SwitchControlled", childRight.transform);
               PoolManager.Instance.ReturnToPool("MaxSizeBall", this.gameObject);

           });


    }

}
