using DG.Tweening;
using DigitalRuby.LightningBolt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningBehavior : MonoBehaviour
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

    private const float delta = 0.02f;
    public const string TAG = "Lightning";
    Vector3 direction;
    Quaternion newRotation;
    float distance;
    //  IEnumerator StartChasingTarget()
    //  {
    //while(_jumpCount < BouncingTime){
    //          UpdateDirectionAndDistance(_transform, _targetList[_jumpCount].transform.position);
    //          _rigidbody.MovePosition(_transform.position + direction * _speed * delta);
    //          newRotation = Quaternion.LookRotation(direction);
    //          _transform.rotation = Quaternion.Slerp(_transform.rotation, newRotation, delta * _turnspeed);
    //          if(distance <= 1f)
    //          {
    //              _targetList[_jumpCount].TakeDamage(Damage);
    //              _jumpCount++;
    //          }
    //    yield return null;
    //      }
    //      ProjectilesSpawner.Instance.ReturnPoolProjectile(this.gameObject, TAG);
    //  }
    //private void Start()
    //{
    //    StartCoroutine(StartChasingTarget());
    //}
    private void OnEnable()
    {
        _jumpCount = 0;
    }
    private void FixedUpdate()
    {
        if (_jumpCount >= BouncingTime || _jumpCount >= EnemySpawner.Instance.EnemyBehaviours.Count)
        {
            ProjectilesSpawner.Instance.ReturnPoolProjectile(this.gameObject, TAG);
            return;
        }
        if (EnemySpawner.Instance.EnemyBehaviours[_jumpCount] == null || !EnemySpawner.Instance.EnemyBehaviours[_jumpCount].isActiveAndEnabled)
        {
            _jumpCount++;
            if (_jumpCount >= BouncingTime || _jumpCount >= EnemySpawner.Instance.EnemyBehaviours.Count)
            {
                ProjectilesSpawner.Instance.ReturnPoolProjectile(this.gameObject, TAG);
                return;
            }
        }
        UpdateDirectionAndDistance(_transform, EnemySpawner.Instance.EnemyBehaviours[_jumpCount].transformEnemy.position);
        //_transform.rotation = Quaternion.Slerp(_transform.rotation, newRotation, delta * _turnspeed);
        //newRotation = Quaternion.LookRotation(direction);
        //move towards the player
        transform.position += direction * Time.deltaTime * _speed;
        if (distance <= 4f)
        {
            EnemySpawner.Instance.EnemyBehaviours[_jumpCount].TakeDamage(Damage);
            _jumpCount++;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other == _targetList[_jumpCount])
    //    {
    //        _targetList[_jumpCount].TakeDamage(Damage);
    //        _jumpCount++;
    //        if(_jumpCount >= BouncingTime)
    //        {
    //            ProjectilesSpawner.Instance.ReturnPoolProjectile(this.gameObject, TAG);
    //        }
    //    }
    //}

    //public void SetListTarget(List<EnemyBehavior> targetList)
    //{
    //    _targetList = targetList;
    //}
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
