using Assets.Scripts.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Skills.Attributes
{
    [System.Serializable]
    public class Damage : IAttribute
    {
        [Range(0.0f, 1000f)]
        public int BasePoint;
        public int Percent;
        
        public int RealPoint
        {
            get
            {
                return BasePoint + MathUtils.GetPercentOfValue(Percent, BasePoint);
            }
        }

    }
}