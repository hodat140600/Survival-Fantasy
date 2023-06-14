using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine;
using Assets.Scripts.Skills.Attributes;

namespace Assets.Scripts.Skills
{
    public abstract class SkillBehavior : MonoBehaviour
    {

        [Space(10)]
        [Header("Skill Details")]
        public Damage damage;
        public NumberProjectiles numberProjectiles;
        public Radius radius;
        /*public float coolDown;*/

        public GameObject skillObjectPrefab;
    }
}