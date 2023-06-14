using Assets.Scripts.Skills.Attributes;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Skills.SmashSkill
{
    public class SmashSkillBehavior : SkillBehavior, IDamageSkillBehavior, IRadiusSkillBehavior,ICooldownSkillBehavior
    {
        public Cooldown cooldown;
        public GameObject smashVFX;
        void Awake()
        {
            damage = new Damage();
            cooldown = new Cooldown();
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
            smashVFX.transform.localScale = new Vector3(1, 1, 1) * radius.RealPoint;
        }

        public void IncreaseCooldownPercent(int percent)
        {
            cooldown.Percent += percent;
        }
    }
}