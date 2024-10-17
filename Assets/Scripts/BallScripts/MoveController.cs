using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private Transform nowControlled;
    //private CinemachineVirtualCamera virtualCamera;
    private BasicBallLogic nowBallLogicScript;
    private BaseDivideBall nowDivideScript;

    private bool isMaxSize;


    private void Awake()
    {
        //virtualCamera = this.GetComponent<CinemachineVirtualCamera>();
      
    }

    private void OnEnable()
    {
        EventHub.Instance.AddEventListener<Transform>("SwitchControlled", SwitchControlled);
        //EventHub.Instance.AddEventListener<Transform>("SwitchLookAt", SwitchLookAt);
    }

    private void OnDisable()
    {
        EventHub.Instance.RemoveEventListener<Transform>("SwitchControlled", SwitchControlled);
        //EventHub.Instance.RemoveEventListener<Transform>("SwitchLookAt", SwitchLookAt);

    }




    private void SwitchControlled(Transform ball)
    {

        //切换之后，通过事件中心传出当前的控制对象的动画状态机，便于外部调用对应的逻辑；
        EventHub.Instance.EventTrigger<Animator>("FetchAnimatorNowControlled", ball.GetComponentInChildren<Animator>());

        if (nowControlled == null)
        {
            nowBallLogicScript = ball.GetComponent<BasicBallLogic>();
            nowControlled = ball;
            nowBallLogicScript.ifInputDetect = true;
          
            //virtualCamera.Follow = nowControlled;
        }
        else
        {
            //将上一个操作的球的读取输入关闭；
            nowBallLogicScript.ifInputDetect = false;
            Debug.Log("Now Banned Ball is " + nowBallLogicScript.gameObject.name);

            //将下一个传入的球的读取输入开启；
            nowControlled = ball;
            nowBallLogicScript = ball.GetComponent<BasicBallLogic>();
            Debug.Log("Now Controlled Ball is " + nowBallLogicScript.gameObject.name);
            nowBallLogicScript.ifInputDetect = true;
            //virtualCamera.Follow = nowControlled;
        }

        //处理分裂的逻辑；
        if(ball.gameObject.CompareTag("MaxSize") || ball.gameObject.CompareTag("MediumSize"))
        {        
            if (nowDivideScript == null)
            {
                isMaxSize = ball.gameObject.CompareTag("MaxSize");
                nowDivideScript = isMaxSize ? ball.GetComponent<MaxSizeBall>() : ball.GetComponent<MediumSizeBall>();
                if (isMaxSize)
                    (nowDivideScript as MaxSizeBall).LockDivideOrNot(false);
                else
                    (nowDivideScript as MediumSizeBall).LockDivideOrNot(false);
            }

            //如果不为空，那么就是上一个分裂关闭，当前传入的分裂开启；
            else
            {
                //关闭上一个脚本实例的分裂逻辑；
                if (isMaxSize)
                    (nowDivideScript as MaxSizeBall).LockDivideOrNot(true);
                else
                    (nowDivideScript as MediumSizeBall).LockDivideOrNot(true);

                isMaxSize = ball.gameObject.CompareTag("MaxSize");
                nowDivideScript = isMaxSize ? ball.GetComponent<MaxSizeBall>() : ball.GetComponent<MediumSizeBall>();
                //开启当前的分裂逻辑；
                if (isMaxSize)
                    (nowDivideScript as MaxSizeBall).LockDivideOrNot(false);
                else
                    (nowDivideScript as MediumSizeBall).LockDivideOrNot(false);
            }
        }

    }
  
    //private void SwitchLookAt(Transform target)
    //{
    //    virtualCamera.Follow = target;
    //}
}
