
using UniRx;
using UnityEngine;

///<summary> Only using for calculator spawn and drop rate </summary>
public class RewardSystem : MonoBehaviour
{
    [SerializeField] private RewardItemSettings[] _rewardItems;
    private Randomize<ScriptableObject> _randomize;
    [SerializeField] private int _dropTimPeriod;

    void Start()
    {
        this._randomize = new Randomize<ScriptableObject>(GetRewardItems(), GetChances());

        MessageBroker.Default.Receive<TickEvent>()
            .Where(tickEvent => tickEvent.secondNumber % this._dropTimPeriod == 0)
            .Subscribe(tickEvent => DropItem());
        MessageBroker.Default.Receive<EnemyDropItemEvent>()
            .Subscribe(enemyDropItemEvent => { DropItemFromEnemy(enemyDropItemEvent.position); });
    }
    private void DropItem()
    {
        RandomizeItem itemReward = _randomize.Run();

        var itemRewardSettings = itemReward.Item as IRewardSettings;

        RewardSpawner.Instance.InstantiateReward(itemRewardSettings);
    }
    private void DropItemFromEnemy(Vector3 position)
    {
        //Debug.Log("Gold Drop");
        RewardSpawner.Instance.InstantiateRewardItem(_rewardItems[_rewardItems.Length-1].RewardItems as IRewardSettings, position);
    }

    #region Get data from array
    private int[] GetChances()
    {
        int[] chances = new int[_rewardItems.Length];
        for (int i = 0; i < _rewardItems.Length; i++)
        {
            chances[i] = _rewardItems[i].MaxChances;
        }
        return chances;
    }

    private ScriptableObject[] GetRewardItems()
    {
        ScriptableObject[] rewardItems = new ScriptableObject[_rewardItems.Length];
        for (int i = 0; i < _rewardItems.Length; i++)
        {
            rewardItems[i] = _rewardItems[i].RewardItems;
        }
        return rewardItems;
    }
    #endregion //Get data from array
}