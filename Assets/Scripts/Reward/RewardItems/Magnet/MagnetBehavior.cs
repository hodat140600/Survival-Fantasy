using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBehavior : MonoBehaviour, IRewardItem
{
    [SerializeField] private float _radius;

    public void Init(float radius)
    {
        _radius = radius;
    }

    public void Active()
    {
        ExpGemManager.Instance.GetAllGems();
    }
}
