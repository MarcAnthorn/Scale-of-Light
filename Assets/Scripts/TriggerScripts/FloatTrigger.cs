using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class FloatTrigger : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1.5f)]
    private float floatTime;

    //外部关联一个漂浮的高度目标对象；
    [SerializeField]
    private Transform floatTarget;
    private float targetYValue;

    [SerializeField]
    private bool isFall = false;

    private bool isCheckLocked = false;

    private int ballCounts = 0;
    private void Awake()
    {
        ballCounts = 0;
    }

    private BasicBallLogic nowBallScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.CompareTag("MinSize"))
        {
            //浮空特效：（有问题，未显示）
            PoolManager.Instance.SpawnFromPool("Effect/StoneEff", this.transform.position, Quaternion.identity);
            //通过加锁的方式，防止多个小球进入漂浮机关之后能够将通关check重复增加；
            //加锁就是为了让一个漂浮机关只有一个让pointcheck增加的机会；
            if(!isCheckLocked)
            {
                isCheckLocked = true;
                EventHub.Instance.EventTrigger("PointChecked");
            }
            //禁用当前的对象跳跃；
            nowBallScript = collision.GetComponent<BasicBallLogic>();
            nowBallScript.CancelOrResumeJump(false);

            //触发机关动画

            //记录目标高度：
            targetYValue = floatTarget.position.y;
            collision.transform.LeanMoveLocalY(targetYValue, floatTime).setEase(LeanTweenType.easeInQuart)
            .setOnComplete(() =>
            {
                isFall = false;
            });


            //进入漂浮机关的小球数量；
            ballCounts++;
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MinSize") && !isFall)
        {
            collision.transform.position = new Vector3(collision.transform.position.x, targetYValue, collision.transform.position.z);


            // 获取当前对象的刚体
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            // 停止所有的力和速度
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MinSize"))
        {
            ballCounts--;
            //只有当漂浮机关中没有任何一个小球了，才让漂浮失效；
            //同时才能触发让通关check减少，并且开锁：
            if(ballCounts == 0)
            {
                isFall = true;
                EventHub.Instance.EventTrigger("PointUnchecked");
                isCheckLocked = false;
            }
        }
    }
}
