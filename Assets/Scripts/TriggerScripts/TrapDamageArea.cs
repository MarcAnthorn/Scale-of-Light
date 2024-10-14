using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamageArea : BaseTrap
{
    protected override void TrapTriggerEnter()
    {
        EventHub.Instance.EventTrigger<int>("HealthDamage",2);
        Debug.Log("You are damaged");
    }

    protected override void TrapTriggerExit()
    {
        
    }
}
