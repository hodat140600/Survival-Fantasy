using System.Collections;
using UnityEngine;

namespace Assets._SDK.Ads.AdsClient.Google
{
    public class MockAppOpenAdManager : SingletonClass<MockAppOpenAdManager>, IAppOpenAdManager
    {
         public void LoadAd()
        {
            Debug.Log("MockAppOpenAdManager LoadAd");
        }

        public void ShowAdIfAvailable()
        {
            Debug.Log("MockAppOpenAdManager ShowAdIfAvailable");
        }
    }
}