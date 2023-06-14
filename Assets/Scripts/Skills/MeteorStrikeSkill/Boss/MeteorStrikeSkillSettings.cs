using UnityEngine;

namespace Assets.Scripts.Skills
{
    [CreateAssetMenu(menuName = "Skills/MeteorStrikeSkillSettings", fileName = "MeteorStrikeSkillSettings")]
    public class MeteorStrikeSkillSettings : DeathCircleSkillSettings
    {
        public override void LevelUp(GameObject gameObject)
        {
            base._gameObject = gameObject;
            base._skill = base._gameObject.GetComponent<MeteorStrikeSkillBehavior>();

            if (base._skill == null)
            {
                base._skill = gameObject.AddComponent<MeteorStrikeSkillBehavior>();
                base.ApplyBaseSettings();
            }

            base.UpdateSkill();
        }

    }
}