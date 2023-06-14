using Assets._SDK.Ads;
using System;
using System.Collections;

namespace RocketSg.Sdk.AdsClient
{
    public interface IAdsClient
    {
        bool IsRewardedVideoReady { get; }
        bool IsInterstitialLoaded { get; }
        void OnApplicationPause(bool isPause);


        /// <summary>
        /// show unskippable rewarded video (30sec)
        /// </summary>
        /// <param name="placementName"></param>
        /// <param name="onRewarded"></param>
        void ShowRewardedVideo(int levelIndex, string placementName, Action<ShowResult> onRewarded = null);

        /// <summary>
        /// show skippable video after 5 sec
        /// </summary>
        /// <param name="placementName"></param>
        void ShowInterstitial(int levelIndex, string placementName);

        /// <summary>
        /// Show banner
        /// </summary>
        IEnumerator ShowBanner();

        void SetCappingTimer(CappingTimer cappingTimer);
    }
}