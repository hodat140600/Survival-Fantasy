using Assets.Scripts.Skills.Attributes;
using Skills.Interfaces;

namespace Assets.Scripts.Skills
{
    public class PryoSkillBehaviour : SkillBehavior, IDamageSkillBehavior, IRadiusSkillBehavior, IProjectilesSkillBehavior, ICooldownSkillBehavior
    {
        public Cooldown cooldown;
        public float flySpeed;
        public float angle;
        void Awake()
        {
            damage = new Damage();
            numberProjectiles = new NumberProjectiles();
            cooldown = new Cooldown();
            radius = new Radius();
            flySpeed = 0;
            angle = 0;
        }
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