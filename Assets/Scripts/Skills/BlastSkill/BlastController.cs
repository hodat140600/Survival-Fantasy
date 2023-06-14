using DatHQ.Skill.Hero;
using System.Collections;
using UniRx;
using UnityEngine;

public class BlastController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;    
    [SerializeField]
    private AudioSource _blastSource;
    [SerializeField]
    private BlastSkillBehaviour _blastSkillBehavior;
    Transform _playerTransform;
    public GameObject bulletPrefab;
    [SerializeField]
    private static float fireRate = 0.5f;
    private static float cooldown = 1f;
    public float force = 1f;

    public void Init(BlastSkillBehaviour blastSkillBehavior)
    {
        _blastSkillBehavior = blastSkillBehavior;
        fireRate = 0.2f;
        cooldown = _blastSkillBehavior.cooldown.RealPoint;
        _playerTransform = GameManager.Instance.playerTransform;
        this.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ShootProjectile());
    }
    WaitUntil WaitUntil = new WaitUntil(() => EnemySpawner.Instance.EnemyBehaviours.Count > 0);
    int index = 0;
    int stackDamage = 1;
    IEnumerator ShootProjectile()
    {
        while (isActiveAndEnabled)
        {
            MessageBroker.Default.Publish(new UpdateCoolDownEvent(nameof(BlastSkillSettings), _blastSkillBehavior.cooldown.RealPoint));
            yield return new WaitForSeconds(_blastSkillBehavior.cooldown.RealPoint);
            index = 0;
            stackDamage = 1;
            for (int projectile = 0; projectile < _blastSkillBehavior.numberProjectiles.RealPoint; projectile++)
            {
                //yield return WaitUntil;
                //Debug.Log("IndexShot: " + index);
                //Debug.Log("CountShot: " + EnemySpawner.Instance.EnemyBehaviours.Count);
                //Debug.Log("CountCheck: " + (EnemySpawner.Instance.EnemyBehaviours.Count > index));
                //Debug.Log("CountValue: " + (EnemySpawner.Instance.EnemyBehaviours.Count > index ? (EnemySpawner.Instance.EnemyBehaviours[index].transformEnemy.position - _playerTransform.position) : Vector3.forward));
                bool check = EnemySpawner.Instance.EnemyBehaviours.Count > index;
                Vector3 direction = check ? (EnemySpawner.Instance.EnemyBehaviours[index].transformEnemy.position - _playerTransform.position) : Vector3.forward;
                Shoot(direction);
                _audioSource.Play();
                if (check && EnemySpawner.Instance.EnemyBehaviours[index].EnemyVisitor.health <= _blastSkillBehavior.damage.RealPoint * stackDamage)
                {
                    //Debug.Log("Index: " + index);
                    //Debug.Log("Count: " + EnemySpawner.Instance.EnemyBehaviours.Count);
                    //Debug.Log("Check: " + (index < EnemySpawner.Instance.EnemyBehaviours.Count));
                    index = index < EnemySpawner.Instance.EnemyBehaviours.Count - 1 ? index + 1 : 0;
                    stackDamage = 1;
                    //Debug.Log("IndexIncrease: " + index);
                    //Debug.Log("CountAfter: " + EnemySpawner.Instance.EnemyBehaviours.Count);
                }
                else
                {
                    stackDamage++;
                }
                yield return new WaitForSeconds(1/_blastSkillBehavior.attackSpeed);
            }
        }
    }

    void Shoot(Vector3 direction)
    {
        Quaternion lookAtRotation = Quaternion.LookRotation(direction);
        BlastBehaviour blast = ProjectilesSpawner.Instance.SpawnProjectile("Blast", _playerTransform.position + Vector3.up, lookAtRotation).GetComponent<BlastBehaviour>();
        blast.Damage = _blastSkillBehavior.damage.RealPoint;
        blast.ExplosionRadius = _blastSkillBehavior.radius.RealPoint;
        blast.audioSource = _blastSource;
        blast.Force(_blastSkillBehavior.flySpeed, direction);
    }
}
