using System;
using Assets.Scripts.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Assets._SDK.Analytics;
using Assets._SDK.Ads.AdsClient;
using RocketSg.Sdk.AdsClient;

public class ChapterWinPanel : MonoBehaviour
{
	[SerializeField]
	private Image _iconSkillEarn;

	[SerializeField]
	private TextMeshProUGUI _lootText;

	[SerializeField]
	private TextMeshProUGUI _killText;

	[SerializeField]
	private TextMeshProUGUI _goldKillText;

	[SerializeField]
	private GameObject _auraLight;

	private ISkillSettings _skillEarn;
	[SerializeField] private TextMeshProUGUI _moneyTextButton;
	private void Start()
	{
		SkillDescriptionSystem skillDescription = UIManager.Instance.gameObject.GetComponent<SkillDescriptionSystem>();
		_skillEarn = LevelManager.Instance.ChapterList[LevelManager.Instance.IndexCurrentChap - 1].skillEarn as ISkillSettings;
		_iconSkillEarn.sprite = skillDescription.GetSkillIcon(_skillEarn.GetType().Name);
		_lootText.text = GameManager.Instance.playerSettings.GoldInChapter.ToString();
		_killText.text = EnemySpawner.Instance.EnemiesKilled.ToString();
		_goldKillText.text = GameManager.Instance.playerSettings.GoldKillEnemiesInChapter.ToString();
		//var money = GameManager.Instance.playerSettings.GoldInChapter*3;//ads *3 money in chapter;
		_moneyTextButton.text = (GameManager.Instance.playerSettings.GoldInChapter * 3).ToString();
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
	public void Changelevel()
	{
		AdsClientManager.Instance.AdsClient.ShowInterstitial(LevelManager.Instance.CurrentChapter, AnalyticsEvent.INTERSTITIAL_SHOW);
		gameObject.SetActive(false);
		GameManager.Instance.playerSettings.AddSkill(_skillEarn);
		GameManager.Instance.playerSettings.AddGold(GameManager.Instance.playerSettings.GoldInChapter);
		GameManager.Instance.UpdateGameState(GameState.SelectToPlay);
		// ChapterStartPanel.Instance.GameManagerOnGameStateChanged(GameState.SelectToPlay);
	}
	public void OnAdsMoneyButton()
	{
		if (AdsClientManager.Instance.AdsClient.IsRewardedVideoReady)
		{
			AdsClientManager.Instance.AdsClient.ShowRewardedVideo(LevelManager.Instance.CurrentChapter, AnalyticsEvent.REWARDED_VIDEO_SHOW, rewarded =>
			{
				if (rewarded == ShowResult.Failed || rewarded == ShowResult.Skipped) return;
				GetReward();
			});
		}
		else
		{
			GetReward();
		}
	}
	private void GetReward()
	{
		GameManager.Instance.playerSettings.RewardAdsMoney();
		Changelevel();
	}
}
