using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.LiveSkill;
using UnityEngine;

public class BombBehavior : MonoBehaviour, IRewardItem
{
    [SerializeField] private int _damage;
    [SerializeField] private float _radius;

    public void Init(int damage, float radius)
    {
        _damage = damage;
        _radius = radius;
    }

    public void Active()
    {
        Explore();
    }

    private void Explore()
    {
        LayerMask whatIsEnemy = (1 << 7);
        EnemySpawner.Instance.CheckTakeDamage(CheckInRange.CheckEnemyNotBossInRange(_radius), _damage);
    }

    
}
