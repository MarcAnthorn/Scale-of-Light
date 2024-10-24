using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SwitchVirtualCameraTrigger : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera corridorCamera;
    private int countIndexMedium = 0;
    
    private void Start()
    {
        countIndexMedium = 0;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("MediumSize"))
        {
            EventHub.Instance.EventTrigger<bool>("LockDivide", false);
            countIndexMedium++;
            if(countIndexMedium % 2 == 1)
            {
                corridorCamera.gameObject.SetActive(false);
            }
            else
            {
                corridorCamera.gameObject.SetActive(true);
            }
        }

      
    }
}
