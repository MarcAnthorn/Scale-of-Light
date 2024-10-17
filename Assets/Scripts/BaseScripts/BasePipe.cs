using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(BoxCollider2D))]
//[RequireComponent(typeof(Rigidbody2D))]
//如果子对象的 Collider 触发了事件，并且父对象有 Rigidbody，事件会自动传递到 Rigidbody 所在的对象；
//这样就能通过子对象的BoxCollider触发父对象的Trigger响应检测函数了
public abstract class BasePipe : MonoBehaviour
{
    protected BoxCollider2D entryTrigger;
    private BoxCollider2D boxMine;
    private Rigidbody2D rb;

    [SerializeField]
    protected Transform target;

    //提示性UI的偏移
    [SerializeField]
    protected Vector3 spawnOffset;

    [SerializeField]
    [Range(0, 2)]
    protected float floatTime;

    protected Color currentColor;
    protected SpriteRenderer sr;


    protected Vector3 boxSize;

    private void Awake()
    {
        spawnOffset = new Vector3(0, -0.6f, 0);
        floatTime = 0.5f;

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
