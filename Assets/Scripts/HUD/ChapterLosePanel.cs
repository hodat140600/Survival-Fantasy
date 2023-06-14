using TMPro;
using UnityEngine;
using Assets._SDK.Analytics;
using RocketSg.Sdk.AdsClient;
using Assets._SDK.Ads.AdsClient;

public class ChapterLosePanel : MonoBehaviour
{

	[SerializeField] private TextMeshProUGUI enemiesKill;
	[SerializeField] private TextMeshProUGUI goldEnemiesKill;
	[SerializeField] private TextMeshProUGUI moneyText;
	[SerializeField] private TextMeshProUGUI moneyTextButton;

	private void OnEnable()
	{
		enemiesKill.text = EnemySpawner.Instance.EnemiesKilled.ToString();
		goldEnemiesKill.text = GameManager.Instance.playerSettings.GoldKillEnemiesInChapter.ToString();
		int money = GameManager.Instance.playerSettings.GoldInChapter;
		moneyText.text = money.ToString();
		moneyTextButton.text = (money * 3).ToString();
	}
	public void Changelevel()
	{
		AdsClientManager.Instance.AdsClient.ShowInterstitial(LevelManager.Instance.CurrentChapter, AnalyticsEvent.INTERSTITIAL_SHOW);
		gameObject.SetActive(false);
		GameManager.Instance.playerSettings.AddGold(GameManager.Instance.playerSettings.GoldInChapter);
		GameManager.Instance.UpdateGameState(GameState.SelectToPlay);
		// ChapterStartPanel.Instance.GameManagerOnGameStateChanged(GameState.SelectToPlay);
	}
	public void OnAdsMoneyButtonClick()
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
