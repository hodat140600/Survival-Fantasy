#define ENABLE_ADS
using AppsFlyerSDK;
using Assets._SDK.Ads.AdsClient.Google;
using RocketSg.Sdk.AdsClient;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._SDK.Ads.AdsClient
{
	public class AdsClientManager : Singleton<AdsClientManager>
	{
		public IAdsClient AdsClient { get; private set; }

		private CappingTimer cappingTimer;

		protected override void OnAwake()
		{
			cappingTimer = new CappingTimer();

			ConstructAdsClient();

		}
		private void ConstructAdsClient()
		{

#if ENABLE_ADS
			AdsClient = MaxAdsClient.Instance;
			//AOAManager = AppOpenAdManager.Instance;
#else
            AdsClient = MockAdsClient.Instance;
            //AOAManager = MockAppOpenAdManager.Instance;
#endif

			//MobileAds.Initialize(status =>
			//         {
			//             AOAManager.LoadAd();
			//             AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
			//         });

			AdsClient.SetCappingTimer(cappingTimer);
			cappingTimer.Start();
		}

		public void OnApplicationPause(bool isPause)
		{
			if (!isPause && AdsClient != null && !AdsClient.HasResumedFromAds())
			{
				AdsClient.ShowAOA();
			}
		}

		//private void OnAppStateChanged(AppState state)
		//{
		//    // Display the app open ad when the app is foregrounded.
		//    UnityEngine.Debug.Log("App State is " + state);
		//    if (state == AppState.Foreground)
		//    {
		//        // AOAManager.ShowAdIfAvailable();

		//    }
		//}
	}
}