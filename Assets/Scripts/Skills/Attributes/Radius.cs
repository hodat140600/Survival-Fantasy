using Assets.Scripts.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Skills.Attributes
{
    [System.Serializable]
    public class Radius : IAttribute
    {
        [Range(0.0f, 10000f)]
        public float BasePoint;
        [Range(0.0f, 10000f)]
        public int Percent;

        [SerializeField]
        public float RealPoint
        {
            get
            {
                return BasePoint + MathUtils.GetPercentOfValue(Percent, BasePoint);
            }
        }

    }
}