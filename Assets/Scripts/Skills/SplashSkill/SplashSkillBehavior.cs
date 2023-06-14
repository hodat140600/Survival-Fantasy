using Assets.Scripts.Skills.Attributes;
using Skills.Interfaces;

namespace Assets.Scripts.Skills.SplashSkill
{
    public class SplashSkillBehavior : SkillBehavior, IDamageSkillBehavior, IRadiusSkillBehavior, IProjectilesSkillBehavior,ICooldownSkillBehavior
    {
        public Cooldown cooldown;
        public int passThrough;
        public float flySpeed;
        void Awake()
        {
            damage = new Damage();
            numberProjectiles = new NumberProjectiles();
            cooldown = new Cooldown();
            radius = new Radius();
            passThrough = 0;
            flySpeed = 0;
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