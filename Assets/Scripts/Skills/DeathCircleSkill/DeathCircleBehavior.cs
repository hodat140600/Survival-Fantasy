using System.Collections;
using Assets.Scripts.Skills;
using UnityEngine;
using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Skills.LiveSkill;
using DG.Tweening;

public class DeathCircleBehavior : MonoBehaviour
{
    #region Skill info
    [SerializeField] protected Damage _damage;
    [SerializeField] protected Radius _radius;
    [SerializeField] protected bool _isEnemySkill;
    [SerializeField] protected LayerMask _opponentLayerMask;
    #endregion //Skill info

    #region Skill Time Settings
    [Header("Skill time")]
    [Tooltip("Time active waiting skill")][SerializeField] protected float _activeTime = 1.0f;
    [Tooltip("Time the explore deal damage")][SerializeField] protected float _exploreTime = 1.0f;
    [Tooltip("Time must less than Explore Time")][SerializeField] protected float _fireTime = 1.0f;
    [Tooltip("Time to destroy game object after spawn")][SerializeField] protected float _destroyTime = 3.0f;
    #endregion //Skill time Settings

    #region Object reference
    ///<summary> The skill as object : FX or prefab" </summary>
    [Tooltip("Skill FX object")][SerializeField] protected GameObject _skillObject;
    [Tooltip("List skill active after active time")][SerializeField] private GameObject[] activeChildObjects;
    #endregion //Object reference

    #region Constructor
    protected virtual void Start()
    {
        StartCoroutine(ActiveSkill());
        Destroy(gameObject, _exploreTime + _destroyTime);
    }

    public void Init(Damage damage, Radius radius, bool isEnemySkill)
    {
        _damage = damage;
        _radius = radius;
        _isEnemySkill = isEnemySkill;

        //* Get LayerMask
        LayerMask whatIsEnemy = (1 << 7);
        LayerMask whatIsPlayer = (1 << 6);

        _opponentLayerMask = _isEnemySkill ? whatIsPlayer : whatIsEnemy;

        gameObject.transform.localScale = new Vector3(_radius.BasePoint, _radius.BasePoint, _radius.BasePoint);
    }
    #endregion //Constructor

    #region Skill State
    private IEnumerator ActiveSkill()
    {
        OnActiveState(false);

        yield return new WaitForSeconds(_activeTime);

        OnActiveState(true);

        //yield return new WaitForSeconds(_exploreTime - _fireTime);

        //OnFireState(_fireTime);

        yield return new WaitForSeconds(_fireTime);

        OnCheckSphereState();
    }

    protected virtual void OnActiveState(bool state)
    {
        _skillObject.SetActive(state);
    }

    protected virtual void OnFireState(float time)
    {
        GetComponent<SpriteRenderer>().enabled = false;

        DOTween
            .To(() => _skillObject.transform.position, x => _skillObject.transform.position = x,
                new Vector3(transform.position.x, 0, transform.position.z), time)  // Axis y = 0 because the ground's axis y = 0 
            .OnComplete(OnCompleteFireState);
    }

    protected virtual void OnCheckSphereState() { }

    protected virtual void OnCompleteFireState()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }
    #endregion // Skill State

    #region Skill check and deal Damage
    protected void HitOnce()
    {
        float actualRadius = GetComponent<SphereCollider>().radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, actualRadius * 5, _opponentLayerMask);

        foreach (var hitCollider in hitColliders)
        {
            CheckTakeDamage(hitCollider);
        }
    }

    private void CheckTakeDamage(Collider collider)
    {
        //Debug.Log("Player take damage " + _damage.BasePoint);

        ITakeDamageable takeDamageable = null;

        takeDamageable = _isEnemySkill ? GameManager.Instance.player.GetComponent<LiveSkillBehavior>()
            : collider.GetComponent<EnemyBehavior>();
        takeDamageable.TakeDamage(_damage.RealPoint);
    }
    #endregion //Skill check and deal Damage
}
