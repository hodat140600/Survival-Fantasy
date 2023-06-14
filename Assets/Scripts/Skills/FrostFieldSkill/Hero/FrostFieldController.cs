using System.Collections;
using UniRx;
using UnityEngine;

namespace DatHQ.Skill.Hero
{
    public class FrostFieldController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _frostFieldPrefab;
        [SerializeField]
        private FrostFieldSkillBehaviour _frostFieldSkillBehavior;
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

        public void Init(FrostFieldSkillBehaviour frostFieldSkillBehavior)
        {
            _frostFieldSkillBehavior = frostFieldSkillBehavior;
            this.gameObject.SetActive(true);
        }

        IEnumerator CastSkill()
        {
            //Debug.Log("Test Skill");
            while (true)
            {
                MessageBroker.Default.Publish(new UpdateCoolDownEvent(nameof(FrostFieldSkillSettings), _frostFieldSkillBehavior.cooldown.RealPoint));
                //Debug.Log("Message Broker");
                yield return new WaitForSeconds(_frostFieldSkillBehavior.cooldown.RealPoint);
                for (int projectile = 0; projectile < _frostFieldSkillBehavior.numberProjectiles.RealPoint; projectile++)
                {
                    //_target = CheckInRange.UpdateTarget(LevelManager.Instance.EnemyTransforms, this.transform);
                    //if (_target != null)
                    //{
                    //    Shoot();
                    //}
                    this.Instantiate();
                    yield return new WaitForSeconds(_fireRate);
                }
            }
        }
        void Instantiate()
        {
            //GameObject frostFieldPref = ProjectilesSpawner.Instance.SpawnProjectile(_frostFieldPrefab.name, CheckInRange.GetClosestTargetVector(EnemySpawner.Instance.EnemiesEnable, GameManager.Instance.playerTransform), _frostFieldPrefab.transform.rotation);
            FrostFieldBehaviour frostField = ProjectilesSpawner.
                Instance.
                SpawnProjectile(_frostFieldPrefab.name, 
                CheckInRange.
                GetClosestTargetVector(EnemySpawner.Instance.EnemiesEnable, 
                GameManager.Instance.playerTransform), 
                _frostFieldPrefab.transform.rotation).
                GetComponentInChildren<FrostFieldBehaviour>();

            frostField?.InitBehaviourSkill(_frostFieldSkillBehavior);
        }
    }
}