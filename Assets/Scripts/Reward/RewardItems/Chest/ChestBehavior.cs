using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ChestBehavior : MonoBehaviour, IRewardItem
{
    public void Active()
    {
        MessageBroker.Default.Publish(new RewardEvent());
        //Debug.Log("<color=Cyan> Player choice skill </color>");
    }
}
