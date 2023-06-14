using Skills.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Skills.ProjectilesSkill
{
    [CreateAssetMenu(menuName = "Skills/IncreaseProjectilesSettings", fileName = "IncreaseProjectilesSettings")]
    public class ProjectilesSettings : ScriptableObject, IBaseSettings
    {
        [Range(0.0f, 100f)] public int addedPoint;
        [SerializeField] private int _level;
        public int Level { get => _level; }
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
                if (addedPoint != 0)
                {
                    description += "<color=white>Projectiles <color=green>+" + addedPoint.ToString();
                }
                return description;
            }

        }

        public void LevelUp(GameObject gameObject)
        {
            IProjectilesSkillBehavior[] behaviors = gameObject.GetComponents<IProjectilesSkillBehavior>();
            foreach (var behavior in behaviors)
            {
                behavior.IncreaseProjectilesPoint(addedPoint);
            }
        }

        public void UpdateSkill(IBaseSettings skill)
        {
            var projectilesSettings = skill as ProjectilesSettings;

            this.addedPoint += projectilesSettings.addedPoint;
        }

    }
}