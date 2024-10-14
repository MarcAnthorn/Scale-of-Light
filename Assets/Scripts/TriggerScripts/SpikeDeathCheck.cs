using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDeathCheck : BaseTrap
{
    protected override void TrapTriggerEnter()
    {
        EventHub.Instance.EventTrigger("Death");
    }

    protected override void TrapTriggerExit()
    {
       
    }
}
