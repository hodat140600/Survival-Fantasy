using UnityEngine;

namespace Assets.Scripts.Skills
{
    [CreateAssetMenu(menuName = "Skills/EarthquakeSkillSettings", fileName = "EarthquakeSkillSettings")]
    public class EarthquakeSkillSettings : DeathCircleSkillSettings
    {
        public override void LevelUp(GameObject gameObject)
        {
            base._gameObject = gameObject;
            base._skill = base._gameObject.GetComponent<EarthquakeSkillBehavior>();

            if (base._skill == null)
            {
                base._skill = new EarthquakeSkillBehavior();
                base._skill = gameObject.AddComponent<EarthquakeSkillBehavior>();
                base.ApplyBaseSettings();
            }

            base.UpdateSkill();
        }

    }
}