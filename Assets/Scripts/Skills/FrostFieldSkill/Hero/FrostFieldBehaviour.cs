
using UnityEngine;

namespace DatHQ.Skill.Hero
{
    public class FrostFieldBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        public int Damage => _frostFieldSkillBehaviour.damage.RealPoint;
        // +1 do sai so cua collider trigger vi no lay tu center cua 2 object
        public float Radius => _frostFieldSkillBehaviour.radius.RealPoint + 1;

        [SerializeField]
        private GameObject _frostFieldHolder;

        [SerializeField]
        private Collider _frostFieldCollider;

        [SerializeField]
        private float _timerReturnPool = 2.5f;

        [SerializeField]
        private float _timerDamage = 0.1f;

        private int _percentScale = 20;

        private float _scale => ((_frostFieldSkillBehaviour.radius.RealPoint / _percentScale) + 1);

        private void Reset()
        {
            _frostFieldHolder = transform.parent.gameObject;
            _audioSource = GetComponent<AudioSource>();
            _frostFieldCollider = GetComponent<Collider>();
        }
        private FrostFieldSkillBehaviour _frostFieldSkillBehaviour;
        public void InitBehaviourSkill(FrostFieldSkillBehaviour frostFieldSkillBehaviour)
        {
            _frostFieldSkillBehaviour = frostFieldSkillBehaviour;
            _frostFieldHolder.transform.localScale = Vector3.one * _scale;
            Invoke(nameof(Destroy), _timerReturnPool);
            Invoke(nameof(DamageInRadius), _timerDamage);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
        private void DamageInRadius()
        {
            //Debug.Log("Damage Frost");
            _audioSource.Play();
            _frostFieldCollider.enabled = true;
            //for (int index = 0; index < EnemySpawner.Instance.EnemyBehaviours.Count; index++)
            //{
            //    if (Vector3.Distance(_frostFieldHolder.transform.position, EnemySpawner.Instance.EnemyBehaviours[index].transformEnemy.position) <= Radius)
            //    {
            //        EnemySpawner.Instance.EnemyBehaviours[index].TakeDamage(Damage);
            //    }
            //}
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<EnemyBehavior>().TakeDamage(Damage);
            }
        }
        private void Destroy()
        {
            _frostFieldCollider.enabled = false;
            ProjectilesSpawner.Instance.ReturnPoolProjectile(_frostFieldHolder, "FrostFieldHero");
        }
    }
}