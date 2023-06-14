using System.Collections;
using UnityEngine;
using Assets.Scripts.Skills.Attributes;

namespace Assets.Scripts.Skills
{
    public class MeteorStrikeSkillBehavior : DeathCircleSkillBehavior
    {
        private MeteorStrikeBehavior _meteorStrikeBehavior;
        protected override void SpawnSkill(Vector3 position)
        {
            if (_meteorStrikeBehavior == null)
            {
                _meteorStrikeBehavior = skillObjectPrefab.GetComponent<MeteorStrikeBehavior>();
            }

            _meteorStrikeBehavior.Init(damage, radius, isEnemySkill);
            var skillObject = Instantiate(skillObjectPrefab, position, skillObjectPrefab.transform.rotation);
            skillObject.transform.parent = transform.parent.parent;
        }
    }
}