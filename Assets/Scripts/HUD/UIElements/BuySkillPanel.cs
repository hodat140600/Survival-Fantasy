using System;
using System.Collections.Generic;
using Assets._SDK.Ads.AdsClient;
using Assets._SDK.Analytics;
using Assets.Scripts.Skills;
using Manager.HUD.UIElements;
using RocketSg.Sdk.AdsClient;
using UnityEngine;
using UnityEngine.UI;

public class BuySkillPanel : MonoBehaviour
{
    // [SerializeField] 
    // private GameObject _player;
    [SerializeField]
    private PlayerSettings _playerSettings;
    [SerializeField]
    private GoldPanel _goldPanel;

    // private SkillSystem _skillSystem;
    private List<SkillPrice> _sellingSkillPrices;
    private SkillDescriptionSystem _skillDescriptionSystem;

    Transform _transform;

    private void OnEnable()
    {
        // _skillSystem = _player.GetComponent<SkillSystem>();
        _transform = transform;
        _goldPanel.UpdateGoldCount(_playerSettings.Gold);
        UpdateSkillContainer();
    }

    private void Start()
    {
        _goldPanel.UpdateGoldCount(_playerSettings.Gold);
    }

    public void UpdateSkillContainer()
    {
        _sellingSkillPrices = GameManager.Instance.skillSystem.GetBuyableSkills();

        for (int skillIndex = 0; skillIndex < _transform.childCount; skillIndex++)
        {
            BuySkillButton buySkillButton = _transform.GetChild(skillIndex).GetComponent<BuySkillButton>();
            SkillPrice skill = _sellingSkillPrices[skillIndex];

            bool isBuyable = skill.Price <= _playerSettings.Gold;
            buySkillButton.LoadSkill(skill, isBuyable);

            var buyButton = buySkillButton.gameObject.GetComponent<Button>();
            buyButton.onClick.RemoveAllListeners();

            var index = skillIndex;
            buyButton.onClick.AddListener(() =>
            {
                if (isBuyable)
                {
                    OnBuySkill(index);
                }
                else
                {
                    if(AdsClientManager.Instance.AdsClient.IsRewardedVideoReady)
					    AdsClientManager.Instance.AdsClient.ShowRewardedVideo(_playerSettings.LastWinChapter, AnalyticsEvent.REWARDED_VIDEO_SHOW, rewarded => 
                        {
						    if (rewarded == ShowResult.Failed || rewarded == ShowResult.Skipped) return;
						    OnLevelUpSkill(_sellingSkillPrices[index]);
                        });
                    else
						OnLevelUpSkill(_sellingSkillPrices[index]);
				}
            });
        }
    }

    private void OnBuySkill(int skillIndex)
    {
        SkillPrice skillPrice = _sellingSkillPrices[skillIndex];
        _playerSettings.Gold -= skillPrice.Price;
        OnLevelUpSkill(skillPrice);

        _goldPanel.UpdateGoldCount(_playerSettings.Gold);
    }
    private void OnLevelUpSkill(SkillPrice skillPrice)
    {
        _playerSettings.BoughtSkill(skillPrice);
        UpdateSkillContainer();
    }
}
