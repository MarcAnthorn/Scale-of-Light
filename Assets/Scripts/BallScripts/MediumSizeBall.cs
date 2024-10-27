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

    [SerializeField]
    private Vector3 divideForce;

    public void LockDivideOrNot(bool isLock)
    {
        divideLock = isLock;
    }

    //这个是由事件中心发布的是否开启分裂锁，或者是关闭；
    private void LockDivide(bool _isLock)
    {
        divideLock = _isLock;
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
        EventHub.Instance.AddEventListener<bool>("LockDivide", LockDivide);
    }

    private void OnDisable()
    {
        SwitchManager.Instance.DeleteBallFromQueue(this.gameObject);
        EventHub.Instance.RemoveEventListener("MediumDivide", MediumDivide);
        EventHub.Instance.RemoveEventListener<bool>("LockDivide", LockDivide);
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

               // 获取Player层的层级索引
               int layerPlayer = LayerMask.NameToLayer("Player");
               // 禁用这两个层之间的力的作用（仍然进行碰撞检测）
               Physics2D.IgnoreLayerCollision(layerPlayer, layerPlayer, true);

               //由于强制位移可能会导致穿模，因此优化为施加力的方式进行分裂推动；
               Rigidbody2D left = childLeft.GetComponent<Rigidbody2D>();
               left.AddForce(-divideForce, ForceMode2D.Impulse);

               Rigidbody2D right = childRight.GetComponent<Rigidbody2D>();
               right.AddForce(divideForce, ForceMode2D.Impulse);
               LeanTween.delayedCall( 0.12f, () =>
               {
                   Physics2D.IgnoreLayerCollision(layerPlayer, layerPlayer, false);
                   left.velocity = new Vector2(0, 0);
                   right.velocity = new Vector2(0, 0);
               });

               EventHub.Instance.EventTrigger<Transform>("SwitchControlled", childRight.transform);
               PoolManager.Instance.ReturnToPool("MaxSizeBall", this.gameObject);

           });


    }

}
