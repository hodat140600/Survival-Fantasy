using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitImpactSpawner : Singleton<HitImpactSpawner>
{
    [SerializeField]
    private MyPooler.ObjectPooler _hitImpactPooler;
    [SerializeField]
    private static readonly float _timeDisapear = 1.5f;
    private void Reset()
    {
        _hitImpactPooler = GetComponent<MyPooler.ObjectPooler>();
    }

    public GameObject SpawnHitImpact(string tag, Vector3 position, Quaternion rotation)
    {
        GameObject hitImpact =_hitImpactPooler.GetFromPool(tag, position + Vector3.up, rotation);
        if (hitImpact == null)
        {
            return null;
        }
        //Invoke(ReturnPoolHitImpact(hitImpact, tag), _timeDisapear);
        StartCoroutine(ReturnPoolVFX(hitImpact, tag));
        return hitImpact;
    }
    WaitForSeconds waitForSeconds = new WaitForSeconds(_timeDisapear);
    IEnumerator ReturnPoolVFX(GameObject hitImpact, string tag)
    {
        yield return waitForSeconds;
        _hitImpactPooler.ReturnToPool(tag ,hitImpact);
    }

    public void ReturnPoolHitImpact(GameObject hitImpact, string tag)
    {
        _hitImpactPooler.ReturnToPool(tag, hitImpact);
    }
    public void SpawnHitImpactForOtherSkill(Vector3 position, Quaternion rotation)
    {
        SpawnHitImpact("Hit", position, rotation);
    }
    public void DisableAll()
    {
        foreach (var item in _hitImpactPooler.activeObjects)
        {
            var count = item.Value.Count;
            for (int index = 0; index < count; index++)
            {
                // always remove first index
                ReturnPoolHitImpact(item.Value[0], item.Key);
            }
        }
    }
}
