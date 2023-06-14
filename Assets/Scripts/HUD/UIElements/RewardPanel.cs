using System;
using System.Collections.Generic;
using Assets._SDK.Ads.AdsClient;
using Assets._SDK.Analytics;
using Assets.Scripts.Skills;
using Manager.HUD.UIElements;
using RocketSg.Sdk.AdsClient;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class RewardPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _auraLight;
    [SerializeField]
    private Image[] _bonusImages;
    private List<ISkillSettings> _skills;
    private SkillDescriptionSystem _skillDescription;


    private void Awake()
    {
        _skillDescription = UIManager.Instance.gameObject.GetComponent<SkillDescriptionSystem>();
    }
    private void OnEnable()
    {
        _skills = GameManager.Instance.skillSystem.GetSkillsReward();
        SetPanel();
        AdsClientManager.Instance.AdsClient.ShowInterstitial(LevelManager.Instance.CurrentChapter, AnalyticsEvent.INTERSTITIAL_SHOW);
    }
    private float _speed = 2f;

    private Vector3[] zAxis = { new Vector3(0, 0, 1), new Vector3(0, 0, -1) };

    void Update()
    {
        for (int i = 0; i < _auraLight.transform.childCount; i++)
        {
            _auraLight.transform.GetChild(i).transform.RotateAround(_auraLight.transform.position, zAxis[i], _speed);
        }
    }

    public void SetPanel()
    {
        for (int i = 0; i < _skills.Count; i++)
        {
            _bonusImages[i].sprite = _skillDescription.GetSkillIcon(_skills[i].GetType().Name);
        }
    }

    public void OnGetOneButton()
    {
        LevelUpSkillByPosition(0);//recieved 1st Skill
        UIManager.Instance.ExitRewardChapter();
    }

    public void OnGetAllButton()
    {
        if (AdsClientManager.Instance.AdsClient.IsRewardedVideoReady)
        {
            AdsClientManager.Instance.AdsClient.ShowRewardedVideo(LevelManager.Instance.CurrentChapter, AnalyticsEvent.REWARDED_VIDEO_SHOW, rewarded =>
            {
                if (rewarded == ShowResult.Failed || rewarded == ShowResult.Skipped) return;
                GetReward();

			});
        }else
        {
            GetReward();
		}
    }

    private void GetReward()
    {
		for (int i = 0; i < _skills.Count; i++)
		{
			LevelUpSkillByPosition(i);
		}
		UIManager.Instance.ExitRewardChapter();
	}

    private void LevelUpSkillByPosition(int position)
    {
        GameManager.Instance.skillSystem.SelectSkill(_skills[position]);
        UIManager.Instance.skillContainer.SetActive(_skills[position]);
    }
}
