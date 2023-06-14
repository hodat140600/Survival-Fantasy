using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utils;
using Assets.Scripts.Skills.MoveSkill;
using Skills.Interfaces;

namespace Assets.Scripts.Skills
{
    [CreateAssetMenu(menuName = "Skills/MoveSkillSettings", fileName = "MoveSkillSettings")]
    public class MoveSkillSettings : ScriptableObject, IBaseSettings, IPercentSettings
    {
        [Range(0.0f, 10000f)]
        public int addedPercent;

        [Range(0.0f, 10000f)]
        public int addedPoint;

        [SerializeField]
        private int _level;
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        [SerializeField] private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private MoveSkillBehavior _skill;
        private GameObject _gameObject;

        public void LevelUp(GameObject gameObject)
        {
            _gameObject = gameObject;
            _skill = _gameObject.GetComponent<MoveSkillBehavior>();

            if (_skill == null)
            {
                _skill = gameObject.AddComponent<MoveSkillBehavior>();
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
            _skill.speed.Percent += addedPercent;
            _skill.speed.BasePoint += addedPoint;
        }


        private void ApplyBaseSettings()
        {
            SkillSystem skillSystem = _gameObject.GetComponent<SkillSystem>();
        }
        public string Description
        {
            get
            {
                string description = "";
                if (addedPercent != 0)
                {
                    description += "<color=white>Move <color=green>+" + addedPercent.ToString() +"%";
                }
                return description;
            }
        }

        

        public void AddPercent(int addedPercent)
        {
            this.addedPercent += addedPercent;
        }

        public void SetPercent(int percent)
        {
            this.addedPercent = percent;
        }

        public int GetAddedPercent()
        {
            return addedPercent;
        }

        public void UpdateSkill(IBaseSettings skill)
        {
            var moveSkillBehavior = skill as MoveSkillSettings;

            this.addedPoint += moveSkillBehavior.addedPoint;
            this.addedPercent += moveSkillBehavior.addedPercent;
        }
    }
}
