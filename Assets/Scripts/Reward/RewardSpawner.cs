using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyPooler;

public class RewardSpawner : Singleton<RewardSpawner>
{
    private ObjectPooler _rewardItemPool;
    private const float RADIUS_SPAWN = 40.0f;

    [SerializeField] private List<RewardItem> _listReward;
    public List<RewardItem> listReward
    {
        get { return _listReward; }
    }

    private void Start()
    {
        _listReward = new List<RewardItem>();
        _rewardItemPool = GetComponent<ObjectPooler>();
    }

    public void InstantiateReward(IRewardSettings rewardSettings)
    {
        string tag = rewardSettings.GetName();
        var objectRotation = _rewardItemPool.pools.Find(objectPool => objectPool.tag.Equals(tag)).prefab.gameObject.transform.rotation;
        var rewardItemGameObject = _rewardItemPool.GetFromPool(tag, gameObject.transform.position, objectRotation);

        rewardItemGameObject.transform.position = GetVector3Spawn(GameManager.Instance.playerTransform.position);
        rewardSettings.Active(rewardItemGameObject);

        RewardItem rewardItem = new RewardItem();
        rewardItem.Init(rewardItemGameObject);

        _listReward.Add(rewardItem);
    }

    public void InstantiateRewardItem(IRewardSettings rewardSettings, Vector3 position)
    {
        string tag = rewardSettings.GetName();
        var objectRotation = _rewardItemPool.pools.Find(objectPool => objectPool.tag.Equals(tag)).prefab.gameObject.transform.rotation;
        var rewardItemGameObject = _rewardItemPool.GetFromPool(tag, gameObject.transform.position, objectRotation);

        rewardItemGameObject.transform.position = CheckInRange.GetPosOnRing(position, 5f, 5f);
        rewardSettings.Active(rewardItemGameObject);

        RewardItem rewardItem = new RewardItem();
        rewardItem.Init(rewardItemGameObject);

        _listReward.Add(rewardItem);
    }

    public void DestroyReward(RewardItem rewardItem)
    {
        RemoveFromPool(rewardItem);

        _listReward.Remove(rewardItem);
    }

    private void RemoveFromPool(RewardItem rewardItem)
    {
        string tag = rewardItem.name;
        int index = tag.IndexOf("(Clone)");
        string rewardItemTagInPool = tag.Substring(0, index);

        _rewardItemPool.ReturnToPool(rewardItemTagInPool, rewardItem.gameObjectPrefab);
    }

    private Vector3 GetVector3Spawn(Vector3 gameObjectPosition)
    {
        Vector3 randomDirection = Random.insideUnitSphere * RADIUS_SPAWN + gameObjectPosition;
        randomDirection.y = 0; // Spawn on ground

        return randomDirection;
    }

    public void ReturnAllPool()
    {
        foreach (RewardItem rewardItem in _listReward)
        {
            RemoveFromPool(rewardItem);
        }

        _listReward.Clear();
    }

    public List<RewardItem> GetRewardItemInactive()
    {
        return _listReward.FindAll(expGem => expGem.isActives == false);
    }
}