using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reward/MagnetSettings", fileName = "MagnetSettings")]
public class MagnetSettings : ScriptableObject, IRewardSettings
{
    [Range(0.0f, float.MaxValue / 2)][SerializeField] private float _radius;

    public void Active(GameObject gameObject)
    {
        gameObject.GetComponent<MagnetBehavior>().Init(_radius);
    }

    [SerializeField] private string _name;
    public string GetName()
    {
        return _name;
    }
}
