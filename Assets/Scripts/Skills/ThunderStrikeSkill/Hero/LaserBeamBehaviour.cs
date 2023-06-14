
using UnityEngine;

namespace DatHQ.Skill.Hero
{
    public class LaserBeamBehaviour : MonoBehaviour
    {
        public int Damage => _laserBeamSkillBehaviour.damage.RealPoint;
        // +1 do sai so cua collider trigger vi no lay tu center cua 2 object
        public float Radius => _laserBeamSkillBehaviour.radius.RealPoint + 1;

        [SerializeField]
        private GameObject _laserHolder;        
        [SerializeField]
        private Transform _laserHolderTransform;

        [SerializeField]
        private float _timerReturnPool = 6f;
        
        [SerializeField]
        private float _timerDamage = 1.6f;

        [SerializeField]
        private AudioSource _laserSource;

        private int _percentScale = 20;
        private float _scale => ((_laserBeamSkillBehaviour.radius.RealPoint / _percentScale) + 1);


        private void Reset()
        {
            _laserHolder = transform.parent.gameObject;
            _laserHolderTransform = _laserHolder.transform;
        }

        private LaserBeamSkillBehaviour _laserBeamSkillBehaviour;
        public void InitBehaviourSkill(LaserBeamSkillBehaviour laserBeamSkillBehaviour)
        {
            _laserBeamSkillBehaviour = laserBeamSkillBehaviour;
            _laserHolderTransform.localScale = Vector3.one * _scale;
            Invoke(nameof(Destroy), _timerReturnPool);
            Invoke(nameof(DamageInRadius), _timerDamage);
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
        private void DamageInRadius()
        {
            _laserSource.Play();
            for(int index = 0; index < EnemySpawner.Instance.EnemyBehaviours.Count; index++)
            {
                if(Vector3.Distance(_laserHolder.transform.position, EnemySpawner.Instance.EnemyBehaviours[index].transform.position) <= Radius)
                {
                    //Debug.Log("TakeDamageLasser");
                    EnemySpawner.Instance.EnemyBehaviours[index].TakeDamage(Damage);
                }
            }
        }

        private void Destroy()
        {
            if(_laserHolder.activeInHierarchy)
                ProjectilesSpawner.Instance.ReturnPoolProjectile(_laserHolder, "LaserHero");
        }
    }
}