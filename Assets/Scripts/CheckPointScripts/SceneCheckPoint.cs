using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCheckPoint : MonoBehaviour
{
    private int checkCounts = 0;

    [SerializeField]
    [Range(0, 3)]
    private int targetCheckCounts;
    private void Awake()
    {
        checkCounts = 0;
    }


    private void OnEnable()
    {
        EventHub.Instance.AddEventListener("PointChecked", PointChecked);
        EventHub.Instance.AddEventListener("PointUnchecked", PointUnchecked);
    }

    private void OnDisable()
    {
        EventHub.Instance.RemoveEventListener("PointChecked", PointChecked);
        EventHub.Instance.RemoveEventListener("PointUnchecked", PointUnchecked);
    }

    private void PointChecked()
    {
        checkCounts++;
        Debug.Log(checkCounts);
        if (checkCounts == targetCheckCounts)
            EventHub.Instance.EventTrigger("DoorOpen");
    }

    private void PointUnchecked()
    {
        Debug.Log(checkCounts);
        checkCounts--;
        if(checkCounts < targetCheckCounts)
            EventHub.Instance.EventTrigger("DoorClose");
    }

    
}
