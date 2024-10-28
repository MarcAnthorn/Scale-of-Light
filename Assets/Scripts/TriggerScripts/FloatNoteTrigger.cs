using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatNoteTrigger : MonoBehaviour
{
    [SerializeField]
    private Transform basePoint;
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MinSize"))
        {
            PoolManager.Instance.SpawnFromPool("FloatNote", basePoint.position, Quaternion.identity).gameObject.transform.SetParent(basePoint);

        }
    }
}
