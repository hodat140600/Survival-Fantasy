using System.Collections;
using Assets.Scripts.Skills.Attributes;
using Skills.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Skills.LiveSkill
{
    [CreateAssetMenu(menuName = "Skills/LiveSkillSettings", fileName = "LiveSkillSettings")]
    public class LiveSkillSettings : ScriptableObject, IBaseSettings, IPercentSettings
    {
        [Range(0.0f, 10000f)]
        public int addedPercent;

        [Range(0.0f, 50000f)]
        public int addedPoint;

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
        public string Description
        {
            get
            {
                string description = "";
                if (addedPercent != 0)
                {
                    description += "<color=white>Health <color=green>+" + addedPercent.ToString() + "%";
                }

                return description;
            }

        }


        private LiveSkillBehavior _skill;
        private GameObject _gameObject;

        public void LevelUp(GameObject gameObject)
        {
            _gameObject = gameObject;
            _skill = _gameObject.GetComponent<LiveSkillBehavior>();

            if (_skill == null)
            {
                _skill = gameObject.AddComponent<LiveSkillBehavior>();
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
            _skill.health.Percent += addedPercent;
            _skill.health.BasePoint += addedPoint;
        }


        private void ApplyBaseSettings()
        {
            SkillSystem skillSystem = _gameObject.GetComponent<SkillSystem>();
        }

        public void AddPercent(int addedPercent)
        {
            this.addedPercent += addedPercent;
        }

        public void SetPercent(int percent)
        {
            addedPercent = percent;
        }

        public int GetAddedPercent()
        {
            return addedPercent;
        }

        public void UpdateSkill(IBaseSettings skill)
        {
            var liveSkillSettings = skill as LiveSkillSettings;

            this.addedPoint += liveSkillSettings.addedPoint;
            this.addedPercent += liveSkillSettings.addedPercent;
        }

    }
}