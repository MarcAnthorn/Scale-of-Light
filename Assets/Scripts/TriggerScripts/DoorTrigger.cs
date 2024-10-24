using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        EventHub.Instance.AddEventListener("DoorOpen", DoorOpen);
        EventHub.Instance.AddEventListener("DoorClose", DoorClose);
    }

    private void OnDisable()
    {
        EventHub.Instance.RemoveEventListener("DoorOpen", DoorOpen);
        EventHub.Instance.RemoveEventListener("DoorClose", DoorClose);
    }

    private void DoorOpen()
    {
        //播放机关门的开启动画
        Debug.Log("The Door Is Opened");
        //this.gameObject.SetActive(false);
    }


    //自己失活了自然不可能再响应任何事件中心的事件发布；
    //但是到时候不是通过失活的方式进行门的开启的，而是通过动画的方式开启；
    //动画会让门锁到地下或者是地上，从而实现模拟门的效果
    
    private void DoorClose()
    {
        Debug.Log("The Door Is Closed");
        //this.gameObject.SetActive(true);
    }
}
