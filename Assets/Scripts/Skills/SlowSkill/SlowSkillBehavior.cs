using Assets.Scripts.Skills.Attributes;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Skills.SlowSkill
{
    public class SlowSkillBehavior : SkillBehavior, IDamageSkillBehavior, IRadiusSkillBehavior,ICooldownSkillBehavior
    {
        public int slowingSpeed;
        public Cooldown cooldown;
        public float timeEffect;
        public GameObject frostField;
        void Awake()
        {
            damage = new Damage();
            slowingSpeed = 0;
            cooldown = new Cooldown();
            timeEffect = 0;
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
            frostField.transform.localScale = (Vector3.one * radius.RealPoint);
        }

        public void IncreaseCooldownPercent(int percent)
        {
            cooldown.Percent += percent;
        }
    }
}