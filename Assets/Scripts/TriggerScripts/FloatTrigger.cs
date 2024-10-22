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
            if(ballCounts == 0)
                isFall = true;
        }
    }
}
