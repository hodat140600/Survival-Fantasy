using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class ChainLightningController : MonoBehaviour
{
    [SerializeField]
    private GameObject _lightningPrefab;
    [SerializeField]
    private Transform _playerTransform;
    private float _fireRate = 1f;
    [SerializeField]
    private ChainLightningSkillBehavior _chainLightningSkillBehavior;
    [SerializeField]
    private AudioSource _audioSource;

    private void Start()
    {
        if (_playerTransform == null)
        {
            _playerTransform = GameManager.Instance.playerTransform;
        }
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(CastSKill());
    }

    IEnumerator CastSKill()
    {
        while (true)
        {
            MessageBroker.Default.Publish(new UpdateCoolDownEvent(nameof(ChainLightningSkillSetting), _chainLightningSkillBehavior.cooldown.RealPoint));
            yield return new WaitForSeconds(_chainLightningSkillBehavior.cooldown.RealPoint);
            for (int projectile = 0; projectile < _chainLightningSkillBehavior.numberProjectiles.RealPoint; projectile++)
            {
                Shoot();
                _audioSource.Play();

                yield return new WaitForSeconds(1/_fireRate);
            }
        }
    }
    void Shoot()
    {
        //Vector3 direction = trans != null ? trans.position - _playerTransform.position : Vector3.forward;
        //Quaternion lookAtRotation = Quaternion.LookRotation(direction);
        ChainLightningBehavior chainLightning = ProjectilesSpawner.Instance.SpawnProjectile("Lightning", _playerTransform.position + Vector3.up, Quaternion.identity).GetComponent<ChainLightningBehavior>();

        if (chainLightning != null)
        {
            chainLightning.Damage = _chainLightningSkillBehavior.damage.RealPoint;
            chainLightning.BouncingTime = _chainLightningSkillBehavior.timeBouncing;
            //chainLightning.SetListTarget(_listTarget);
        }
    }
    public void Init(ChainLightningSkillBehavior chainLightningSkillBehavior)
    {
        _chainLightningSkillBehavior = chainLightningSkillBehavior;
        this.gameObject.SetActive(true);
    }
}
