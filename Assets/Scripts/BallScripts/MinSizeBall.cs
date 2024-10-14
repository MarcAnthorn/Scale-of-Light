using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinSizeBall : BaseChild
{
    protected override void ChangeChildName()
    {
        this.gameObject.name = "MinSizeBall" + SwitchManager.Instance.ballIndex; 
    }

}
