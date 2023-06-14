using System.Collections;
using UniRx;
using UnityEngine;

namespace DatHQ.Skill.Hero
{
    public class MeteorStrikeController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _meteorPrefab;
        [SerializeField]
        private AudioSource _meteorSound;
        [SerializeField]
        private MeteorStrikeSkillBehavior _meteorStrikeSkillBehavior;
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

        public void Init(MeteorStrikeSkillBehavior meteorStrikeSkillBehavior)
        {
            _meteorStrikeSkillBehavior = meteorStrikeSkillBehavior;
            this.gameObject.SetActive(true);
        }

        IEnumerator CastSkill()
        {
            while (true)
            {
                MessageBroker.Default.Publish(new UpdateCoolDownEvent(nameof(MeteorStrikeSkillSettings), _meteorStrikeSkillBehavior.cooldown.RealPoint));
                for (int projectile = 0; projectile < _meteorStrikeSkillBehavior.numberProjectiles.RealPoint; projectile++)
                {
                    this.Instantiate();
                    _meteorSound.Play();
                    yield return new WaitForSeconds(_fireRate);
                }
                yield return new WaitForSeconds(_meteorStrikeSkillBehavior.cooldown.RealPoint);
            }
        }
        void Instantiate()
        {
            MeteorStrikeBehavior meteorStrike = ProjectilesSpawner.
                Instance.
                SpawnProjectile(_meteorPrefab.name, 
                CheckInRange.GetClosestTargetVector(EnemySpawner.Instance.EnemiesEnable, 
                GameManager.Instance.playerTransform), 
                Quaternion.identity).
                GetComponentInChildren<MeteorStrikeBehavior>();
            meteorStrike?.InitBehaviourSkill(_meteorStrikeSkillBehavior);
        }
    }
}
