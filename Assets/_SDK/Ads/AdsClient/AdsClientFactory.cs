#define ENABLE_ADS
using RocketSg.Sdk.AdsClient;
using System;
using System.Collections;
using UnityEngine;

namespace Assets._SDK.Ads.AdsClient
{
    public class AdsClientFactory
    {
        private static IAdsClient _adsClient;
        public static IAdsClient GetAdsClient()
        {
            if (_adsClient != null)
            {
                return _adsClient;
            }

            if (!FirebaseService.Instance.IsConnected)
            {
                throw new ApplicationException("Must Connect to Firebase to get Ads Client");
            }

#if ENABLE_ADS
            _adsClient = MaxAdsClient.Instance;
#else
            _adsClient = MockAdsClient.Instance;
#endif

            CappingTimer cappingTimer = new CappingTimer();

            _adsClient.SetCappingTimer(cappingTimer);
            
            return _adsClient;
        }
    }
}