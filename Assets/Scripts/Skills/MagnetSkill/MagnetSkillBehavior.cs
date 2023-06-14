using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using UnityEngine;
using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Utils;

public class MagnetSkillBehavior : SkillBehavior, IRadiusSkillBehavior
{
    private const float SCANNING_GROUND_TIME = 0.02f;
    public Radius rewardRadius;

    private void Awake()
    {
        radius = new Radius();
        rewardRadius = new Radius();
    }

    private void Start()
    {
        StartCoroutine(ScanningGround());
    }

    IEnumerator ScanningGround()
    {
        while (true)
        {
            yield return new WaitForSeconds(SCANNING_GROUND_TIME);
            ExpGemManager.Instance.GetGemsNearByPlayer(radius.RealPoint);
            RewardManager.Instance.GetRewardNearByPlayer(rewardRadius.RealPoint);
        }
    }

    public void IncreaseRadiusPercent(int percent)
    {
        radius.Percent += percent;
    }

}
