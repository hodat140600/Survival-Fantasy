using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilesSpawner : Singleton<ProjectilesSpawner>
{
    [SerializeField]
    private MyPooler.ObjectPooler _projectilePooler;
    private static float _timeLast = 8f;
    private void Reset()
    {
        _projectilePooler = GetComponent<MyPooler.ObjectPooler>();
    }

    public GameObject SpawnProjectile(string tag, Vector3 position, Quaternion rotation)
    {
        GameObject projectile = _projectilePooler.GetFromPool(tag, position, rotation);
        //StartCoroutine(ReturnPool(hitImpact, tag));
        return projectile;
    }
    public void ReturnPoolProjectile(GameObject hitImpact, string tag)
    {
        _projectilePooler.ReturnToPool(tag, hitImpact);
    }
    WaitForSeconds timeLast = new WaitForSeconds(_timeLast);
    //IEnumerator ReturnPool(GameObject hitImpact, string tag)
    //{
    //    yield return timeLast;
    //    if (hitImpact.activeInHierarchy)
    //    {
    //        _projectilePooler.ReturnToPool(tag, hitImpact);
    //    }
    //}

    public void DisableAll()
    {
        foreach(var item in _projectilePooler.activeObjects)
        {
            //foreach(var hitImpact in item.Value)
            //{
            //    ReturnPoolProjectile(hitImpact, item.Key);
            //}
            var count = item.Value.Count;
            for(int index = 0; index < count; index++)
            {
                // always remove first index
                ReturnPoolProjectile(item.Value[0], item.Key);
            }
        }
    }
}
