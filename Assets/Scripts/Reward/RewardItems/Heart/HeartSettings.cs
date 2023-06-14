using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills.Attributes;
using UnityEngine;


[CreateAssetMenu(menuName = "Reward/HeartSettings", fileName = "HeartSettings")]
public class HeartSettings : ScriptableObject, IRewardSettings
{
    [SerializeField] private Health _health;

    public void Active(GameObject gameObject)
    {
        gameObject.GetComponent<HeartBehavior>().Init(_health);
    }

    [SerializeField] private string _name;
    public string GetName()
    {
        return _name;
    }

}
