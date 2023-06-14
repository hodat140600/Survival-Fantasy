using Assets.Scripts.Skills.SlowSkill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Move logic nay vao SlowSkill
public class SlowMovement : MonoBehaviour
{
    [SerializeField] ParticleSystem _slowEffect;
    SlowSkillBehavior _slowSkillBehaviorBehavior;
    // + 1 do sai so trigger
    public float Radius => _slowSkillBehaviorBehavior.radius.RealPoint + 1;
    public int Damage => _slowSkillBehaviorBehavior.damage.RealPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            StartCoroutine(OnDamagePerSecond(Damage, other.GetComponent<EnemyBehavior>()));
        }
    }
    WaitForSeconds forSeconds = new WaitForSeconds(1f);
    IEnumerator OnDamagePerSecond(int damage, EnemyBehavior enemyBehavior)
    {
        while(Vector3.Distance(enemyBehavior.transformEnemy.position, enemyBehavior.PlayerTransform.position) <= Radius)
        {
            if (!enemyBehavior.isActiveAndEnabled) break;
            enemyBehavior.TakeDamage(damage);
            yield return forSeconds;
        }
    }


    public void Init(SlowSkillBehavior slowSkillBehaviorBehavior)
    {
        _slowSkillBehaviorBehavior = slowSkillBehaviorBehavior;
        _slowSkillBehaviorBehavior.frostField = this.gameObject;
        this.gameObject.SetActive(true);
    }
}
