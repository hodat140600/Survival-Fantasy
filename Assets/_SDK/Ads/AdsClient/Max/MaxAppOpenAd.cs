using Assets._SDK.Logger;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets._SDK.Ads.AdsClient;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace RocketSg.Sdk.AdsClient
{
	public class MaxAppOpenAd
	{
		private readonly MaxAdsClient _adsClient;
		private readonly string _AOAUnitId;
		private bool _isTheFirst = true;
		private int _timeStart = int.MaxValue;
		private bool _hasResumed = false;

		public bool HasResumed => _hasResumed;

		bool canShowFisrt => _isTheFirst && ((DateTime.Now.Second - _timeStart) <= AdsConfig.CONST_MAX_TIME_WAIT_FOR_SHOW_FIRST_AOA  && (DateTime.Now.Second -_timeStart) >= AdsConfig.CONST_TIME_WAIT_FOR_SHOW_FIRST_AOA);

		public MaxAppOpenAd(string AOAUnitId, MaxAdsClient adsClient)
		{
			_AOAUnitId = AOAUnitId;
			_adsClient = adsClient;

			MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
			MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += OnAOALoadedEvent;
			MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += OnAdFailedToDisplayEvent;

			ShowAdIfReady();
			_timeStart = DateTime.Now.Second;
		}

		public void ShowAdIfReady()
		{
			if (MaxSdk.IsAppOpenAdReady(_AOAUnitId))
			{
				MaxSdk.ShowAppOpenAd(_AOAUnitId);
				_hasResumed = true;
				_isTheFirst = false;

			}
			else
			{
				MaxSdk.LoadAppOpenAd(_AOAUnitId);
			}
		}

		/// <summary>
		/// Load AOA First
		/// </summary>
		private void OnAOALoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
		{
			if (canShowFisrt)
			{
				ShowAdIfReady();
			}
		}

		/// <summary>
		/// Set RusumedAd = False after close Ads
		/// </summary>
		private void OnAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
					MaxSdkBase.AdInfo adInfo)
		{
			_hasResumed = false;
		}

		public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
		{
			MaxSdk.LoadAppOpenAd(_AOAUnitId);
			_hasResumed = false;
		}
	}
}