using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class MoveController : MonoBehaviour
{
    private Transform nowControlled;
    //private CinemachineVirtualCamera virtualCamera;
    private BasicBallLogic nowBallLogicScript;
    private BaseDivideBall nowDivideScript;
    private Rigidbody2D nowrb;

    //材质变量
    private Material _material;

    //全局唯一的静态变量，用于在特定的场合（如死亡时）取消所有的键盘输入；
    public static bool isInputLockedStatic = false;

    private bool isMaxSize;

    //当前的QuitConfirmPanel的引用；
    private GameObject panel;

    //如果此时检测到玩家按下esp键，暂停游戏时间，以及弹出是否退出的Confirm面板
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isInputLockedStatic = true;
            panel = UIManager.Instance.ShowPanel<QuitConfirmPanel>().gameObject;
            PanelEffects.Instance.PanelPopUp(panel.transform);
            LeanTween.delayedCall(0.3f, () =>
            {
                Time.timeScale = 0;
            });

        }

    }

    private void Awake()
    {
        //virtualCamera = this.GetComponent<CinemachineVirtualCamera>();
      
    }


    private void OnEnable()
    {
        EventHub.Instance.AddEventListener<Transform>("SwitchControlled", SwitchControlled);
        EventHub.Instance.AddEventListener("BeforeSwitchScene", BeforeSwitchScene);

        //EventHub.Instance.AddEventListener<Transform>("SwitchLookAt", SwitchLookAt);
    }

    private void OnDisable()
    {
        EventHub.Instance.RemoveEventListener<Transform>("SwitchControlled", SwitchControlled);
        EventHub.Instance.RemoveEventListener("BeforeSwitchScene", BeforeSwitchScene);

        //EventHub.Instance.RemoveEventListener<Transform>("SwitchLookAt", SwitchLookAt);

    }


    private void SwitchControlled(Transform ball)
    {

        //切换之后，通过事件中心传出当前的控制对象的动画状态机，便于外部调用对应的逻辑；
        EventHub.Instance.EventTrigger<Animator>("FetchAnimatorNowControlled", ball.GetComponentInChildren<Animator>());
       

        if (nowControlled == null)
        {
            nowBallLogicScript = ball.GetComponent<BasicBallLogic>();

            //获取当前的对象材质；
            _material = nowBallLogicScript.gameObject.GetComponentInChildren<Renderer>().material;

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

            //获取当前的对象材质；
            _material = nowBallLogicScript.gameObject.GetComponentInChildren<Renderer>().material;
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

    private void BeforeSwitchScene()
    {
        nowrb = nowBallLogicScript.gameObject.GetComponent<Rigidbody2D>();
        nowrb.constraints = RigidbodyConstraints2D.FreezePosition;
        switch (nowBallLogicScript.gameObject.tag)
        {
            case "MaxSize":
                SpawnParticals("Effect/DeathEffMax", nowBallLogicScript.gameObject.transform.position);
                break;
            case "MediumSize":
                SpawnParticals("Effect/DeathEffMedium", nowBallLogicScript.gameObject.transform.position);
                break;
            case "MinSize":
                SpawnParticals("Effect/DeathEffMin", nowBallLogicScript.gameObject.transform.position);
                break;
        }

    }

    private void SpawnParticals(string key, Vector3 _position)
    {

        // 修改透明度
        _material.DOFloat(-1, "_Strength", 0.8f);

        //创建粒子特效
        PoolManager.Instance.SpawnFromPool(key).gameObject.transform.position = _position; 
    }


    //private void SwitchLookAt(Transform target)
    //{
    //    virtualCamera.Follow = target;
    //}
}
