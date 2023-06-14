using System;
using UnityEngine;

namespace Assets.Scripts.Skills.Attributes
{
    [Serializable]
    public class NumberProjectiles:IAttribute
    {
        [Range(0,10f)]
        public int BasePoint;
        public int AddPoint;

        public int RealPoint
        {
            get
            {
                return BasePoint + AddPoint;
            }
            
        }
    }
}