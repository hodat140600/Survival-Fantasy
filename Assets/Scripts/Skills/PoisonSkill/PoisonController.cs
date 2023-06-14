using System.Collections;
using UniRx;
using UnityEngine;

namespace DatHQ.Skill.Hero
{
    public class PoisonController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _poisonPrefab;
        [SerializeField]
        private AudioSource _poisonSound;
        [SerializeField]
        private PoisonSkillBehaviour _poisonSkillBehaviour;
        [SerializeField]
        private float FireRate => _poisonSkillBehaviour.cooldown.RealPoint/ _poisonSkillBehaviour.numberProjectiles.RealPoint;

        float _minDistance = 0f;
        [SerializeField]
        float _maxDistance = 20f;

        private void OnEnable()
        {
            StopAllCoroutines();
            StartCoroutine(CastSkill());
        }

        public void Init(PoisonSkillBehaviour poisonSkillBehaviour)
        {
            _poisonSkillBehaviour = poisonSkillBehaviour;
            this.gameObject.SetActive(true);
        }

        IEnumerator CastSkill()
        {
            while (true)
            {
                MessageBroker.Default.Publish(new UpdateCoolDownEvent(nameof(PoisonSkillSettings), _poisonSkillBehaviour.cooldown.RealPoint));
                yield return new WaitForSeconds(_poisonSkillBehaviour.cooldown.RealPoint);
                for (int projectile = 0; projectile < _poisonSkillBehaviour.numberProjectiles.RealPoint; projectile++)
                {
                    this.Instantiate();
                    _poisonSound.Play();
                    //yield return new WaitForSeconds(FireRate);
                }
            }
        }
        void Instantiate()
        {
            GameObject poisonPref = ProjectilesSpawner.Instance.SpawnProjectile(_poisonPrefab.name, GameManager.Instance.playerTransform.position, Quaternion.identity).transform.GetChild(0).gameObject;
            //PoisonBehaviour poison = ProjectilesSpawner.
            //    Instance.
            //    SpawnProjectile(_poisonPrefab.name, 
            //    GameManager.Instance.playerTransform.position, 
            //    Quaternion.identity).
            //    GetComponentInChildren<PoisonBehaviour>();
            PoisonBehaviour poison = poisonPref.GetComponentInChildren<PoisonBehaviour>();
            poison?.InitBehaviourSkill(_poisonSkillBehaviour);
        }
    }
}