using Assets.Scripts.Skills.Attributes;
using Skills.Interfaces;
using System;

namespace Assets.Scripts.Skills.TargetSkill
{
    [Serializable]
    public class TargetSkillBehavior : SkillBehavior, IDamageSkillBehavior, IRadiusSkillBehavior,IProjectilesSkillBehavior, ICooldownSkillBehavior
    {
        /*public Speed speed;*/
        public Cooldown cooldown;
        public float fireRate;
        public float projectileSpeed;
        void Awake()
        {
            damage = new Damage();
            numberProjectiles = new NumberProjectiles();
            cooldown = new Cooldown();
            /*speed = new Speed();*/
            radius = new Radius();
            projectileSpeed = 0;
            fireRate = 0;
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

        public void IncreaseCooldownPercent(int percent)
        {
            cooldown.Percent += percent;
        }
    }
}