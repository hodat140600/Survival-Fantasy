using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class RewardManager : Singleton<RewardManager>
{
    private const float POSITION_HEIGHT = 3.0f;
    private const float RISE_TIME = 0.5f;
    private const float PLAYER_SENSOR_AREA = 1.5f;
    private const float FPS_CONFIG = 0.02f;
    private Transform _destinationObject;

    public string[] listStaticItem;

    private SpinMovement _spinMovement;

    private void Start()
    {
        _destinationObject = GameManager.Instance.playerTransform;

        _spinMovement = gameObject.GetComponent<SpinMovement>();
        _spinMovement.Init(listStaticItem);
    }

    public void GetRewardNearByPlayer(float playerRadius)
    {
        if (RewardSpawner.Instance.GetRewardItemInactive() == null)
        {
            return;
        }

        foreach (RewardItem rewardItem in RewardSpawner.Instance.GetRewardItemInactive())
        {
            if (rewardItem.isActives == false && IsInRange(rewardItem.currentPosition, playerRadius))
            {
                ActiveRewardItem(rewardItem);
            }
        }
    }

    private bool IsInRange(Vector3 rewardItemVector, float distance)
    {
        return Vector3.Distance(rewardItemVector, _destinationObject.position) < distance;
    }

    ///<summary> Call and check is static item for running </summary>
    private void ActiveRewardItem(RewardItem rewardItem)
    {
        if (IsStaticItem(rewardItem.name) == false)
        {
            rewardItem.isActives = true;
            Rise(rewardItem);

            return;
        }

        if (Vector3.Distance(rewardItem.currentPosition, _destinationObject.position) < PLAYER_SENSOR_AREA)
        {
            rewardItem.isActives = true;
            ActiveBehaviorRewardItem(rewardItem);
        }
    }

    ///<summary> Rise exp gem to the air with POSITION_HEIGHT and RISE_TIME  </summary>
    private void Rise(RewardItem rewardItem)
    {
        DOTween.To(() => rewardItem.currentPosition, position => rewardItem.currentPosition = position,
            new Vector3(rewardItem.currentPosition.x, POSITION_HEIGHT, rewardItem.currentPosition.z), RISE_TIME)
            .OnComplete(() =>
            {
                //* Start Curve movement
                // StartCoroutine(MoveToPlayer(rewardItem));
                MainThreadDispatcher.StartUpdateMicroCoroutine(MoveToPlayer(rewardItem));
            }
        );
    }

    ///<summary> The reward item will move to player by time </summary>
    bool isTrue = false;
    private const string Gold = "Gold";
    private const string Magnet = "Magnet";
    private const string TNT = "Tnt";
    private const string Bomb = "Bomb";
    private const string Chest = "Chest";
    private const string Collect = "Collect";
    private string[] SwitchStrings = new string[] {Gold, Magnet, Bomb, Chest };
    IEnumerator MoveToPlayer(RewardItem rewardItem)
    {
        float returnTime = 0.0f;

        while (true)
        {
            //* Get and calculate position 
            rewardItem.currentPosition = Calculate(returnTime, rewardItem.currentPosition, new Vector3(_destinationObject.position.x, 0, _destinationObject.position.z));

            //* if the exp gem is near player then disappear
            if (Vector3.Distance(rewardItem.currentPosition, _destinationObject.position) < PLAYER_SENSOR_AREA)
            {
                // active skill 
                ActiveBehaviorRewardItem(rewardItem);
                
                break;
            }

            // returnTime = Mathf.Clamp(returnTime + FPS_CONFIG, 0, 1); //* condition 0 < t < 1
            // yield return new WaitForSeconds(FPS_CONFIG);
            returnTime = Mathf.Clamp(returnTime + Time.deltaTime, 0, 1); //* condition 0 < t < 1
            yield return null;
        }
    }
    void PlayRewardItemCollectSound(string name)
    {
        MessageBroker.Default.Publish(new PlaySoundEvent(name + Collect));
    }
    ///<summary> Active behavior skill of reward item </summary>
    private void ActiveBehaviorRewardItem(RewardItem rewardItem)
    {
        rewardItem.gameObjectPrefab.GetComponent<IRewardItem>().Active();
        GameManager.Instance.Vibrate();
        switch (SwitchStrings.FirstOrDefault<string>(s => rewardItem.name.Contains(s)))
        {
            case Gold:
                PlayRewardItemCollectSound(Gold);
                break;
            case Magnet:
                PlayRewardItemCollectSound(Magnet);
                break;
            case Bomb:
                PlayRewardItemCollectSound(TNT);
                break;
            case Chest:
                PlayRewardItemCollectSound(Chest);
                break;
            default:
                break;
        }
        RewardSpawner.Instance.DestroyReward(rewardItem);
    }

    public Vector3 Calculate(float t, Vector3 p0, Vector3 p1)
    {
        return ((1 - t) * p0) + t * p1;
    }

    public bool IsStaticItem(string itemName)
    {
        for (int i = 0; i < listStaticItem.Length; i++)
        {
            if (itemName.Contains(listStaticItem[i]))
            {
                return true;
            }
        }
        return false;
    }
}
