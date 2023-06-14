using System.Collections;
using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Utils;
using Skills.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Skills.SlowSkill
{
    [CreateAssetMenu(menuName = "Skills/SlowSkillSettings", fileName = "SlowSkillSettings")]
    public class SlowSkillSettings : ScriptableObject, IDamageSettings
    {
        public Damage damage;

        public Radius radius;
       
        public Cooldown coolDown;

        [Space(30)]
        [Range(0.0f, 50f)]
        public float AddedTimeEffect;
        [Range(0.0f, 50f)]
        public int AddedSlowingSpeed;
        
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
        private SlowSkillBehavior _skill;
        private GameObject _gameObject;

        public void LevelUp(GameObject gameObject)
        {
            _gameObject = gameObject;
            _skill = _gameObject.GetComponent<SlowSkillBehavior>();

            if (_skill == null)
            {
                _skill = gameObject.AddComponent<SlowSkillBehavior>();
                ApplyBaseSettings();
                UpdateSkill();
            }
            else
            {
                UpdateSkill();
            }
        }
        private void UpdateSkill()
        {
            _skill.damage.BasePoint += damage.BasePoint;
            _skill.damage.Percent += damage.Percent;
            
            _skill.radius.BasePoint += radius.BasePoint;
            _skill.radius.Percent += radius.Percent;

            _skill.frostField.transform.localScale = (Vector3.one * _skill.radius.RealPoint); /*new Vector3(radius.RealPoint,radius.RealPoint, radius.RealPoint)*/;
            
            _skill.slowingSpeed += AddedSlowingSpeed;
            
            _skill.cooldown.BasePoint += coolDown.BasePoint;
            _skill.cooldown.Percent += coolDown.Percent;
            
            _skill.timeEffect += AddedTimeEffect;
        }


        private void ApplyBaseSettings()
        {
            SkillSystem skillSystem = _gameObject.GetComponent<SkillSystem>();
            skillSystem.GetObjectSelectedSkil("SlowSkill").GetComponent<SlowMovement>().Init(_skill);
            skillSystem.ApplyDamageSetting(_skill);
            skillSystem.ApplyRadiusSetting(_skill);
            skillSystem.ApplyCooldownSetting(_skill);
        }
        public string Description
        {
            get
            {
                string description = "";
                if (damage.BasePoint != 0)
                {
                    description+="<color=white>Damage <color=green>+"+damage.BasePoint.ToString()+"<br>";
                }
                if (radius.BasePoint != 0)
                {
                    description+="<color=white>Radius <color=green>+"+radius.BasePoint.ToString()+"<br>";
                }

                if (coolDown.BasePoint != 0)
                {
                    description += "<color=white>Cooldown <color=green>" + coolDown.BasePoint + "<br>";
                }


                return description;
            }
            
        }

        
    }
}