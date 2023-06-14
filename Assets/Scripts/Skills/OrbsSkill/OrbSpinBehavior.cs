using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Skills.OrbsSkill
{
    public class OrbSpinBehavior : MonoBehaviour
    {
        float ROTATION = 360f;

        OrbsSkillBehavior _orbsSkill;

        //private void Start()
        //{
        //    _orbsSkill = gameObject.GetComponentInParent<CircularMotion>()
        //        .player.GetComponent<OrbsSkillBehavior>();
        //}
        private void OnEnable()
        {
            _orbsSkill = gameObject.GetComponentInParent<CircularMotion>()
                .player.GetComponent<OrbsSkillBehavior>();
        }
        private void Update()
        {
            transform.Rotate(0, ROTATION * Time.deltaTime, 0);
        }
        private void OnTriggerEnter(Collider other)
        {
            var enemyHealth = other.GetComponent<EnemyBehavior>();

            if (enemyHealth != null && _orbsSkill != null)
            {
                enemyHealth.TakeDamage(_orbsSkill.damage.RealPoint);
            }
        }
    }
}