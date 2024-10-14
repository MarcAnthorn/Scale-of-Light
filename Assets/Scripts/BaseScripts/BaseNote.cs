using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNote : MonoBehaviour
{
    [SerializeField]
    protected Transform target;

    [SerializeField]
    protected Vector3 spawnOffset;

    [SerializeField]
    [Range(0, 2)]
    protected float floatTime;

    protected Color currentColor;
    protected SpriteRenderer sr;

  

     
    
}
