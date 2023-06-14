using System;
using System.Collections;
using System.Diagnostics;
using Assets._SDK.Ads;
using Assets._SDK.Analytics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.ComponentModel;
using static MaxSdkCallbacks;

namespace RocketSg.Sdk.AdsClient
{
	public class MaxAdsClient : Singleton<MaxAdsClient>, IAdsClient
	{

		private string MaxSdkKey = AdsConfig.MaxSdkKey;
		private string InterstitialAdUnitId = AdsConfig.InterstitialAdUnitId;
		private string RewardedAdUnitId = AdsConfig.RewardedAdUnitId;
		private string BannerAdUnitId = AdsConfig.BannerAdUnitId;
		private string AOAUnitId = AdsConfig.ID_TIER_1;
		private MaxSdkBase.BannerPosition bannerPosition = MaxSdkBase.BannerPosition.BottomCenter;
		private TypeAdsMax _typeAdsUse = AdsConfig.TypeAdsUse;
		private int LevelIndex => 1; // YOUR CURRENT LEVEL HERE MapManager.Instance.currentMapIndex;

		private MaxInterstitial _interstitial;
		private MaxRewarded _rewarded;
		private MaxBanner _banner;
		private MaxAppOpenAd _appOA;
		private CappingTimer _cappingTimer;

		protected override void OnAwake()
		{
			MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
			{
				// AppLovin SDK is initialized, configure and start loading ads.
				Debug.Log("MAX SDK Initialized");

				if (_typeAdsUse.HasFlag(TypeAdsMax.AOA))
				{
					_appOA = new MaxAppOpenAd(AOAUnitId, this);
					MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += LoadAdsOther; // Tao 1 Method Load Inter, Reward
				}
				if (_typeAdsUse.HasFlag(TypeAdsMax.Interstitial))
				{
					_interstitial = new MaxInterstitial(InterstitialAdUnitId, this);
				}

				if (_typeAdsUse.HasFlag(TypeAdsMax.RewardVideo))
				{
					_rewarded = new MaxRewarded(RewardedAdUnitId, this);
				}

				if (_typeAdsUse.HasFlag(TypeAdsMax.Banner))
				{
					_banner = new MaxBanner(BannerAdUnitId, bannerPosition);
				}

				if (!_typeAdsUse.HasFlag(TypeAdsMax.AOA))
				{
					_rewarded?.LoadRewardedAd();
					_interstitial?.LoadInterstitial();
				}
			};

			MaxSdk.SetSdkKey(MaxSdkKey);

			// Devices TEST CTY
			MaxSdk.SetTestDeviceAdvertisingIdentifiers(new string[] { "1ef54a65-15dd-4740-95fc-1b34a740c544", "d6590ef7-0c38-4537-b916-6212c4088b28" });

			MaxSdk.InitializeSdk();
		}

		

		/// <summary>
		/// Set Capping Timer
		/// </summary>
		public void SetCappingTimer(CappingTimer cappingTimer)
		{
			_cappingTimer = cappingTimer;
		}

		#region Max

		public bool IsRewardedVideoReady => MaxSdk.IsRewardedAdReady(RewardedAdUnitId);

		public bool IsInterstitialLoaded => MaxSdk.IsInterstitialReady(InterstitialAdUnitId);

		public bool IsAOALoaded => MaxSdk.IsAppOpenAdReady(AOAUnitId);

		public void OnApplicationPause(bool isPause)
		{

			Debug.Log("<color=green>OnApplicationPause is :</color>" + isPause);
			if (isPause)
			{
				_cappingTimer?.Stop();
			}
			else
			{
				_cappingTimer?.Start();
			}
		}

		//CheckResumedAds
		public bool HasResumedFromAds()
		{
			return (_appOA!=null && _appOA.HasResumed) && (_rewarded!= null && _rewarded.HasResumed)  && (_interstitial != null && _interstitial.HasResumed);
		}


		/// <summary>
		/// Load Ads Other
		/// </summary>
		private void LoadAdsOther(string adUnitId, MaxSdkBase.AdInfo adInfo)
		{
			_rewarded?.LoadRewardedAd();
			_interstitial?.LoadInterstitial();
		}


		/// <summary>
		/// Long ads (it is often 30sec)
		/// </summary>
		/// <param name="placementName"></param>
		/// <param name="onRewarded"></param>
		public void ShowRewardedVideo(int levelIndex, string placementName, Action<ShowResult> onRewarded = null)
		{

			Debug.Log("<color=green>Show Video Reward</color>");
			_rewarded?.ShowRewardedAd(levelIndex, placementName, (result) =>
			{

				AnalyticsService.LogEventRewardedVideoShow(levelIndex, AnalyticsEvent.REWARDED_VIDEO_SHOW);
				onRewarded.Invoke(result);

			});
		}


		/// <summary>
		/// Show short ads 
		/// </summary>
		/// <param name="placementName"></param>
		/// <param name="onRewarded"></param>
		public void ShowInterstitial(int levelIndex, string placementName)
		{

			if (_cappingTimer.IsInterstitialCapped() && IsInterstitialLoaded)
			{
				Debug.Log("<color=green>Show Interstitial</color>");
				_interstitial?.ShowInterstitial(levelIndex, placementName);
				AnalyticsService.LogEventInterstitialShow(levelIndex, AnalyticsEvent.INTERSTITIAL_SHOW);
				_cappingTimer.Restart();
			}
			else
			{
				Debug.Log("<color=green>Not able to show Interstitial</color>");
				Debug.Log("capping time: " + _cappingTimer.Cappingtime.ToString());
			}
		}

		/// <summary>
		/// Show banner
		/// </summary>
		public void ShowBanner(bool isShow)
		{
			StartCoroutine(WaitToShowBanner(isShow));

			//_banner.ToggleBannerVisibility(isShow);

		}
		IEnumerator WaitToShowBanner(bool isActive)
		{

			while (_banner == null)
			{
				yield return new WaitForSecondsRealtime(1f);
			}

			Debug.Log("<color=green>Show Banner</color>");
			_banner.ToggleBannerVisibility(isActive);
		}

		/// <summary>
		/// Show ShowAOA
		/// </summary>
		public void ShowAOA()
		{
			Debug.Log("<color=green>Show AOA</color>");
			_appOA?.ShowAdIfReady();
			AnalyticsService.LogEventAppOpenAdsShow();
		}

		#endregion
	}
}