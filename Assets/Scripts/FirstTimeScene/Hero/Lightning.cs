using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField]
    private int _damage = 5;
    public int Damage { get => _damage; set => _damage = value; }
    public int BouncingTime { get => _bouncingTime; set => _bouncingTime = value; }

    [SerializeField]
    private int _bouncingTime = 3;

    [SerializeField] private int _jumpCount = 0;
    [SerializeField] private int _speed = 15;
    private float _turnspeed = 8f;

    [SerializeField] private Transform _transform;
    [SerializeField] private Rigidbody _rigidbody;
    public const string TAG = "Lightning";
    Vector3 direction;
    float distance;
    EnemiesSpawner enemiesSpawner;
    private void OnEnable()
    {
        _jumpCount = 0;
        if (EnemiesBehaviourController.Instance != null)
        {
            enemiesSpawner = EnemiesBehaviourController.Instance.EnemySpawner;
            StartCoroutine(ChaseTarget());
        }
    }
    WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    IEnumerator ChaseTarget()
    {
        while (_jumpCount < BouncingTime 
            && _jumpCount <= enemiesSpawner.MinionBehaviours.Count 
            && enemiesSpawner.MinionBehaviours.Count > 0)
        {
            UpdateDirectionAndDistance(_transform, enemiesSpawner.MinionBehaviours[0].transformEnemy.position);
            transform.position += direction * Time.deltaTime * _speed;
            if (distance <= 4f)
            {
                enemiesSpawner.MinionBehaviours[0].TakeDamage(Damage);
                _jumpCount++;
            }
            yield return WaitForFixedUpdate;
        }
        ProjectilesSpawner.Instance.ReturnPoolProjectile(this.gameObject, TAG);
    }
    private void UpdateDirectionAndDistance(Transform start, Vector3 targetPosition)
    {
        var tempPosition = start.position;
        float xD = targetPosition.x - tempPosition.x;
        float zD = targetPosition.z - tempPosition.z;
        direction.x = xD;
        direction.z = zD;
        direction = direction.normalized;
        distance = xD * xD + zD * zD;
    }
}
