using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BasePipe : MonoBehaviour
{
    protected BoxCollider2D entryTrigger;
    private BoxCollider2D boxMine;
    private Rigidbody2D rb;

    protected Vector3 boxSize;

    private void Awake()
    {
        entryTrigger = GetComponentInChildren<BoxCollider2D>();
        entryTrigger.isTrigger = true;
        boxMine = this.GetComponent<BoxCollider2D>();
        boxMine.isTrigger = false;

        boxSize = entryTrigger.size;

        rb = this.GetComponent<Rigidbody2D>();       
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //触发进入管道的UI提示；
        if (collision.gameObject.CompareTag("MaxSize") || collision.gameObject.CompareTag("MediumSize") || collision.gameObject.CompareTag("MinSize"))
            PipeNotification();
             
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MaxSize") || collision.gameObject.CompareTag("MediumSize") || collision.gameObject.CompareTag("MinSize"))
            PipeTriggerExit();
    }

    protected abstract void PipeTriggerEnter(Transform ballTransform);

    protected abstract void PipeNotification();

    protected abstract void PipeTriggerExit();


    
}
