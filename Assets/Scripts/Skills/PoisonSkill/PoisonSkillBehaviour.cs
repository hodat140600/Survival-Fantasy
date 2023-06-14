using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using Skills.Interfaces;

namespace DatHQ.Skill.Hero
{
    public class PoisonSkillBehaviour : SkillBehavior, IDamageSkillBehavior, IProjectilesSkillBehavior, ICooldownSkillBehavior, IRadiusSkillBehavior
    {
        public Cooldown cooldown;
        void Awake()
        {
            damage = new Damage();
            numberProjectiles = new NumberProjectiles();
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