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

    //提示性UI对象；
    private GameObject note;

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

        SoundEffectManager.Instance.PlaySoundEffect("Sound/Teleport");
        if(ball.CompareTag("MaxSize"))
            BaseBallEntry(ballTransform);
        else if(ball.CompareTag("MinSize") || ball.CompareTag("MediumSize"))
            ChildBallEntry(ballTransform);


    }

    //触发管道提示UI；
    protected override void PipeNotification()
    {
        //只有当进入了某个管道的提示范围，才会让该管道监听进入管道事件；
        EventHub.Instance.AddEventListener<Transform>("PipeTriggerEnter", PipeTriggerEnter);

        if (note != null)
            return;
        note = PoolManager.Instance.SpawnFromPool("PipeEntryNote", Vector3.zero, Quaternion.identity);
        RevealPipeNote();
        Debug.Log("InputDetect is opened");
        EventHub.Instance.EventTrigger<bool>("UnlockOrLockPipeEnter", true);
    }

    //显示管道提示性UI的方法；
    private void RevealPipeNote()
    {
        note.transform.position = target.position + spawnOffset;

        sr = note.GetComponent<SpriteRenderer>();

        //重置alpha值为0；
        currentColor = sr.color;
        currentColor.a = 0;
        sr.color = currentColor;

        LeanTween.value(note, currentColor.a, 1, 0.55f)
          .setOnUpdate((float alpha) =>
          {
              // 在插值过程中更新 SpriteRenderer 的 Alpha 值
              currentColor.a = alpha;
              sr.color = currentColor;
          }).setOnComplete(() =>
          {
              note.transform.LeanMoveLocalY(target.position.y, floatTime).setEase(LeanTweenType.easeInOutCirc).setLoopPingPong();
          });

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

        HidePipeNote();
        EventHub.Instance.EventTrigger<bool>("UnlockOrLockPipeEnter", false);

        //当前范围没有对象了，才移除事件的监听；
        EventHub.Instance.RemoveEventListener<Transform>("PipeTriggerEnter", PipeTriggerEnter);

    }

    //隐藏提示性UI的方法
    protected void HidePipeNote()
    {
        LeanTween.value(note, currentColor.a, 0, 0.55f)
         .setOnUpdate((float alpha) =>
         {
             // 在插值过程中更新 SpriteRenderer 的 Alpha 值
             currentColor.a = alpha;
             sr.color = currentColor;
         }).setOnComplete(() =>
         {
             LeanTween.cancel(note);
             //销毁而不是回池；
             Destroy(note);
         });

    }


    private void OnDisable()
    {
        EventHub.Instance.RemoveEventListener<Transform>("PipeTriggerEnter", PipeTriggerEnter);
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
                LeanTween.delayedCall(0.3f, () =>
                {
                    EventHub.Instance.EventTrigger("Death");
                });
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
