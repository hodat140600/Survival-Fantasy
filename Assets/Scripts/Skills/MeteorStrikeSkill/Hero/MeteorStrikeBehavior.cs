using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DatHQ.Skill.Hero
{
    public class MeteorStrikeBehavior : MonoBehaviour
    {
        public int Damage => _meteorStrikeSkillBehavior.damage.RealPoint;
        // +1 do sai so cua collider trigger vi no lay tu center cua 2 object
        public float Radius => _meteorStrikeSkillBehavior.radius.RealPoint + 1;

        [SerializeField]
        private GameObject _meteorHolder;

        [SerializeField]
        private AudioSource _meteorSource;

        [SerializeField]
        private float _timerReturnPool = 6f;

        [SerializeField]
        private float _timerDamage = 1.5f;

        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private SphereCollider _collider;

        private int _percentScale = 20;
        private float _scale => ((_meteorStrikeSkillBehavior.radius.RealPoint / _percentScale) + 1);

        private float _radiusCollider => _meteorStrikeSkillBehavior.radius.RealPoint / 2;

        private void Reset()
        {
            _transform = transform;
            _meteorHolder = transform.parent.gameObject;
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
        private MeteorStrikeSkillBehavior _meteorStrikeSkillBehavior;
        public void InitBehaviourSkill(MeteorStrikeSkillBehavior meteorStrikeSkillBehavior)
        {
            _meteorStrikeSkillBehavior = meteorStrikeSkillBehavior;
            _transform.localScale = Vector3.one * _scale;
            //_collider.radius = _radiusCollider;
            _collider.enabled = false;
            Invoke(nameof(Destroy), _timerReturnPool);
            Invoke(nameof(DamageInRadius), _timerDamage);
        }
        private void DamageInRadius()
        {
            _meteorSource.Play();
            _collider.enabled = true;
            for (int index = 0; index < EnemySpawner.Instance.EnemyBehaviours.Count; index++)
            {
                if (Vector3.Distance(_meteorHolder.transform.position, EnemySpawner.Instance.EnemyBehaviours[index].transformEnemy.position) <= Radius)
                {
                    //Debug.Log("TakeDamageMeteor");
                    EnemySpawner.Instance.EnemyBehaviours[index].TakeDamage(Damage);
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                StartCoroutine(OnDamagePerSecond(Damage, other.GetComponent<EnemyBehavior>()));
            }
        }
        WaitForSeconds forSeconds = new WaitForSeconds(1f);
        float updateSpeedSeconds = 1f;
        private IEnumerator OnDamagePerSecond(int damage, EnemyBehavior enemyBehavior)
        {
            while (Vector3.Distance(enemyBehavior.transformEnemy.position, _transform.position) <= Radius)
            {
                if (!enemyBehavior.gameObject.activeInHierarchy) break;
                enemyBehavior.TakeDamage(damage);
                yield return forSeconds;
            }
        }
        private void Destroy()
        {
            ProjectilesSpawner.Instance.ReturnPoolProjectile(_meteorHolder, "MeteorHero");
        }
    }
}