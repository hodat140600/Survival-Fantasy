using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBoss : MonoBehaviour
{
    [SerializeField] protected bool _isEnemySkill;
    [SerializeField] protected LayerMask _opponentLayerMask;

    #region Skill Time Settings
    [Header("Skill time")]
    [Tooltip("Time active waiting skill")] [SerializeField] protected float _activeTime = 1.0f;
    [Tooltip("Time the explore deal damage")] [SerializeField] protected float _exploreTime = 1.0f;
    [Tooltip("Time must less than Explore Time")] [SerializeField] protected float _fireTime = 1.0f;
    [Tooltip("Time to destroy game object after spawn")] [SerializeField] protected float _destroyTime = 3.0f;
    #endregion //Skill time Settings

    #region Object reference
    ///<summary> The skill as object : FX or prefab" </summary>
    [Tooltip("Skill FX object")] [SerializeField] protected GameObject _skillObject;
    [Tooltip("List skill active after active time")] [SerializeField] private GameObject[] activeChildObjects;
    #endregion //Object reference

    #region Constructor
    protected virtual void Start()
    {
        StartCoroutine(ActiveSkill());
        Destroy(gameObject, _exploreTime + _destroyTime);
    }

    #endregion //Constructor

    #region Skill State
    private IEnumerator ActiveSkill()
    {
        OnActiveState(false);

        yield return new WaitForSeconds(_activeTime);

        OnActiveState(true);

        yield return new WaitForSeconds(_fireTime);

        OnCheckSphereState();
    }

    void OnActiveState(bool state)
    {
        _skillObject.SetActive(state);
    }

    void OnFireState(float time)
    {
        GetComponent<SpriteRenderer>().enabled = false;

        DOTween
            .To(() => _skillObject.transform.position, x => _skillObject.transform.position = x,
                new Vector3(transform.position.x, 0, transform.position.z), time)  // Axis y = 0 because the ground's axis y = 0 
            .OnComplete(OnCompleteFireState);
    }

    void OnCheckSphereState() { }

    void OnCompleteFireState()
    {
        GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
    }
    #endregion // Skill State

}
