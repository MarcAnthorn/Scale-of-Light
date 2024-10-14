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
        if(Input.GetKeyDown(KeyCode.X))
        {
            Die();
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            _material.DOColor(Color.black, "_SlimeColor", 0.5f);
        }
    }

    void Die()
    {
        _material.DOFloat(-1, "_Strength", 0.8f);
        particle.Play();
    }
}
