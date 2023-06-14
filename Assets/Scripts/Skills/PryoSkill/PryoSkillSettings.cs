using Assets.Scripts.Skills.Attributes;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    [CreateAssetMenu(menuName = "Skills/PryoSkillSettings", fileName = "PryoSkillSettings")]
    public class PryoSkillSettings : ScriptableObject, IDamageSettings
    {
        [SerializeField]
        private Damage _damage;

        [SerializeField]
        private NumberProjectiles _numberProjectiles;

        [SerializeField]
        private Radius _radius;

        [SerializeField]
        private Cooldown _cooldown;

        [SerializeField]
        private int _flySpeed;

        [SerializeField]
        private int _level;

        [SerializeField]
        private int _angle;
        public int Level
        {
            get { return _level; }
        }
        [SerializeField] private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private PryoSkillBehaviour _skill;
        private GameObject _gameObject;

        public void LevelUp(GameObject gameObject)
        {
            _gameObject = gameObject;
            _skill = _gameObject.GetComponent<PryoSkillBehaviour>();

            if (_skill == null)
            {
                _skill = gameObject.AddComponent<PryoSkillBehaviour>();
                UpdateSkill();
                ApplyBaseSettings();
            }
            else
            {
                UpdateSkill();
            }

        }



        private void UpdateSkill()
        {
            _skill.damage.BasePoint += _damage.BasePoint;
            _skill.damage.Percent += _damage.Percent;

            _skill.radius.BasePoint += _radius.BasePoint;
            _skill.radius.Percent += _radius.Percent;

            _skill.numberProjectiles.BasePoint += _numberProjectiles.BasePoint;
            _skill.numberProjectiles.AddPoint += _numberProjectiles.AddPoint;

            _skill.cooldown.BasePoint += _cooldown.BasePoint;
            _skill.cooldown.Percent += _cooldown.Percent;

            _skill.flySpeed += _flySpeed;

            _skill.angle += _angle;
        }


        private void ApplyBaseSettings()
        {
            SkillSystem skillSystem = _gameObject.GetComponent<SkillSystem>();
            skillSystem.GetObjectSelectedSkil("PryoSkill").GetComponent<PryoController>().Init(_skill);
            skillSystem.ApplyDamageSetting(_skill);
            skillSystem.ApplyRadiusSetting(_skill);
            skillSystem.ApplyProjectilesSetting(_skill);
            skillSystem.ApplyCooldownSetting(_skill);
        }
        public string Description
        {
            get
            {
                string description = "";
                if (_damage.BasePoint != 0)
                {
                    description += "<color=white>Damage <color=green>+" + _damage.BasePoint.ToString() + "<br>";
                }
                if (_radius.BasePoint != 0)
                {
                    description += "<color=white>Radius <color=green>+" + _radius.BasePoint.ToString() + "<br>";
                }

                if (_numberProjectiles.BasePoint != 0)
                {
                    description += "<color=white>Projectiles <color=green>+" + _numberProjectiles.BasePoint.ToString() + "<br>";
                }
                if (_cooldown.BasePoint != 0)
                {
                    description += "<color=white>CoolDown <color=green>" + _cooldown.BasePoint.ToString() + "<br>";
                }
                if (_flySpeed != 0)
                {
                    description += "<color=white>Fly Speed <color=green>+" + _flySpeed.ToString() + "<br>";
                }
                if (_angle != 0)
                {
                    description += "<color=white>Angle Bullet <color=green>" + _angle.ToString() + "<br>";
                }
                return description;
            }

        }
    }
}