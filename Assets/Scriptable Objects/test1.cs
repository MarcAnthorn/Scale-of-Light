using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    [SerializeField]
    private Enemy data1;
    // Start is called before the first frame update
    void Start()
    {
        data1.health--;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Data1 " + data1.health);
    }
}
