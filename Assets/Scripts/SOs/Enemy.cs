using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyAbout")]
public class Enemy : ScriptableObject
{
    [Range(0, 10)]
    [SerializeField]
    public int health;

    [SerializeField]
    private int indexOfEnemy;

}
