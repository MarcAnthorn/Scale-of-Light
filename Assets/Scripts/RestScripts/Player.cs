using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public GameObject player;
    public ParticleSystem particle;
    private Material _material;

    // Start is called before the first frame update
    void Start()
    {
        _material = player.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Die()
    {
        _material.DOFloat(-1, "_Strength", 0.8f);
        particle.Play();
    }
}
