using Assets._SDK.Analytics;
using Assets._SDK.Logger;
using System;
using System.Collections;
using UnityEngine;

namespace RocketSg.Sdk.AdsClient
{
    public class MaxRewarded
    {
        private readonly string _rewardedAdUnitId;
        private readonly MaxAdsClient _adsClient;
        private int _retryAttempt;
        private Action<ShowResult> _onRewarded;
        private ShowResult _showResult;
		private bool _hasResumed = false;

		public bool HasResumed => _hasResumed;

		public MaxRewarded(string rewardedAdUnitId, MaxAdsClient adsClient)
        {
            _rewardedAdUnitId = rewardedAdUnitId;
            _adsClient = adsClient;
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            //LoadRewardedAd();
        }

		public void LoadRewardedAd()
        {
            MaxSdk.LoadRewardedAd(_rewardedAdUnitId);
        }

        public void ShowRewardedAd(int levelIndex, string placementName, Action<ShowResult> onRewarded = null)
        {
            _onRewarded = onRewarded;
            _showResult = ShowResult.Skipped;
            if (MaxSdk.IsRewardedAdReady(_rewardedAdUnitId))
            {
                MaxSdk.ShowRewardedAd(_rewardedAdUnitId);
                AnalyticsService.LogEventRewardedVideoShow(levelIndex, placementName);
                _hasResumed = true;

			}
            else
            {
                _onRewarded?.Invoke(ShowResult.Failed);
            }
        }

        private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
            Debug.Log("Rewarded ad loaded");

            // Reset retry attempt
            _retryAttempt = 0;
        }

        private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            _retryAttempt++;
            float retryDelay = (float)Math.Pow(2, Math.Min(4, _retryAttempt));

            Debug.Log("Rewarded ad failed to load with error code: " + errorInfo.Code);

            _adsClient.StartCoroutine(LoadRoutine(retryDelay));
        }

        private IEnumerator LoadRoutine(float retryDelay)
        {
            yield return new WaitForSeconds(retryDelay);
            LoadRewardedAd();
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo,
            MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad failed to display. We recommend loading the next ad
            Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
            _showResult = ShowResult.Failed;
            _onRewarded?.Invoke(_showResult);
            LoadRewardedAd();
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Rewarded ad displayed");
            _showResult = ShowResult.Skipped;
            _hasResumed = false;

		}

        private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Rewarded ad clicked");
        }

        private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad is hidden. Pre-load the next ad
            Debug.Log("Rewarded ad dismissed");
            _onRewarded?.Invoke(_showResult);
            LoadRewardedAd();
            _hasResumed = false;

		}

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
        {
            // Rewarded ad was displayed and user should receive the reward
            Debug.Log("Rewarded ad received reward");
            _showResult = ShowResult.Finished;
        }

    }
}