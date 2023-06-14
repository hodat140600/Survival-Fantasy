using System.Collections;
using UniRx;
using UnityEngine;

namespace DatHQ.Skill.Hero
{
    public class EarthquakeController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _earthquakePrefab;
        [SerializeField]
        private EarthquakeSkillBehaviour _earthquakeSkillBehavior;
        [SerializeField]
        private float _fireRate = 0.5f;

        float _minDistance = 0f;
        [SerializeField]
        float _maxDistance = 20f;

        private void OnEnable()
        {
            StopAllCoroutines();
            StartCoroutine(CastSkill());
        }

        public void Init(EarthquakeSkillBehaviour earthquakeSkillBehavior)
        {
            _earthquakeSkillBehavior = earthquakeSkillBehavior;
            this.gameObject.SetActive(true);
        }

        IEnumerator CastSkill()
        {
            while (true)
            {
                MessageBroker.Default.Publish(new UpdateCoolDownEvent(nameof(EarthquakeSkillSettings), _earthquakeSkillBehavior.cooldown.RealPoint));
                //Debug.Log("Message Broker");
                yield return new WaitForSeconds(_earthquakeSkillBehavior.cooldown.RealPoint);
                for (int projectile = 0; projectile < _earthquakeSkillBehavior.numberProjectiles.RealPoint; projectile++)
                {
                    this.Instantiate();
                    yield return new WaitForSeconds(_fireRate);
                }
            }
        }
        void Instantiate()
        {
            //GameObject earthquakePref = ProjectilesSpawner.Instance.SpawnProjectile(_earthquakePrefab.name, CheckInRange.GetClosestTargetVector(EnemySpawner.Instance.EnemiesEnable, GameManager.Instance.playerTransform), _earthquakePrefab.transform.rotation);
            EarthquakeBehaviour earthquake = ProjectilesSpawner.
                Instance.
                SpawnProjectile(_earthquakePrefab.name, 
                CheckInRange.GetClosestTargetVector(EnemySpawner.Instance.EnemiesEnable, 
                GameManager.Instance.playerTransform), 
                _earthquakePrefab.transform.rotation).
                GetComponentInChildren<EarthquakeBehaviour>();

            earthquake?.InitBehaviourSkill(_earthquakeSkillBehavior);
        }
    }
}