using System;

public class AdsConfig
{
    // AppOpenAdd
#if UNITY_ANDROID
        public const string ID_TIER_1 = "a97b3e8335a872aa";
        public const string ID_TIER_2 = "TIER_2_HERE";
        public const string ID_TIER_3 = "TIER_3_HERE";

#elif UNITY_IOS
        public const string ID_TIER_1 = "";
        public const string ID_TIER_2 = "";
        public const string ID_TIER_3 = "";
#else
        public const string ID_TIER_1 = "ca-app-pub-3940256099942544/3419835294";
        public const string ID_TIER_2 = "";
        public const string ID_TIER_3 = "";
#endif

    //AppFlyer
    public const string AppFlyerDevKey = "Mza5CYwx7pzKhdhcFcTHdm";

    // public const string AppFlyerAppId = ""; // Only for iOS

    //Max
    public const string MaxSdkKey = "7PspscCcbGd6ohttmPcZTwGmZCihCW-Jwr7nSJN2a_9Mg0ERPs0tmGdKTK1gs__nr6XHQvK0vTNaTb1uR1mCIN";
    public const string InterstitialAdUnitId = "c868ad943d7ea6f8";
    public const string RewardedAdUnitId = "f36c9541925d014e";
    public const string BannerAdUnitId = "ebfad65095573d33";
	public const string CappingTimeRemoteConfigKey = "config_survival_heroes_capping_time";
	public const float CappingTimeDefaultValue = 20;

	public const float CONST_TIME_WAIT_FOR_SHOW_FIRST_AOA = 7f;
	public const float CONST_MAX_TIME_WAIT_FOR_SHOW_FIRST_AOA = 12f;

	public static TypeAdsMax TypeAdsUse = TypeAdsMax.AOA| TypeAdsMax.Banner| TypeAdsMax.RewardVideo| TypeAdsMax.Interstitial;
}

[Flags]
public enum TypeAdsMax
{
	AOA = 0,
	Banner = 1,
	RewardVideo = 2,
	Interstitial = 3,
	Other = 4,
}