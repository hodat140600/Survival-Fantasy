using System.Collections;
using UniRx;
using UnityEngine;

namespace DatHQ.Skill.Hero
{
    public class LaserBeamController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _laserPrefab;
        [SerializeField]
        private LaserBeamSkillBehaviour _laserBeamSkillBehavior;
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

        public void Init(LaserBeamSkillBehaviour laserBeamSkillBehavior)
        {
            _laserBeamSkillBehavior = laserBeamSkillBehavior;
            this.gameObject.SetActive(true);
        }

        IEnumerator CastSkill()
        {
            //Debug.Log("Test Skill");
            while (true)
            {
                MessageBroker.Default.Publish(new UpdateCoolDownEvent(nameof(LaserBeamSkillSettings), _laserBeamSkillBehavior.cooldown.RealPoint));
                //Debug.Log("Message Broker");
                yield return new WaitForSeconds(_laserBeamSkillBehavior.cooldown.RealPoint);
                for (int projectile = 0; projectile < _laserBeamSkillBehavior.numberProjectiles.RealPoint; projectile++)
                {
                    this.Instantiate();
                    yield return new WaitForSeconds(_fireRate);
                }
            }
        }
        void Instantiate()
        {
            //GameObject laserPref = ProjectilesSpawner.Instance.SpawnProjectile(_laserPrefab.name, CheckInRange.GetClosestTargetVector(EnemySpawner.Instance.EnemiesEnable, GameManager.Instance.playerTransform), _laserPrefab.transform.rotation);
            LaserBeamBehaviour laserBeam = ProjectilesSpawner.
                Instance.
                SpawnProjectile(_laserPrefab.name, 
                CheckInRange.GetClosestTargetVector(EnemySpawner.Instance.EnemiesEnable, 
                GameManager.Instance.playerTransform), 
                _laserPrefab.transform.rotation).
                GetComponentInChildren<LaserBeamBehaviour>();

            laserBeam?.InitBehaviourSkill(_laserBeamSkillBehavior);
        }
    }
}