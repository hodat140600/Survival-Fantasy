using Assets.Scripts.Skills.Attributes;
using UnityEngine;

namespace Assets.Scripts.Skills.TargetSkill
{
    [CreateAssetMenu(menuName = "Skills/TargetSkillSettings", fileName = "TargetSkillSettingsLevel")]
    [System.Serializable]
    public class TargetSkillSettings : ScriptableObject, IDamageSettings
    {
        
        [SerializeField]private Damage _damage;

        [SerializeField]private NumberProjectiles _numberProjectiles;
      
        
        [SerializeField]private Radius _radius;

        [SerializeField]private Cooldown _cooldown;

        [SerializeField]private float _fireRate;

        [SerializeField]private float _projectileSpeed;

        private TargetSkillBehavior _skill;
        private GameObject _gameObject;
        
        [SerializeField] 
        private int _level;
        public int Level
        {
            get { return _level;}
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
            _skill = _gameObject.GetComponent<TargetSkillBehavior>();

            if (_skill == null)
            {
                _skill = gameObject.AddComponent<TargetSkillBehavior>();
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
            
            /*_skill.speed.BasePoint += _speed.BasePoint;
            _skill.speed.Percent += _speed.Percent;*/
            _skill.cooldown.BasePoint += _cooldown.BasePoint;
            _skill.cooldown.Percent += _cooldown.Percent;

            _skill.projectileSpeed += _projectileSpeed;

            _skill.fireRate += _fireRate;
        }


        private void ApplyBaseSettings()
        {
            SkillSystem skillSystem = _gameObject.GetComponent<SkillSystem>();
            skillSystem.GetObjectSelectedSkil("TargetSkill").GetComponent<TargetController>().Init(_skill);
            skillSystem.ApplyDamageSetting(_skill);
            skillSystem.ApplyRadiusSetting(_skill);
            skillSystem.ApplyProjectilesSetting(_skill);
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
                if (_projectileSpeed != 0)
                {
                    description += "<color=white>Fly Speed <color=green>+" + _projectileSpeed.ToString() + "<br>";
                }
                if (_fireRate != 0)
                {
                    description += "<color=white>Attack Speed <color=green>+" + _fireRate.ToString() + "<br>";
                }
                return description;
            }
            
        }

        
    }
}