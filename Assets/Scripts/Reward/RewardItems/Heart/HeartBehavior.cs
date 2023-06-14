using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using UnityEngine;

public class HeartBehavior : MonoBehaviour, IRewardItem
{
    [SerializeField][Tooltip("Percent of heal")] private Health _health;

    public void Init(Health health)
    {
        _health = health;
    }

    public void Active()
    {
        GameManager.Instance.skillSystem.ApplyAttribute(_health);
    }
}
