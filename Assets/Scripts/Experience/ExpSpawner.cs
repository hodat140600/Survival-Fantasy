using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Assets.Scripts.Utils;
using MyPooler;
using System.Linq;

public class ExpSpawner : Singleton<ExpSpawner>
{
    private ObjectPooler _expPool;
    [SerializeField] private List<ExperienceItem> _listExpGem;
    /*[SerializeField] private List<GameObject> _listExpGemObj;*/

    private void Start()
    {
        _listExpGem = new List<ExperienceItem>();
        _expPool = GetComponent<ObjectPooler>();
        /*for (int typeGem = 0; typeGem < _expPool.pools.Count; typeGem++)
        {
            _listExpGemObj.AddRange(_expPool.poolDictionary[_expPool.pools[typeGem].tag].ToArray());
            Debug.Log(_expPool.poolDictionary[_expPool.pools[typeGem].tag].ToArray().Length);
        }*/
    }

    public void InstantiateExpGem(GameObject enemy)
    {
        //* Get exp point from enemy when it killed
        EnemyVisitor enemyVisitor = enemy.GetComponent<EnemyVisitor>();
        int enemyExperiencePoint = enemyVisitor.enemyStatus.expPoint;

        //* Get game object from pool
        var expGemSpawnPosition = new Vector3(enemy.transform.position.x, 0, enemy.transform.position.z);
        string tag = enemyVisitor.enemyStatus.statusPrefab.name;
        var expGemObject = _expPool.GetFromPool(tag, expGemSpawnPosition, Quaternion.identity);
        if (expGemObject != null)
        {
            ExperienceItem experienceItem = new ExperienceItem();
            experienceItem.Init(enemyExperiencePoint , expGemObject);
            //experienceItem.gameObjectPrefab = expGemObject;
            _listExpGem.Add(experienceItem);
        }
    }

    public void DestroyExpGem(ExperienceItem expGem)
    {
        RemoveFromPool(expGem);

        _listExpGem.Remove(expGem);
    }

    private void RemoveFromPool(ExperienceItem expGem)
    {
        string expGemName = expGem.gameObjectPrefab.name;
        int index = expGemName.IndexOf("(Clone)");
        string gemTagInPool = expGemName.Substring(0, index);

        _expPool.ReturnToPool(gemTagInPool, expGem.gameObjectPrefab);
    }
    private void RemoveFromPool(GameObject expGem)
    {
        string expGemName = expGem.name;
        int index = expGemName.IndexOf("(Clone)");
        string gemTagInPool = expGemName.Substring(0, index);

        _expPool.ReturnToPool(gemTagInPool, expGem);
    }

    public void ReturnAllPool()
    {
        for (int indexGemObj = 0; indexGemObj < _listExpGem.Count; indexGemObj++)
        {
            /*if (_listExpGemObj[indexGemObj].activeInHierarchy)
                RemoveFromPool(_listExpGemObj[indexGemObj]);*/
            RemoveFromPool(_listExpGem[indexGemObj]);
        }
        _listExpGem.Clear();
        /*_listExpGemObj.Clear();*/
    }


    public List<ExperienceItem> GetExpGemsInactive()
    {
        return _listExpGem.FindAll(expGem => expGem.isActives == false);
    }
}
