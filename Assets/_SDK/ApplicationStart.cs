using UnityEngine;
using Object = UnityEngine.Object;
public static class ApplicationStart
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnStartBeforeSceneLoad()
    {
        RegisterServices();
        //Debug.unityLogger.logEnabled = false;
        Application.lowMemory += OnLowMemory;
    }

        

    private static void OnLowMemory()
    {
        Resources.UnloadUnusedAssets();
    }

    private static void RegisterServices()
    {
        RegisterFirebase();
        RegisterAppFlyer();
    }

    private static void RegisterAppFlyer()
    {
        var appFlyer = RegisterMonoService<AppsFlyerObjectScript>();
        // appFlyer.appID = AdsConfig.AppFlyerAppId;
        appFlyer.devKey = AdsConfig.AppFlyerDevKey;
        appFlyer.getConversionData = true;

    }

    private static void RegisterFirebase()
    {
        var firebaseService = FirebaseService.Instance;
    }

    private static T RegisterMonoService<T>() where T : Component
    {
        var fullName = typeof(T).FullName;
        var obj = new GameObject();
        if (!string.IsNullOrEmpty(fullName))
            obj.name = fullName;
        Object.DontDestroyOnLoad(obj);
        var instance = obj.AddComponent<T>();
        return instance;
    }

    private static T RegisterPrefabService<T>(string path) where T : Component
    {
        var res = Resources.Load<T>(path);
        var obj = Object.Instantiate(res);
        return obj;
    }
}