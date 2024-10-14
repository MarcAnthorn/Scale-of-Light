using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    [SerializeField]
    [Range(1, 8)]
    private int health;
    public int Health => health;


    private TextMeshProUGUI text = null;

    private void Awake()
    {     
        text = this.GetComponent<TextMeshProUGUI>();
        text.SetText(text.text, health);
    }

    private void OnEnable()
    {
        EventHub.Instance.AddEventListener<int>("HealthDamage", HealthDamage);
        EventHub.Instance.AddEventListener("Death", Death);
    }

    private void OnDisable()
    {
        EventHub.Instance.RemoveEventListener<int>("HealthDamage", HealthDamage);
        EventHub.Instance.RemoveEventListener("Death", Death);
    }

    public void HealthDamage(int delta = 1)
    {
        health -= delta;
        text.SetText("Health Point: {0}", health);
        Debug.Log("Damage is triggered");
        if (health <= 0)
        {
            //Death();
            Debug.Log("Now Health: " + health);
            text.SetText("Health Point: {0}", 0);
        }
    }

    public void Death()
    {
        Debug.Log("You are Dead");
    }
}
