using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GoldCoinBehavior : MonoBehaviour, IRewardItem
{
    [SerializeField] private int _goldAmount;

    public void Init(int goldAmount)
    {
        _goldAmount = goldAmount;
    }

    public void Active()
    {
        GameManager.Instance.skillSystem.AddGold(this._goldAmount);
    }
}
