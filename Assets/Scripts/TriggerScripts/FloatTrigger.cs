using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class FloatTrigger : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1.5f)]
    private float floatTime;

    private BoxCollider2D bc;
    private void Awake()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("MinSize"))
        {
            //触发机关动画

            //
            collision.transform.LeanMoveLocalY(collision.gameObject.transform.position.y + 2, floatTime).setEase(LeanTweenType.easeInQuart)
            .setOnComplete(() =>
            {
                
            });
        }

    }
}
