using Assets.Scripts.Skills.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningSkill : MonoBehaviour
{
    [SerializeField]
    private GameObject _lightningPrefab;
    [SerializeField]
    private Transform _playerTransform;
    private float _fireRate = 1f;
    [SerializeField]
    private AudioSource _audioSource;
    public Speed attackSpeed;
    public Damage damage;
    public NumberProjectiles numberProjectiles;
    public Radius radius;
    public Cooldown cooldown;
    public int timeBouncing = 3;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(CastSKill());
    }

    IEnumerator CastSKill()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown.RealPoint);
            for (int projectile = 0; projectile < numberProjectiles.RealPoint; projectile++)
            {
                Shoot();
                _audioSource.Play();

                yield return new WaitForSeconds(1 / _fireRate);
            }
        }
    }
    void Shoot()
    {
        Lightning chainLightning = ProjectilesSpawner.Instance.SpawnProjectile("Lightning", _playerTransform.position + Vector3.up, Quaternion.identity).GetComponent<Lightning>();

        if (chainLightning != null)
        {
            chainLightning.Damage = damage.RealPoint;
            chainLightning.BouncingTime = timeBouncing;
        }
    }
}
