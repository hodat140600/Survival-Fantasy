using UnityEngine;

namespace Assets.Scripts.Skills.Heal
{
    [CreateAssetMenu(menuName = "Skills/HealSkillSettings", fileName = "HealSkillSettings")]
    public class HealSkillSettings : ScriptableObject,ISkillSettings
    {
        public int addPercent;
        public void LevelUp(GameObject gameObject)
        {
            IHealthSkillBehavior[] behaviors = gameObject.GetComponents<IHealthSkillBehavior>();
            foreach (var behavior in behaviors)
            {
                behavior.IncreaseHealth(addPercent);
            }
        }

        [SerializeField] private int _level;
        public int Level { get=>_level; }
        [SerializeField] private string _id;
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string Description
        {
            get=>"<color=white>Healing +<color=green>"+addPercent + "%";
        }
    }
}