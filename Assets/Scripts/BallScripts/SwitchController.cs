using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            GameObject nowBall = SwitchManager.Instance.FetchBallInSequence();
            if (nowBall == null)
                return;
            EventHub.Instance.EventTrigger<Transform>("SwitchControlled", nowBall.transform);
        }
    }
}
