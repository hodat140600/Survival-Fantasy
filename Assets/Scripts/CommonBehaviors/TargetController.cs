using Assets.Scripts.Skills.TargetSkill;
using System.Collections;
using System.Linq;
using UnityEngine;
using UniRx;
public class TargetController : MonoBehaviour
{
    [SerializeField]
    private TargetSkillBehavior _targetSkillBehavior;
    [SerializeField]
    private AudioSource _audioSource;
    Transform _playerTransform;
    public GameObject bulletPrefab;

    public void Init(TargetSkillBehavior targetSkillBehavior)
    {
        _targetSkillBehavior = targetSkillBehavior;
        _playerTransform = GameManager.Instance.playerTransform;
        this.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ShootProjectile());
    }

    WaitUntil WaitUntil = new WaitUntil(() => EnemySpawner.Instance.EnemyBehaviours.Count > 0);
    int index;
    int stackDamage;
    IEnumerator ShootProjectile()
    {
        while (isActiveAndEnabled)
        {
            MessageBroker.Default.Publish(new UpdateCoolDownEvent(nameof(TargetSkillSettings), _targetSkillBehavior.cooldown.RealPoint) );
            yield return new WaitForSeconds(_targetSkillBehavior.cooldown.RealPoint);

            index = 0;
            stackDamage = 1;
            for (int projectile = 0; projectile < _targetSkillBehavior.numberProjectiles.RealPoint; projectile++)
            {
                //yield return WaitUntil;
                bool check = EnemySpawner.Instance.EnemyBehaviours.Count > index;
                Vector3 direction = check ? (EnemySpawner.Instance.EnemyBehaviours[index].transformEnemy.position - _playerTransform.position) : Vector3.forward;
                Shoot(direction);
                _audioSource.Play();
                if (check && EnemySpawner.Instance.EnemyBehaviours[index].EnemyVisitor.health <= _targetSkillBehavior.damage.RealPoint * stackDamage)
                {
                    index = index < EnemySpawner.Instance.EnemyBehaviours.Count - 1 ? index + 1 : 0;
                    stackDamage = 1;
                }
                else
                {
                    stackDamage++;
                }
                yield return new WaitForSeconds(1/_targetSkillBehavior.fireRate);
            }
        }
    }

    void Shoot(Vector3 direction)
    {
        Quaternion lookAtRotation = Quaternion.LookRotation(direction);
        TargetBulletBehaviour bullet = ProjectilesSpawner.Instance.SpawnProjectile("Target", _playerTransform.position + Vector3.up, lookAtRotation).GetComponent<TargetBulletBehaviour>();
        bullet.damage = _targetSkillBehavior.damage.RealPoint;
        bullet.Force(_targetSkillBehavior.projectileSpeed, direction);
    }
}
