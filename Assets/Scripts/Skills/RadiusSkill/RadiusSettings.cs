using System.Collections;
using Skills.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Skills
{

    [CreateAssetMenu(menuName = "Skills/IncreaseRadiusSettings", fileName = "IncreaseDamageSettings")]
    public class RadiusSettings : ScriptableObject, IBaseSettings, IPercentSettings
    {
        [Range(0.0f, 10000f)]
        public int AddedPercent;
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
                if (AddedPercent != 0)
                {
                    description += "<color=white>Radius <color=green>+" + AddedPercent.ToString() +"%";
                }

                return description;
            }

        }


        public void LevelUp(GameObject gameObject)
        {
            IRadiusSkillBehavior[] behaviors = gameObject.GetComponents<IRadiusSkillBehavior>();
            foreach (var behavior in behaviors)
            {
                behavior.IncreaseRadiusPercent(AddedPercent);
            }
        }

        public void AddPercent(int addedPercent)
        {
            this.AddedPercent += addedPercent;
        }

        public void SetPercent(int percent)
        {
            this.AddedPercent = percent;
        }

        public int GetAddedPercent()
        {
            return this.AddedPercent;
        }

        public void UpdateSkill(IBaseSettings skill)
        {
            var radiusSettings = skill as RadiusSettings;

            this.AddedPercent += radiusSettings.AddedPercent;
        }
    }
}