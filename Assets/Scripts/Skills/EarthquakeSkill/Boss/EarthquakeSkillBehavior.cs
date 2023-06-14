using System.Collections;
using UnityEngine;
using Assets.Scripts.Skills.Attributes;

namespace Assets.Scripts.Skills
{
    public class EarthquakeSkillBehavior : DeathCircleSkillBehavior
    {
        private void Start()
        {
            base.Start();
        }

        private EarthquakeBehavior _earthquakeBehavior;
        protected override void SpawnSkill(Vector3 position)
        {
            if (_earthquakeBehavior == null)
            {
                _earthquakeBehavior = skillObjectPrefab.GetComponent<EarthquakeBehavior>();
            }

            _earthquakeBehavior.Init(damage, radius, isEnemySkill);
            var skillObject = Instantiate(skillObjectPrefab, position, skillObjectPrefab.transform.rotation);
            skillObject.transform.parent = transform.parent.parent;
        }
    }
}