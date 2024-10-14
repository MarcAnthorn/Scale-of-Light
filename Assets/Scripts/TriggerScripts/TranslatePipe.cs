using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TranslatePipe : BasePipe
{

    [SerializeField]
    [Range(0 , 2)]
    private float spawnDelayTime = 0;

    private GameObject nowControlledBall;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField]
    private Transform spawnPoint;

    private Transform fakeLookAt;

    //确认管道的进入逻辑；
    protected override void PipeTriggerEnter(Transform ballTransform)
    {
        GameObject ball = ballTransform.gameObject;


        //动画播放完毕之后再移动镜头
        //fakeLookAt = PoolManager.Instance.SpawnFromPool("FakeLookAt", ballTransform.position, Quaternion.identity).transform;
        //EventHub.Instance.EventTrigger<Transform>("SwitchLookAt", fakeLookAt);
        //fakeLookAt.LeanMoveLocalX(spawnPoint.position.x, spawnDelayTime).setOnComplete(() =>
        //{
        //    PoolManager.Instance.ReturnToPool("FakeLookAt", fakeLookAt.gameObject);
        //});
        
        if(ball.CompareTag("MaxSize"))
            BaseBallEntry(ballTransform);
        else if(ball.CompareTag("MinSize") || ball.CompareTag("MediumSize"))
            ChildBallEntry(ballTransform);


    }

    //触发管道提示UI；
    protected override void PipeNotification()
    {
        if (GameObject.Find("PipeEntryNote") != null)
            return;
        GameObject note = PoolManager.Instance.SpawnFromPool("PipeEntryNote", Vector3.zero, Quaternion.identity);
        note.gameObject.name = "PipeEntryNote";
        EventHub.Instance.EventTrigger<GameObject>("RevealPipeNote", note);
        Debug.Log("InputDetect is opened");
        EventHub.Instance.EventTrigger<bool>("UnlockOrLockPipeEnter", true);
    }

    protected override void PipeTriggerExit()
    {
        //执行一次范围检测，如果当前的触发器范围之内还有球，那么提示性UI就不会消失；
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(new Vector2(20, -1f), new Vector2(1.28f, 3.51f), 0f);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("MaxSize") || hitCollider.gameObject.CompareTag("MediumSize") || hitCollider.gameObject.CompareTag("MinSize"))
                return;
        }

        EventHub.Instance.EventTrigger("HidePipeNote");
        Debug.Log("InputDetect is closed");
        EventHub.Instance.EventTrigger<bool>("UnlockOrLockPipeEnter", false);
    }

    private void Awake()
    {
        EventHub.Instance.AddEventListener<Transform>("PipeTriggerEnter", PipeTriggerEnter);
    }


    private void BaseBallEntry(Transform ballTransform)
    {
        //大球进管道动画
        PoolManager.Instance.ReturnToPool("MaxSizeBall",ballTransform.gameObject);

        LeanTween.delayedCall(spawnDelayTime, () =>
        {
            nowControlledBall = SwitchManager.Instance.AddNewMediumToQueue(spawnPoint.position);
            EventHub.Instance.EventTrigger<Transform>("SwitchControlled", nowControlledBall.transform);
        });
    }

    private void ChildBallEntry(Transform ballTransform)
    {
        //小球进管道动画
       if(ballTransform.gameObject.CompareTag("MinSize"))
       {
            PoolManager.Instance.ReturnToPool(ballTransform.gameObject.name, ballTransform.gameObject);
            LeanTween.delayedCall(spawnDelayTime, () =>
            {
                nowControlledBall = SwitchManager.Instance.AddNewMinToQueue(spawnPoint.position);
                EventHub.Instance.EventTrigger<Transform>("SwitchControlled", nowControlledBall.transform);
            });
       }


        //中球进管道动画
        else
        {
            PoolManager.Instance.ReturnToPool(ballTransform.gameObject.name, ballTransform.gameObject);
            LeanTween.delayedCall(spawnDelayTime, () =>
            {
                nowControlledBall = SwitchManager.Instance.AddNewMinToQueue(spawnPoint.position);
                EventHub.Instance.EventTrigger<Transform>("SwitchControlled", nowControlledBall.transform);
            });

        }


    }
}
