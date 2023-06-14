using System.Collections;
using UnityEngine;

namespace Assets._SDK.Ads.AdsClient
{
    public interface IAppOpenAdManager
    {

        public void LoadAd();
        public void ShowAdIfAvailable();

    }
}