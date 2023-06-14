using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    // Base Settings la cac setting chi tang chi so Damage, Radius, Speed, Health
    public interface IBaseSettings : ISkillSettings
    {
        void UpdateSkill(IBaseSettings skill);
    }
}