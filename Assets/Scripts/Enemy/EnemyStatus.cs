using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatus", menuName = "Chapter/EnemyStatus")]
public class EnemyStatus : ScriptableObject/*, IVisitors*/
{
    public string statusName;
    public GameObject statusPrefab;
    public string statusDescription;

    [Range(0.0f, 50f)]
    [Tooltip("Speed for enemy")]
    public float speed; 

    [Range(0.0f, 500000f)]
    [Tooltip("Health for enemy")]
    public int health; 

    [Range(0.0f, 5000f)]
    [Tooltip("Damege for enemy")]
    public int damage;

    [Range(0, 50000)]
    [Tooltip("ExpPoint for enemy")]
    public int expPoint;

    public void Visit(EnemyVisitor enemyStatus)
    {
        enemyStatus.maxHealth = health;
        enemyStatus.health = health;
        enemyStatus.damage = damage;
        enemyStatus.speed = speed;
        enemyStatus.expPoint = expPoint;
    }

    //public void Visit(EnemySpeed enemySpeed)
    //{
    //    enemySpeed.speed = speed;
    //}

    //public void Visit(EnemyDamage enemyDamege)
    //{
    //    enemyDamege.damage = damage;
    //}
}
