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

        public void OnApplicationPause(bool isPause)
        {
            Debug.Log("OnApplicationPause " + isPause.ToString());
        }

        public void SetCappingTimer(CappingTimer cappingTimer)
        {
            _cappingTimer = cappingTimer;
        }

        public void ShowInterstitial(int levelIndex, string placementName)
        {
            Debug.Log("ShowInterstitial");
        }

        public void ShowRewardedVideo(int levelIndex, string placementName, Action<ShowResult> onRewarded = null)
        {
            Debug.Log("ShowRewardedVideo");
        }

        public IEnumerator ShowBanner()
        {
            while (!FirebaseService.Instance.IsConnected)
            {
                yield return new WaitForSecondsRealtime(1f);
            }

            Debug.Log("Show Banner");
        }

        protected override void OnAwake()
        {
            Debug.LogWarning("Mock Ads is running");
        }
    }
}