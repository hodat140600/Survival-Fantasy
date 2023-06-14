using System;
using System.Collections;
using System.Threading.Tasks;
using Assets._SDK.Ads.AdsClient;
using Assets._SDK.Analytics;
using Assets._SDK.Logger;
using Firebase;
using Firebase.RemoteConfig;
using UnityEngine;

public class FirebaseService : Singleton<FirebaseService>
{
    private bool _fixResult;
    public bool InternetAvailable => Application.internetReachability != NetworkReachability.NotReachable;

    private FirebaseRemoteConfigService _remoteConfigService;
    public bool IsConnected => _fixResult;
    
    protected override void OnAwake()
    {
        StartCoroutine(InitService());
    }

    private IEnumerator InitService()
    {
        var seconds = new WaitForSecondsRealtime(1f);
        
        while (true)
        {
            yield return seconds;
            if (InternetAvailable)
                break;
        }

        yield return CheckAndFixDependenciesAsync();

        if (_fixResult)
        {
            _remoteConfigService = new FirebaseRemoteConfigService();
        }

        Debug.Log($"Firebase initialize : {_fixResult}");

        //AdsClientFactory.GetAdsClient();

    }

    private IEnumerator CheckAndFixDependenciesAsync()
    {
        var check = FirebaseApp.CheckAndFixDependenciesAsync();
        
        while (!check.IsCompleted)
        {
            yield return null;
        }

        _fixResult = !check.IsFaulted && check.Result == DependencyStatus.Available;
    }

    public ConfigValue GetRemoteConfigByKey(string key)
    {
        if (_remoteConfigService == null)
        {
            Debug.Log("Must Init Firebase Remote Config");
        }

        return _remoteConfigService.GetValue(key);
    }
}