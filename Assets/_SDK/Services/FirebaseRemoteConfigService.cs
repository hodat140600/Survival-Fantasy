using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;

public class FirebaseRemoteConfigService
{

    public ConfigValue GetValue(string key)
    {
        return FirebaseRemoteConfig.DefaultInstance.GetValue(key);
    }

    public FirebaseRemoteConfigService()
    {

        Dictionary<string, object> defaults = new Dictionary<string, object>()
        {
            { AdsConfig.CappingTimeRemoteConfigKey, AdsConfig.CappingTimeDefaultValue }
        };

        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);

        FetchDataAsync();
    }

    #region Fetch
    private Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        Debug.Log
        (
            fetchTask.IsCanceled  ? "Fetch canceled." :
            fetchTask.IsFaulted   ? "Fetch encountered an error." :
            fetchTask.IsCompleted ? "Fetch completed successfully!" :
                                    "Fetch status unknown."
        );

        var info = FirebaseRemoteConfig.DefaultInstance.Info;

        if (info.LastFetchStatus == LastFetchStatus.Failure)
        {
            Debug.Log
            (
                info.LastFetchFailureReason switch
                {
                    FetchFailureReason.Error     => "Fetch failed for unknown reason",
                    FetchFailureReason.Throttled => "Fetch throttled until " + info.ThrottledEndTime,
                    FetchFailureReason.Invalid   => "Fetch invalid",
                    _                            => "Fetch failure reason unknown"
                });

            return;
        }

        if (info.LastFetchStatus == LastFetchStatus.Pending)
        {
            Debug.Log("Latest Fetch call still pending.");

            return;
        }


        FirebaseRemoteConfig.DefaultInstance.ActivateAsync();

        Debug.Log(string.Format("Remote data loaded and ready (last fetch time {0}).", info.FetchTime));
    }
    #endregion
}