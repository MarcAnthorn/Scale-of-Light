using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    [SerializeField]
    private int checkTime = 0;

    private void OnEnable()
    {
        EventHub.Instance.AddEventListener<int>("Check1", Check1);
        EventHub.Instance.AddEventListener<int>("Check2", Check2);
    }

    private void OnDisable()
    {
        EventHub.Instance.RemoveEventListener<int>("Check1", Check1);
        EventHub.Instance.RemoveEventListener<int>("Check2", Check2);
    }

    public void Check1(int delta)
    {
        checkTime += delta;
        if(checkTime == 1)
        {
            Passed();
            checkTime = 0;
        }

    }

    public void Check2(int delta)
    {
        Debug.Log("Check 2 has been triggered");
        checkTime += delta;
        if (checkTime == 2)
        {
            Passed();
            checkTime = 0;
        }
    }

    private void Passed()
    {
        //通关相关的逻辑
        Debug.Log("You have passed");
    }
}
