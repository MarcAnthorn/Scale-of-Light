using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCheck3 : BaseGravityCheckPoint
{
    private BasicBallLogic info;
    protected override void TriggerEnterLogic(Collider2D collision)
    {
        //播放对应的重力机关开启动画

        info = collision.gameObject.GetComponent<BasicBallLogic>();
        if(info.Gravity >= 3)
        {
            EventHub.Instance.EventTrigger<int>("Check2", 1);
            Debug.Log("One has been checked");
        }

    }

    protected override void TriggerExitLogic(Collider2D collision)
    {
        //延时播放对应的重力机关关闭动画
        EventHub.Instance.EventTrigger<int>("Check2", -1);
        Invoke("ExitCheckPoint", 1f);
    }

    private void ExitCheckPoint()
    {
        Debug.Log("One has been canceled");
    }





}
