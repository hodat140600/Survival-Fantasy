using Assets._SDK.Logger;
using UnityEngine;

namespace RocketSg.Sdk.AdsClient
{
    public class MaxBanner
    {
        private readonly string _bannerAdUnitId;
        private readonly MaxSdkBase.BannerPosition _position;

        public MaxBanner(string bannerAdUnitId, MaxSdkBase.BannerPosition position)
        {
            _bannerAdUnitId = bannerAdUnitId;
            _position = position;

            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;

            // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
            // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
            MaxSdk.CreateBanner(_bannerAdUnitId, _position);
            MaxSdk.SetBannerExtraParameter(_bannerAdUnitId, "adaptive_banner", "true");

            // Set background or background color for banners to be fully functional.
            MaxSdk.SetBannerBackgroundColor(_bannerAdUnitId, Color.clear);
        }

        public void ToggleBannerVisibility(bool isActive)
        {
            if (isActive)
            {
                MaxSdk.ShowBanner(_bannerAdUnitId);
            }
            else
            {
                MaxSdk.HideBanner(_bannerAdUnitId);
            }
        }

        private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            // Banner ad is ready to be shown.
            // If you have already called MaxSdk.ShowBanner(BannerAdUnitId) it will automatically be shown on the next ad refresh.
            Debug.Log("Banner ad loaded");
        }

        private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
        {
            // Banner ad failed to load. MAX will automatically try loading a new ad internally.
            Debug.Log("Banner ad failed to load with error code: " + errorInfo.Code);
        }

        private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Banner ad clicked");
        }

    }
}