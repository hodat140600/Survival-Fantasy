using UnityEngine;

namespace Assets.Scripts.Skills
{
    public class ThunderStrikeSkillBehavior : DeathCircleSkillBehavior
    {
        private ThunderStrikeBehavior _thunderStrikeBehavior;
        protected override void SpawnSkill(Vector3 position)
        {
            if (_thunderStrikeBehavior == null)
            {
                _thunderStrikeBehavior = skillObjectPrefab.GetComponent<ThunderStrikeBehavior>();
            }

            _thunderStrikeBehavior.Init(damage, radius, isEnemySkill);
            var skillObject = Instantiate(skillObjectPrefab, position, skillObjectPrefab.transform.rotation);
            skillObject.transform.parent = transform.parent.parent;
        }
    }
}