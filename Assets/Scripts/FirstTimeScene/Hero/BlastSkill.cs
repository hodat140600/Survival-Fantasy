using Assets.Scripts.Skills.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastSkill : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioSource _blastSource;
    [SerializeField]
    private EnemiesSpawner enemiesSpawner;
    Transform _playerTransform;
    public GameObject bulletPrefab;
    public float force = 1f;
    public Speed attackSpeed;
    public Damage damage;
    public NumberProjectiles numberProjectiles;
    public Radius radius;
    public Cooldown cooldown;
    public float flySpeed;
    private void Start()
    {
        _playerTransform = transform;
        StartCoroutine(ShootProjectile());
    }
    int index = 0;
    IEnumerator ShootProjectile()
    {
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(cooldown.RealPoint);
            for (int projectile = 0; projectile < numberProjectiles.RealPoint; projectile++)
            {
                int random = Random.Range(0, enemiesSpawner.EnemyBehaviours.Count);
                Vector3 direction = (enemiesSpawner.EnemyBehaviours[random].transformEnemy.position - _playerTransform.position);
                Shoot(direction);
                _audioSource.Play();
                yield return new WaitForSeconds(1 / attackSpeed.RealPoint);
            }
        }
    }

    void Shoot(Vector3 direction)
    {
        Quaternion lookAtRotation = Quaternion.LookRotation(direction);
        Blast blast = ProjectilesSpawner.Instance.SpawnProjectile("Blast", _playerTransform.position + Vector3.up, lookAtRotation).GetComponent<Blast>();
        blast.audioSource = _blastSource;
        blast.damage = damage.RealPoint;
        blast.Force(flySpeed, direction);
    }
}
