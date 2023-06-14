using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using UnityEngine;

namespace DatHQ.Skill.Hero
{
    [CreateAssetMenu(menuName = "Skills/Hero/LaserBeamSkillSettings", fileName = "LaserBeamSkillSettings")]
    public class LaserBeamSkillSettings : ScriptableObject, IDamageSettings
    {
        [SerializeField] private Damage _damage;

        [SerializeField] private NumberProjectiles _numberProjectiles;


        [SerializeField] private Radius _radius;

        [SerializeField] private Cooldown _cooldown;

        private LaserBeamSkillBehaviour _skill;
        private GameObject _gameObject;

        [SerializeField]
        private int _level;
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
        public void LevelUp(GameObject gameObject)
        {
            _gameObject = gameObject;
            _skill = _gameObject.GetComponent<LaserBeamSkillBehaviour>();

            if (_skill == null)
            {
                _skill = gameObject.AddComponent<LaserBeamSkillBehaviour>();
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
            _skill.radius.BasePoint += _radius.BasePoint;
            _skill.numberProjectiles.BasePoint += _numberProjectiles.BasePoint;
            _skill.cooldown.BasePoint += _cooldown.BasePoint;
        }


        private void ApplyBaseSettings()
        {
            SkillSystem skillSystem = _gameObject.GetComponent<SkillSystem>();
            skillSystem.GetObjectSelectedSkil("LaserBeam").GetComponent<LaserBeamController>().Init(_skill);
            skillSystem.ApplyDamageSetting(_skill);
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
                    description+="<color=white>Damage <color=green>+"+_damage.BasePoint.ToString()+"<br>";
                }
                if (_radius.BasePoint != 0)
                {
                    description+="<color=white>Radius <color=green>+"+_radius.BasePoint.ToString()+"<br>";
                }

                if (_numberProjectiles.BasePoint != 0)
                {
                    description+="<color=white>Projectiles <color=green>+"+_numberProjectiles.BasePoint.ToString()+"<br>";
                }
                if (_cooldown.BasePoint != 0)
                {
                    description += "<color=white>CoolDown <color=green>" + _cooldown.BasePoint.ToString() + "<br>";
                }
                return description;
            }

        }
    }
}