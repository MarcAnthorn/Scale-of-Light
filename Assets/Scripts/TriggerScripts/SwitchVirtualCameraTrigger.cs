using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SwitchVirtualCameraTrigger : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera corridorCamera;
    private int countIndex = 0;
    private void Start()
    {
        countIndex = 0;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("MediumSize") || collision.gameObject.CompareTag("MinSize"))
        {
            EventHub.Instance.EventTrigger<bool>("LockDivide", false);
            countIndex++;
            if(countIndex % 2 == 1)
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
