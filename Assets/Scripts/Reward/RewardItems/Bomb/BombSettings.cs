using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reward/BombSettings", fileName = "BombSettings")]
public class BombSettings : ScriptableObject, IRewardSettings
{
    [Range(0.0f, int.MaxValue / 2)][SerializeField] private int _damage;
    [Range(0.0f, float.MaxValue / 2)][SerializeField] private float _radius;

    public void Active(GameObject gameObject)
    {
        gameObject.GetComponent<BombBehavior>().Init(_damage, _radius);
    }

    [SerializeField] private string _name;
    public string GetName()
    {
        return _name;
    }

}
