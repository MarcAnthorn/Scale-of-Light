using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        EventHub.Instance.AddEventListener("DoorOpen", DoorOpen);
    }

    private void OnDisable()
    {
        EventHub.Instance.RemoveEventListener("DoorOpen", DoorOpen);
    }

    private void DoorOpen()
    {
        //播放机关门的开启动画
        Debug.Log("The Door Is Opened");
        
    }
}
