using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HiddenRoomTrigger : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera hiddenRoomCamera;

    private int index = 0;

    private void Start()
    {
        index = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MediumSize"))
        {
            index++;
            if (index % 2 == 1)
            {
                hiddenRoomCamera.gameObject.SetActive(true);
            }
            else
            {
                hiddenRoomCamera.gameObject.SetActive(false);
            }

        }
    }

  

}
