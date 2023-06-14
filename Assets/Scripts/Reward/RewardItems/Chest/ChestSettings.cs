using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Reward/ChestSettings", fileName = "ChestSettings")]
public class ChestSettings : ScriptableObject, IRewardSettings
{
    public void Active(GameObject gameObject) { }

    [SerializeField] private string _name;
    public string GetName()
    {
        return _name;
    }
}
