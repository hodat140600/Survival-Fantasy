using UnityEngine;

namespace DatHQ.Skill.Hero
{
    public class EarthquakeBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;
        public int Damage => _earthquakeSkillBehaviour.damage.RealPoint;
        // +1 do sai so cua collider trigger vi no lay tu center cua 2 object
        public float Radius => _earthquakeSkillBehaviour.radius.RealPoint + 1;

        [SerializeField]
        private GameObject _earthquakeHolder;

        [SerializeField]
        private Collider _earthquakeCollider;

        [SerializeField]
        private float _timerReturnPool = 2.5f;

        [SerializeField]
        private float _timerDamage = 0.1f;

        private int _percentScale = 20;

        private float _scale => ((_earthquakeSkillBehaviour.radius.RealPoint / _percentScale) + 1);

        private void Reset()
        {
            _earthquakeHolder = transform.parent.gameObject;
            _audioSource = GetComponent<AudioSource>();
            _earthquakeCollider = GetComponent<Collider>();
        }
        private EarthquakeSkillBehaviour _earthquakeSkillBehaviour;
        public void InitBehaviourSkill(EarthquakeSkillBehaviour earthquakeSkillBehaviour)
        {
            _earthquakeSkillBehaviour = earthquakeSkillBehaviour;
            _earthquakeHolder.transform.localScale = Vector3.one * _scale;
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
            _earthquakeCollider.enabled = true;
            //for (int index = 0; index < EnemySpawner.Instance.EnemyBehaviours.Count; index++)
            //{
            //    if (Vector3.Distance(_earthquakeHolder.transform.position, EnemySpawner.Instance.EnemyBehaviours[index].transformEnemy.position) <= Radius)
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
            _earthquakeCollider.enabled = false;
            ProjectilesSpawner.Instance.ReturnPoolProjectile(_earthquakeHolder, "EarthquakeHero");
        }
    }
}