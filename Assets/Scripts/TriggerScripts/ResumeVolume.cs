using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ResumeVolume : MonoBehaviour
{
    private CircleCollider2D cc;
    private void Awake()
    {
        cc = this.GetComponent<CircleCollider2D>();
        cc.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MinSize") || collision.gameObject.CompareTag("MediumSize"))
            ResumeBallToBase(collision.gameObject.transform);

    }

    private void ResumeBallToBase(Transform childBallTransform)
    {
        if (childBallTransform == null)
            Debug.LogError("childBallTransform is NULL!!!!");
        if(childBallTransform.gameObject.CompareTag("MediumSize"))
        {
            Debug.Log("Medium To Max is Executed");
            GameObject maxSizeBall = SwitchManager.Instance.AddNewMaxToQueue(childBallTransform.position);
            EventHub.Instance.EventTrigger<Transform>("SwitchControlled", maxSizeBall.transform);
            PoolManager.Instance.ReturnToPool(childBallTransform.gameObject.name, childBallTransform.gameObject);
            SwitchManager.Instance.DeleteBallFromQueue(childBallTransform.gameObject);
            PoolManager.Instance.ReturnToPool(this.gameObject.name, this.gameObject);
        }

        else
        {
            GameObject mediumSizeBall = SwitchManager.Instance.AddNewMediumToQueue(childBallTransform.position);
            EventHub.Instance.EventTrigger<Transform>("SwitchControlled", mediumSizeBall.transform);
            PoolManager.Instance.ReturnToPool(childBallTransform.gameObject.name, childBallTransform.gameObject);
            SwitchManager.Instance.DeleteBallFromQueue(childBallTransform.gameObject);
            PoolManager.Instance.ReturnToPool(this.gameObject.name, this.gameObject);
        }
    }
}
