using Assets.Scripts.Utils;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Skills.Attributes
{
    [System.Serializable]
    public class Speed : IAttribute
    {
        [Range(0.0f, 50f)]
        public float BasePoint;
        
        public int Percent;

        public Speed()
        {
            BasePoint = 0;
            Percent = 0;
        }
        public float RealPoint
        {
            get
            {
                return BasePoint + MathUtils.GetPercentOfValue(Percent, BasePoint);
            }
        }

    }
}