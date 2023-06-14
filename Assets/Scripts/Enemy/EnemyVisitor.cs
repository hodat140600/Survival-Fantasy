using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(0)]
public class EnemyVisitor : MonoBehaviour/*, IEnemyElement*/
{
    public EnemyStatus enemyStatus;

    [Header("Status")]
    public int damage;
    public float speed;
    public int health; // Percentage
    public int maxHealth;
    public int expPoint;
    public event Action<float> OnHealthPctChanged;
    private EnemyBehavior _enemyBehavior;

    public void Attack(GameObject player)
    {
        _enemyBehavior.liveSkillBehavior.TakeDamage(damage);
        //Debug.Log("Weapon fired!");
    }
    public float TakeDamage(int damage)
    {
        damage = (health < damage) ? damage = health : damage;
        health -= damage;
        //health = health <= 0 ? 0 : health;
        float currentHealthPct = (float)health / (float)maxHealth;
        OnHealthPctChanged?.Invoke(currentHealthPct);
        return damage;
    }
    private void OnEnable()
    {
        enemyStatus.Visit(this);
        if(_enemyBehavior == null)
        {
            _enemyBehavior = GetComponent<EnemyBehavior>();
        }
        if (_enemyBehavior.IsBoss)
        {
            gameObject.GetComponentInChildren<BossHealthSlider>().Init(this);
        }
        health = maxHealth;
        _enemyBehavior.InitEnemyStatus(this);
    }
}
