using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DatHQ.Skill.Hero
{
    public class PoisonBehaviour : MonoBehaviour
    {
        public int Damage => _poisonSkillBehaviour.damage.RealPoint;
        // +1 do sai so cua collider trigger vi no lay tu center cua 2 object
        public float Radius => _poisonSkillBehaviour.radius.RealPoint + 1;

        [SerializeField]
        private GameObject _poisonHolder;        
        [SerializeField]
        private GameObject _poisonAreaHolder;

        [SerializeField]
        private AudioSource _poisonSource;

        [SerializeField]
        private float _timerReturnPool = 5f;

        [SerializeField]
        private float _timerDamage = 1.5f;

        [SerializeField]
        private Transform _transform;

        [SerializeField]
        private SphereCollider _collider;

        //[SerializeField]
        //private Rigidbody _rigidbody;

        [SerializeField]
        private float _maxRadiusThrowing = 20f;

        [SerializeField]
        private float _minRadiusThrowing = 5f;

        [SerializeField]
        private float _throwForce = 5f;

        [SerializeField]
        private int _numJump = 1;

        [SerializeField]
        private Transform _playerTransform;

        private int _percentScale = 20;

        private float _scale => ((_poisonSkillBehaviour.radius.RealPoint / _percentScale) + 1);

        private float _radiusCollider => _poisonSkillBehaviour.radius.RealPoint / 2;

        //private Parabola parabola;

        private void Reset()
        {
            _poisonHolder = transform.parent.gameObject;
            _transform = _poisonHolder.transform;
        }

        private PoisonSkillBehaviour _poisonSkillBehaviour;
        public void InitBehaviourSkill(PoisonSkillBehaviour poisonSkillBehaviour)
        {
            _poisonSkillBehaviour = poisonSkillBehaviour;
            //_collider.radius = _radiusCollider;
            _playerTransform = GameManager.Instance.playerTransform;
            _transform.position = _playerTransform.position;
            _collider.enabled = false;
            _poisonAreaHolder.transform.localScale = Vector3.one * _scale;
            Invoke(nameof(Destroy), _timerReturnPool);
            Invoke(nameof(DamageInRadius), _timerDamage);
            //Debug.Log("Position: " + _transform.position);
            _transform.DOJump(CheckInRange.GetPosOnRing(_transform.position, _minRadiusThrowing, _maxRadiusThrowing) + (Vector3.up / 2), _throwForce, _numJump, _timerDamage);
        }
        //private void Start()
        //{
        //    parabola = new Parabola(_throwForce);
        //}
        private void OnDisable()
        {
            CancelInvoke();
            _poisonAreaHolder.SetActive(false);
        }
        private void DamageInRadius()
        {
            _poisonSource.Play();
            _poisonAreaHolder.SetActive(true);
            _collider.enabled = true;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                //Debug.Log("Trigger Poison");
                //Debug.Log("Radius = " + Radius);
                
                StartCoroutine(OnDamagePerSecond(Damage, other.GetComponent<EnemyBehavior>()));
            }
        }
        WaitForSeconds forSeconds = new WaitForSeconds(1f);
        float updateSpeedSeconds = 1f;
        private IEnumerator OnDamagePerSecond(int damage, EnemyBehavior enemyBehavior)
        {
            //Debug.Log("Distance = " + Vector3.Distance(enemyBehavior.transformEnemy.position, _transform.position));
            //Debug.Log("Condition: " + (Vector3.Distance(enemyBehavior.transformEnemy.position, _transform.position) <= Radius));
            while (Vector3.Distance(enemyBehavior.transformEnemy.position, _transform.position) <= Radius)
            {
                if (!enemyBehavior.isActiveAndEnabled) break;
                //Debug.Log("Poison Damage");
                enemyBehavior.TakeDamage(damage);
                yield return forSeconds;
            }
        }
        //private IEnumerator OnParabolaMove(Transform target, Vector3 start, Vector3 end, float duration)
        //{
        //    float time = 0;
        //    while((time - duration))
        //}
        private void Destroy()
        {
            _poisonAreaHolder.SetActive(false);
            //_rigidbody.velocity = Vector3.zero;
            ProjectilesSpawner.Instance.ReturnPoolProjectile(_poisonHolder, "PoisonEnergy");
        }
    }
}
public class Parabola
{
    float heigh;

    public Parabola(float heigh)
    {
        this.heigh = heigh;
    }

    public void Move(Transform target, Vector3 a, Vector3 b, float time)
    {
        float target_X = a.x + (b.x - a.x) * time;
        float maxHeigh = (a.y + b.y) / 2 + heigh;
        float target_Y = a.y + ((b.y - a.y)) * time + heigh * (1 - (Mathf.Abs(0.5f - time) / 0.5f) * (Mathf.Abs(0.5f - time) / 0.5f));
        target.position = new Vector3(target_X, target_Y);
    }
}