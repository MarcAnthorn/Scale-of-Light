using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

//[RequireComponent(typeof(BasicBallLogic))]
public class MaxSizeBall : BaseDivideBall
{
    private SpriteRenderer sr;
    private Color currentColor;

    private GameObject childLeft;
    private GameObject childRight;

    [SerializeField]
    private int gravity;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private bool divideLock = false;

    /// <summary>
    /// 是否将该脚本中的divideLock开启或者是关闭；
    /// </summary>
    /// <param name="isLock">true是执行分裂锁，false是关闭分裂锁</param>
    public void LockDivideOrNot(bool isLock)
    {
        divideLock = isLock;
    }



    private void Awake()
    {
        sr = this.GetComponentInChildren<SpriteRenderer>();
        EventHub.Instance.EventTrigger<Transform>("SwitchControlled", this.transform);
        currentColor = sr.color;
        divideLock = false;
    }


    //所有加入队列都是在生成实例的时候实现了，不要再在OnEnable中再加入队列了；
    private void OnEnable()
    {
        Color color = sr.color;
        color.a = 255f; 
        sr.color = color;
        divideLock = true;

        EventHub.Instance.AddEventListener("MaxDivide", MaxDivide);
    }

    private void OnDisable()
    {
        SwitchManager.Instance.DeleteBallFromQueue(this.gameObject);
        EventHub.Instance.RemoveEventListener("MaxDivide", MaxDivide);
    }


    private void MaxDivide()
    {
        Debug.Log("MaxDivide is executed");
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
               childLeft = SwitchManager.Instance.AddNewMediumToQueue(this.transform.position);
               childRight = SwitchManager.Instance.AddNewMediumToQueue(this.transform.position);

               // 获取Player层的层级索引
               int layerPlayer = LayerMask.NameToLayer("Player");
               // 禁用这两个层之间的力的作用（仍然进行碰撞检测）
               Physics2D.IgnoreLayerCollision(layerPlayer, layerPlayer, true);
           
               childLeft.transform.LeanMoveX(this.transform.position.x - 1f, 0.2f);
               childRight.transform.LeanMoveX(this.transform.position.x + 1f, 0.2f);
               LeanTween.delayedCall(0.2f, () =>
               {
                   Physics2D.IgnoreLayerCollision(layerPlayer, layerPlayer, false);
               });

               EventHub.Instance.EventTrigger<Transform>("SwitchControlled", childRight.transform);
               PoolManager.Instance.ReturnToPool("MaxSizeBall", this.gameObject);
              
           });

           
    }


   


   

}
