using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    // TODO IRadiusSkillBehavior
    public interface IRadiusSkillBehavior : ISkillBehavior
    {
        public void IncreaseRadiusPercent(int percent);
    }
}