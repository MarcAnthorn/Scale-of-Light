using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    [SerializeField]
    [Range(0, 2)]
    private float switchDelayTime;
    private void OnEnable()
    {
        EventHub.Instance.AddEventListener("Death", Death);

    }

    private void OnDisable()
    {
        EventHub.Instance.RemoveEventListener("Death", Death);
    }


    public void Death()
    {
        //通过事件中心，调用MoveController中的死亡之前的相关逻辑，如死亡动画特效、死亡遮罩特效等等
        EventHub.Instance.EventTrigger("BeforeSwitchScene");
        // 获取当前场景的索引
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //关闭所有的键盘输入：
        MoveController.isInputLockedStatic = true;

        //设定延迟时间，确保之前的所有逻辑都执行结束之后再进行场景的切换；
        LeanTween.delayedCall(switchDelayTime, () =>
        {
            ResetAllStatic();
            // 重载当前场景
            LoadSceneManager.Instance.LoadSceneAsync(currentSceneIndex);
        });
 
    }


    //重载场景之前，需要确保所有的静态管理器和静态变量都是完成重置的，不然会出错；
    private void ResetAllStatic()
    {
        //管理器重置；
        PoolManager.Instance.ClearPool();
        SwitchManager.Instance.ResetIndex();
        EventHub.Instance.ClearListener();
        SoundEffectManager.Instance.ClearSoundEffect();

        //重新开启键盘输入；
        MoveController.isInputLockedStatic = false;
    }
}
