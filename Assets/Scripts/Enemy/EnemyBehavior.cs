
using UnityEngine;
using UniRx;
using Assets.Scripts.Skills.LiveSkill;
using Assets.Scripts.Skills;
using Sirenix.OdinInspector;


//[DefaultExecutionOrder(10)]
public class EnemyBehavior : MonoBehaviour, ITakeDamageable
{
    [FoldoutGroup("Model" ,true, 0)]
    [SerializeField]
    private GameObject _model;

    [FoldoutGroup("Model", true, 0)] 
    [FoldoutGroup("Model/IK Contraints", true, 0)] 
    public Transform LeftHandIK;
    [FoldoutGroup("Model", true, 0)]
    [FoldoutGroup("Model/IK Contraints", true, 0)]
    public Transform RightHandIK;
    [FoldoutGroup("Model", true, 0)]
    [FoldoutGroup("Model/IK Contraints", true, 0)]
    public Transform HeadIK;

    [FoldoutGroup("Model", true, 0)]
    [FoldoutGroup("Model/Props", true, 0)]
    public Transform LeftHandProp;
    [FoldoutGroup("Model", true, 0)]
    [FoldoutGroup("Model/Props", true, 0)]
    public Transform RightHandProp;
    [FoldoutGroup("Model", true, 0)]
    [FoldoutGroup("Model/Props", true, 0)]
    public Transform HeadProp;

    [SerializeField]
    [FoldoutGroup("References", true, 0)]
    private Animator _animator;
    [SerializeField]
    [FoldoutGroup("References", true, 0)]
    private Transform _transform;
    [FoldoutGroup("References", true, 0)]
    public Transform transformEnemy;
    [FoldoutGroup("References", true, 0)]
    public Rigidbody rigidbody;

    [Space(20)]
    [SerializeField]
    [FoldoutGroup("Values", true, 0)]
    private float _attackDistance = 3f;
    [FoldoutGroup("Values", true, 0)]
    public float _cooldown = 1f; //seconds
    [FoldoutGroup("Values", true, 0)]
    public bool isBoss;
    [FoldoutGroup("Values", true, 0)]
    public bool isSubBoss;


    private float _turnspeed = 8f;
    private float lastAttackedAt;
    private Transform _playerTransform;
    private bool _isDead;
    private EnemyVisitor _statusEnemy;
    [HideInInspector]
    public LiveSkillBehavior liveSkillBehavior;
    

    public bool IsBoss { get => isBoss; set => isBoss = value; }
    public bool IsDead { get => _isDead; set => _isDead = value; }
    public Transform PlayerTransform { get => _playerTransform;private set => _playerTransform = value; }

    public EnemyVisitor EnemyVisitor => _statusEnemy;

    private void Awake()
    {
        PlayerTransform = GameManager.Instance.playerTransform;
    }
    private void OnEnable()
    {
        IsDead = false;
        lastAttackedAt = -9999f;
        if(liveSkillBehavior == null)liveSkillBehavior = PlayerTransform.GetComponent<LiveSkillBehavior>();
    }

    private void Reset()
    {
        _model = gameObject.transform.GetChild(0).gameObject;
        _animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        _transform = this.transform;
        _attackDistance = 9f;
        _cooldown = 1f;
        isSubBoss = true ? gameObject.name.Contains("SubBoss") : false;
        isBoss = true ? gameObject.name.Contains("Boss") && !isSubBoss : false;
        transformEnemy = this.transform;

        var IK = GetChildExtension.GetAllChildren(_model.transform);

        LeftHandProp = _model.transform.Find(nameof(LeftHandProp)).transform;
        RightHandProp = _model.transform.Find(nameof(RightHandProp)).transform;
        HeadProp = _model.transform.Find(nameof(HeadProp)).transform;


        LeftHandIK = IK.Find(x => x.name == nameof(LeftHandIK)).transform;
        RightHandIK = IK.Find(x => x.name == nameof(RightHandIK)).transform;
        HeadIK = IK.Find(x => x.name == nameof(HeadIK)).transform;

        LeftHandProp.transform.position = LeftHandIK.transform.position;
        RightHandProp.transform.position = RightHandIK.transform.position;
        HeadProp.transform.position = HeadIK.transform.position;

        if(LeftHandProp.childCount >= 1) LeftHandProp.GetChild(0)?.SetParent(LeftHandIK.transform);
        else if(LeftHandIK.childCount >= 1) LeftHandIK.GetChild(0)?.SetParent(LeftHandProp.transform);

        if(RightHandProp.childCount >= 1) RightHandProp.GetChild(0)?.SetParent(RightHandIK.transform);
        else if(RightHandIK.childCount >= 1) RightHandIK.GetChild(0)?.SetParent(RightHandProp.transform);

        if(HeadProp.childCount >= 1) HeadProp.GetChild(0)?.SetParent(HeadIK.transform);
        else if(HeadIK.childCount >= 1) HeadIK.GetChild(0)?.SetParent(HeadProp.transform);
    }


    Vector3 direction;
    Quaternion newRotation;
    float distance;
    public float Distance => distance;
    int hashAttack = Animator.StringToHash("Attack");
    int hashDeath = Animator.StringToHash("Death");


    public void MoveToPositionWithRotation(Vector3 playerPosition, float delta)
    {
        UpdateDirectionAndDistance(_transform, playerPosition);
        if (distance <= _attackDistance)
        {
            if (Time.time > lastAttackedAt + _cooldown)
            {
                _animator.SetTrigger(hashAttack);
                _statusEnemy.Attack(PlayerTransform.gameObject);
                lastAttackedAt = Time.time;
            }
        }
        else
        {
            rigidbody.MovePosition(_transform.position + direction * _statusEnemy.speed * delta);
            newRotation = Quaternion.LookRotation(direction);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, newRotation, delta * _turnspeed);
        }
    }

    public void InitEnemyStatus(EnemyVisitor enemyStatus)
    {
        _statusEnemy = enemyStatus;
    }

    public void TakeDamage(int damage)
    {
        SpawnDamageNumber.Instance.SpawnDamageNumberMesh(_transform.position + Vector3.up, _transform, _statusEnemy.TakeDamage(damage));
        //HitImpactSpawner.Instance.SpawnHitImpactForOtherSkill(_transform.position + Vector3.up, _transform.rotation);
        //_healthEnemy.TakeDamage(damage);

        if (_statusEnemy.health <= 0)
        {
            IsDead = true;
            HitImpactSpawner.Instance.SpawnHitImpact("Death", _transform.position, Quaternion.identity);
            DestroyEnemy();
            if (isBoss)
            {
                return;
            }
            if (isSubBoss)
            {
                MessageBroker.Default.Publish(new EnemyDropItemEvent { position = _transform.position });
            }
            ExpSpawner.Instance.InstantiateExpGem(this.gameObject);
        }
    }
    private void DestroyEnemy()
    {
        if (isBoss && isActiveAndEnabled)
        {
            //Debug.Log("Boss Dead !");
            _animator.SetTrigger(hashDeath);
            Invoke(nameof(ReturnEnemyObject), 3f);
            MessageBroker.Default.Publish(new BossDeadEvent());
            return;
        }
        ReturnEnemyObject();
    }
    private void ReturnEnemyObject()
    {
        EnemySpawner.Instance.ReturnToPool(gameObject);
    }
    private void UpdateDirectionAndDistance(Transform enemy, Vector3 targetPosition)
    {
        var tempPosition = enemy.position; 
        float xD = targetPosition.x - tempPosition.x;
        float zD = targetPosition.z - tempPosition.z;
        direction.x = xD;
        direction.z = zD;
        direction = direction.normalized;
        distance = xD * xD + zD * zD;
    }
}
