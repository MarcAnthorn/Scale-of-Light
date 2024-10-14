using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public Transform behindTransform;

    private void Awake()
    {
        behindTransform = GameObject.Find("FakeBehindMask").gameObject.transform;
    }

    private void Start()
    {
        Debug.Log("ShowPanel Executed");
        UIManager.Instance.ShowPanel<BeginPanel>().gameObject.transform.SetParent(behindTransform);
    }
}
