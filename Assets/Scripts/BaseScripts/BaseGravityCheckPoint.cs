using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class BaseGravityCheckPoint : MonoBehaviour
{
    protected BoxCollider2D boxCollider;

    protected void Awake()
    {
        boxCollider = this.GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision Checked");
        if (collision.gameObject.CompareTag("MaxSize") || collision.gameObject.CompareTag("MediumSize") || collision.gameObject.CompareTag("MinSize"))
            TriggerEnterLogic(collision);
            
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Collision Exits");
        if (collision.gameObject.CompareTag("MaxSize") || collision.gameObject.CompareTag("MediumSize") || collision.gameObject.CompareTag("MinSize"))
            TriggerExitLogic(collision);
    }

    protected abstract void TriggerEnterLogic(Collider2D collision);

    protected abstract void TriggerExitLogic(Collider2D collision);


}
