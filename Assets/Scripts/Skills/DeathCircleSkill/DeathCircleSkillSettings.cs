using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    [CreateAssetMenu(menuName = "Skills/DeathCircleSkillSettings", fileName = "DeathCircleSkillSettings")]
    public class DeathCircleSkillSettings : ScriptableObject, ISkillSettings
    {
        [Range(0.0f, 10000.0f)] public int addedDamage;
        [Range(0.0f, 20.0f)] public int addedNumberProjectiles;
        [Range(0.0f, 10.0f)] public float addedRadius;
        [Range(0.0f, 20.0f)] public float AddedCoolDown;

        public GameObject gameObjectPrefab;
        protected GameObject _gameObject;


        protected DeathCircleSkillBehavior _skill;

        public virtual void LevelUp(GameObject gameObject)
        {
            _gameObject = gameObject;
            _skill = _gameObject.GetComponent<DeathCircleSkillBehavior>();

            if (_skill == null)
            {
                _skill = gameObject.AddComponent<DeathCircleSkillBehavior>();
                ApplyBaseSettings();
            }

            UpdateSkill();
        }

        public int Level { get; }
        public string Description
        {
            get;
        }
        public string Id { get; }
        protected virtual void UpdateSkill()
        {
            _skill.damage.BasePoint += addedDamage;
            _skill.numberProjectiles.BasePoint += addedNumberProjectiles;
            _skill.radius.BasePoint += addedRadius;
            _skill.Cooldown.BasePoint += AddedCoolDown;
            // _skill.skillObjectPrefab = gameObjectPrefab; //*actually don't need
        }

        protected virtual void ApplyBaseSettings()
        {
            _skill.skillObjectPrefab = gameObjectPrefab;
        }

    }
}