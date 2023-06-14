using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cooldown : IAttribute
{
    [Range(-50f, 50f)]
    public float BasePoint;

    public int Percent;

    public Cooldown()
    {
        BasePoint = 0;
        Percent = 0;
    }
    public float RealPoint
    {
        get
        {
            return BasePoint - MathUtils.GetPercentOfValue(Percent, BasePoint);
        }
    }
}
