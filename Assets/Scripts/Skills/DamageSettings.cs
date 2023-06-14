using System.Collections;
using Skills.Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Skills
{

    [CreateAssetMenu(menuName = "Skills/DamageSettings", fileName = "DamageSettings")]
    public class DamageSettings : ScriptableObject, IBaseSettings, IPercentSettings
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


        public void LevelUp(GameObject gameObject)
        {
            IDamageSkillBehavior[] behaviors = gameObject.GetComponents<IDamageSkillBehavior>();
            foreach (var behavior in behaviors)
            {
                behavior.IncreaseDamagePercent(AddedPercent);
            }
        }

        public void UpdateSkill(IBaseSettings skill)
        {
            var damageSettings = skill as DamageSettings;

            this.AddedPercent += damageSettings.AddedPercent;
        }

        public void AddPercent(int addedPercent)
        {
            AddedPercent += addedPercent;
        }

        public void SetPercent(int percent)
        {
            AddedPercent = percent;
        }

        public int GetAddedPercent()
        {
            return AddedPercent;
        }
        public string Description
        {
            get
            {
                string description = "";
                if (AddedPercent != 0)
                {
                    description += "Damage <color=green>+" + AddedPercent.ToString() + "%";
                }

                return description;
            }

        }
    }
}