using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class BaseTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MaxSize") || collision.gameObject.CompareTag("MediumSize") || collision.gameObject.CompareTag("MinSize"))
            TrapTriggerEnter();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MaxSize") || collision.gameObject.CompareTag("MediumSize") || collision.gameObject.CompareTag("MinSize"))
            TrapTriggerExit();

    }


    protected BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = this.GetComponent<BoxCollider2D>();
        boxCollider2D.isTrigger = true;
    }

    protected abstract void TrapTriggerEnter();

    protected abstract void TrapTriggerExit();

}
