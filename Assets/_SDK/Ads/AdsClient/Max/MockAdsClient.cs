using Assets._SDK.Logger;
using RocketSg.Sdk.AdsClient;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._SDK.Ads.AdsClient
{
	public class MockAdsClient : Singleton<MockAdsClient>, IAdsClient
	{
		private CappingTimer _cappingTimer;
		public bool IsRewardedVideoReady => true;
		public bool IsInterstitialLoaded => true;


		public bool HasResumedFromAds()
		{
			Debug.Log("HasResumedFromAds");
			return true;
		}

		public bool LoadInterstitial()
		{
			Debug.Log("LoadInterstitial");
			return true;
		}

		public void OnApplicationPause(bool isPause)
		{
			Debug.Log("OnApplicationPause " + isPause.ToString());
			if (isPause)
			{
				_cappingTimer?.Stop();
			}
			else
			{
				_cappingTimer?.Start();
			}
		}

		public void SetCappingTimer(CappingTimer cappingTimer)
		{
			_cappingTimer = cappingTimer;
		}

		public void ShowInterstitial(int levelIndex, string placementName)
		{
			if (_cappingTimer.IsInterstitialCapped())
			{
				Debug.Log("Show Interstitial");
			}
			else
			{
				Debug.Log("Not Able to ShowInterstitial");
			}
		}

		public void ShowRewardedVideo(int levelIndex, string placementName, Action<ShowResult> onRewarded = null)
		{
			onRewarded?.Invoke(ShowResult.Finished);
			Debug.Log("ShowRewardedVideo");
		}

		public void ShowBanner(bool isShow)
		{
			Debug.Log("Show Banner is :" + isShow);
		}

		public void ShowAOA()
		{
			Debug.Log("Show ShowAOA");
		}

		protected override void OnAwake()
		{
			Debug.LogWarning("Mock Ads is running");
		}
	}
}