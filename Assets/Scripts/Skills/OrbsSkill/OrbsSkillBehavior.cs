using Assets.Scripts.Skills.Attributes;
using System.Collections;
using System.Collections.Generic;
using Skills.Interfaces;
using UnityEngine;
namespace Assets.Scripts.Skills
{
    public class OrbsSkillBehavior : SkillBehavior, IDamageSkillBehavior, IRadiusSkillBehavior,IProjectilesSkillBehavior
    {
        public Speed speed;

        void Awake()
        {
            damage = new Damage();
            numberProjectiles = new NumberProjectiles();
            speed = new Speed();
            radius = new Radius();
        }

        // TODO Chuyen code cua Circular Motion va inject OrbSkill GameObject vao day
        public void IncreaseDamagePercent(int percent)
        {
            damage.Percent += percent;

        }

        public void IncreaseRadiusPercent(int percent)
        {
            radius.Percent += percent;
        }

        public void IncreaseProjectilesPoint(int point)
        {
            numberProjectiles.AddPoint += point;
        }
    }

}

