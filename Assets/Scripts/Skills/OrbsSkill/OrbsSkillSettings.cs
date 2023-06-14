using Assets.Scripts.Skills;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Skills.ProjectilesSkill;
using Assets.Scripts.Utils;
using Skills.Interfaces;
using UnityEngine;
namespace Assets.Scripts.Skills
{
    /*// TODO: Tao abstract class SkillSettings
    public abstract class SkillSettings : ScriptableObject
    {
       
        
    }*/
    [CreateAssetMenu(menuName = "Skills/OrbsSkillSettings", fileName = "OrbsSkillSettings")]
    public class OrbsSkillSettings : ScriptableObject, IDamageSettings
    {
        
        public Damage damage;

        public NumberProjectiles numberProjectiles;

        public Radius radius;

        public Speed speed;

        /*public enum SKillSettingOperator
        {
            Overrried,
            Increment
        }*/

        private OrbsSkillBehavior _skill;
        private GameObject _gameObject;

        public void LevelUp(GameObject gameObject)
        {
            _gameObject = gameObject;
            _skill = _gameObject.GetComponent<OrbsSkillBehavior>();

            if (_skill == null)
            {
                _skill = gameObject.AddComponent<OrbsSkillBehavior>();
                UpdateSkill();
                ApplyBaseSettings();
            }
            else
            {
                UpdateSkill();
            }

        }

        [SerializeField]
        private int _level;
        public int Level { get=>_level;}
        [SerializeField] private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private void UpdateSkill()
        {
            //damage
            _skill.damage.BasePoint += damage.BasePoint;
            _skill.damage.Percent += damage.Percent;
            //radius
            _skill.radius.BasePoint += radius.BasePoint;
            _skill.radius.Percent += radius.Percent;
            //numberProjectiles
            _skill.numberProjectiles.BasePoint += numberProjectiles.BasePoint;
            _skill.numberProjectiles.AddPoint += numberProjectiles.AddPoint;
            //speed
            _skill.speed.BasePoint += speed.BasePoint;
            _skill.speed.Percent += speed.Percent;
        }


        private void ApplyBaseSettings()
        {
            SkillSystem skillSystem = _gameObject.GetComponent<SkillSystem>();
            skillSystem.GetObjectSelectedSkil("OrbSkill").GetComponent<CircularMotion>().Init(_skill);
            skillSystem.ApplyDamageSetting(_skill);
            skillSystem.ApplyRadiusSetting(_skill);
            skillSystem.ApplyProjectilesSetting(_skill);
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

                if (numberProjectiles.BasePoint != 0)
                {
                    description+="<color=white>Projectiles <color=green>+"+numberProjectiles.BasePoint.ToString()+"<br>";
                }

                if (speed.BasePoint != 0)
                {
                    description += "<color=white>Speed <color=green>+" + speed.BasePoint.ToString() + "<br>";
                }
                return description; 
            }
        }


        
    }
}