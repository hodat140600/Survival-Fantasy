using System.Collections;
using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Utils;
using Skills.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Skills.SmashSkill
{
    [CreateAssetMenu(menuName = "Skills/SmashSkillSettings", fileName = "SmashSkillSettings")]
    public class SmashSkillSettings : ScriptableObject, IDamageSettings
    {
         
        public Damage damage;

        public Radius radius;

        public Cooldown cooldown;
        
        
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

        private SmashSkillBehavior _skill;
        private GameObject _gameObject;

        public void LevelUp(GameObject gameObject)
        {
            _gameObject = gameObject;
            _skill = _gameObject.GetComponent<SmashSkillBehavior>();

            if (_skill == null)
            {
                _skill = gameObject.AddComponent<SmashSkillBehavior>();
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
            //damage
            _skill.damage.BasePoint += damage.BasePoint;
            _skill.damage.Percent += damage.Percent;
            //radius
            _skill.radius.BasePoint += radius.BasePoint;
            _skill.radius.Percent += radius.Percent;

            _skill.smashVFX.transform.localScale = Vector3.one * _skill.radius.RealPoint;
            //cooldown
            _skill.cooldown.BasePoint += cooldown.BasePoint;
            _skill.cooldown.Percent += cooldown.Percent;
        }


        private void ApplyBaseSettings()
        {
            SkillSystem skillSystem = _gameObject.GetComponent<SkillSystem>();
            skillSystem.GetObjectSelectedSkil("SmashingSkill").GetComponent<Smashing>().Init(_skill);
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
                if (cooldown.BasePoint != 0)
                {
                    description += "<color=white>CoolDown <color=green>" + cooldown.BasePoint.ToString() + "<br>";
                }

            

                return description;
            }
            
        }

        
        
    }
}