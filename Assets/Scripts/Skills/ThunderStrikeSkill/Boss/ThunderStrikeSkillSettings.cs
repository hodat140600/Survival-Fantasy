using UnityEngine;

namespace Assets.Scripts.Skills
{
    [CreateAssetMenu(menuName = "Skills/ThunderStrikeSkillSettings", fileName = "ThunderStrikeSkillSettings")]
    public class ThunderStrikeSkillSettings : DeathCircleSkillSettings
    {
        public override void LevelUp(GameObject gameObject)
        {
            base._gameObject = gameObject;
            base._skill = base._gameObject.GetComponent<ThunderStrikeSkillBehavior>();

            if (base._skill == null)
            {
                base._skill = gameObject.AddComponent<ThunderStrikeSkillBehavior>();
                base.ApplyBaseSettings();
            }

            base.UpdateSkill();
        }


    }
}