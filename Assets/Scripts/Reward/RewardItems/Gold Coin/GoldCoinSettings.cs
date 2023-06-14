using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Reward/GoldCoinSettings", fileName = "GoldCoinSettings")]
public class GoldCoinSettings : ScriptableObject, IRewardSettings
{
    [Range(0.0f, 10000.0f)][SerializeField] private int _goldAmount;

    public void Active(GameObject gameObject)
    {
        gameObject.GetComponent<GoldCoinBehavior>().Init(_goldAmount);
    }

    [SerializeField] private string _name;
    public string GetName()
    {
        return _name;
    }

}
