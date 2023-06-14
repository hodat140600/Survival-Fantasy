using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using RDG;
using Assets.Scripts.Skills;
using Assets.Scripts.Events;
using UnityEngine.Audio;
using System.Diagnostics;
using Assets._SDK.Ads.AdsClient;
using Assets._SDK.Analytics;
using RocketSg.Sdk.AdsClient;

public enum GameState
{
    SelectHero, // TODO
    SelectToPlay,
    SelectSkill,
    Playing,
    Pause,
    Revive,
    Victory,
    Lose
}
public class GameManager : Singleton<GameManager>
{
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;
    public static event Action OnVictory;
    public VariableJoystick joystick;
    [SerializeField] List<GameObject> _skills;
    //[SerializeField] GameObject _spawn;
    //[SerializeField] GameObject _holder;

    [SerializeField] GameObject _skillHolder;
    [SerializeField] Animator _aniamtorPlayer;
    [SerializeField] private GameObject _modelPlayer;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private SoundSource _sourceSound;
    public GameObject player;
    public Transform playerTransform;
    public PlayerSettings playerSettings;
    //public HeroSettings baseHeroSetting;
    public SkillPriceSettings skillPriceSettings;

    private bool isLoadedSkillSystem = false;

    public SkillSystem skillSystem;
    MapInfo _mapInfo;

    Vector3 _offsetPlayer;
    Quaternion _rotationPlayer;
    public bool isVibration;
    public bool isSoundOn;
    int _veclocityID = Animator.StringToHash("Velocity");
    int _idleID = Animator.StringToHash("IsIdle");

    //public IAdsClient adsClient;
    private void FixedUpdate()
    {
        _skillHolder.transform.position = playerTransform.position;
    }

    //IEnumerator MicroCoroutineFollowPlayer()
    //{
    //    while (true)
    //    {
    //        _skillHolder.transform.position = player.transform.position;
    //    }
    //}

    private void Awake()
    {
        StartCoroutine(GetAdsClient());
        playerTransform = player.transform;
        isVibration = true;
        isSoundOn = true;
        Application.targetFrameRate = 60;
        //_holder = GameObject.FindGameObjectWithTag("Holder");
        //_spawn = _holder.transform.Find("SpawnManager").gameObject;
        skillSystem = player.GetComponent<SkillSystem>();
        _mapInfo = LevelManager.Instance.road.GetComponent<MapInfo>();
        _offsetPlayer = playerTransform.position;
        _rotationPlayer = playerTransform.rotation;
        _aniamtorPlayer = _modelPlayer.GetComponentInChildren<Animator>();
        //StopAllCoroutines();
        //MainThreadDispatcher.StartFixedUpdateMicroCoroutine(MicroCoroutineFollowPlayer());
    }
    IEnumerator GetAdsClient()
    {
        yield return new WaitUntil(() => FirebaseService.Instance.IsConnected);
        //adsClient = AdsClientFactory.GetAdsClient();
    }

    void Start()
    {
        LoadSettings();
        UpdateGameState(GameState.SelectToPlay);
		AdsClientManager.Instance.AdsClient.ShowBanner(true);
	}

    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.SelectToPlay:
                HandleSelectToPlay();
                break;
            case GameState.SelectSkill:
                HandleSelectSkill();
                break;
            case GameState.Playing:
                HandlePlaying();
                break;
            case GameState.Pause:
                HandlePause();
                break;
            case GameState.Revive:
                HanldeRevive();
                break;
            case GameState.Victory:
                HandleVictory();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(newState);
    }
    bool isPaused = false;
    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
    }
    private void LoadSkillSystem()
    {
        //SkillSystem skillSystem = player.GetComponent<SkillSystem>();
        //Hero selectedHero = playerSettings?.SelectedHero == null ? defaultHero : playerSettings?.SelectedHero;
        skillSystem.InitSkillToStartChapter();
    }
    #region GameState
    private void HandleSelectToPlay()
    {
        //TimeManager.Instance.ResetTimerCount();
        if (PlayerPrefs.GetInt("IsFirst") == 0)
        {

        }

        playerTransform.position = _offsetPlayer;
        ResetPositionPlayer();
        Time.timeScale = 1;
        SendStartChapEvent();
        isLoadedSkillSystem = false;
        UIManager.Instance.OnChapterStart();
        skillSystem.selectedSkillSettings = new Dictionary<string, ISkillSettings>();
        //LevelManager.Instance.LoadLevel();
        LevelManager.Instance.generateTileMap.StartToGenerateMap();
        LoadMapChapter();
        EnemySpawner.Instance.ClearListEnemyAndStopSpawnAndResetEnemiesKilled();
        ExpSpawner.Instance.ReturnAllPool();
        ProjectilesSpawner.Instance.DisableAll();
        HitImpactSpawner.Instance.DisableAll();
        //_spawn.SetActive(false);
        RewardSpawner.Instance.ReturnAllPool();
        TimeManager.Instance.Stop();
        playerSettings.ResetGoldInChapter();
        _sourceSound.StopAmbientSound();
        SaveSettings();
        #region Ads & Firebase
        AnalyticsService.LogEventLevelStart(LevelManager.Instance.CurrentChapter);
        #endregion
    }
    private void HandleSelectSkill()
    {
        LevelManager.Instance.GetObjectPoolerCurrentChapter.gameObject.SetActive(true);
        MessageBroker.Default.Publish(new SelectedSkillEvent());
        LevelManager.Instance.AddAllEnemy();
        TogglePausing(true);
    }
    private void HandlePlaying()
    {
        TogglePausing(false);
        MessageBroker.Default.Publish(new PlayingEvent());
        if (!isLoadedSkillSystem)
        {
			AdsClientManager.Instance.AdsClient.ShowBanner(true);
            
            LoadSkillSystem();
            isLoadedSkillSystem = true;
            //_spawn.SetActive(true);
            EnemySpawner.Instance.StartToSpawn();
            TimeManager.Instance.Reset();
            TimeManager.Instance.Begin();
            //TimeManager.Instance.StartCounter();
            _sourceSound.SetAndPlayAmbientSound();
        }
    }
    private void HandlePause()
    {
        TogglePausing(true);
        UIManager.Instance.ActivePausePanel(true);
    }
    private void HanldeRevive()
    {
        TogglePausing(true);
        MessageBroker.Default.Publish(new ReviveEvent());
    }
    private void HandleLose()
    {
        //TogglePausing(true);
        //MessageBroker.Default.Publish(new LoseEvent());
        playerSettings.IncreaseGoldInChapter(playerSettings.GoldKillEnemiesInChapter);
        UIManager.Instance.OnChapterLose();
        UnActiveSkill();
        AnalyticsService.LogEventLevelLose(LevelManager.Instance.CurrentChapter);
        /*Time.timeScale = 0;*/
    }
    private void HandleVictory()
    {
        //Debug.Log("Victory");
        playerSettings.IncreaseGoldInChapter(playerSettings.GoldKillEnemiesInChapter);
        TogglePausing(true);
        UIManager.Instance.OnChapterVictory();
        UnActiveSkill();
        AnalyticsService.LogEventLevelWin(LevelManager.Instance.CurrentChapter);
    }
    #endregion

    void ResetPositionPlayer()
    {
        playerTransform.rotation = _rotationPlayer;
        _aniamtorPlayer.SetFloat(_veclocityID, 0);
        _aniamtorPlayer.SetBool(_idleID, true);
    }


    void UnActiveSkill()
    {
        foreach (var item in _skills)
        {
            item.SetActive(false);
        }
    }

    void SendStartChapEvent()
    {
        int countWave = LevelManager.Instance.GetWaveCount;
        int currentChap = LevelManager.Instance.CurrentChapter;
        MessageBroker.Default.Publish(new ChapterStartEvent(currentChap, countWave));
    }
    public List<GameObject> Maps;
    public void LoadMapChapter()
    {
        _mapInfo.ChangeMaterialMap(LevelManager.Instance.MapCurrentChap.materialRoad);
    }

    ///<summary> Call this if you want pause game and time counter (stopwatch for reward system, etc) </summary>
    public void TogglePausing(bool isPause)
    {
        if (isPause)
        {
            StopAllCoroutines();
            Time.timeScale = 0;
            //TimeManager.Instance.PauseTimerCount();
            
            ResetPositionPlayer();
        }
        else
        {
            StartCoroutine(TapToContinues());
        }
    }

    ///<summary> wait for user's input action(joy stick, tap) to continues game </summary>
    IEnumerator TapToContinues()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.01f);
            //Debug.Log("Wait for key down");

            if (joystick.Horizontal != 0 || joystick.Vertical != 0 || Input.anyKey)
            {
                break;
            }
        }

        Time.timeScale = 1;
        //TimeManager.Instance.ResumeTimerCount();
    }
    public void LoadModelPlayer()
    {
        if (_modelPlayer != null)
        {
            var modelPosition = _modelPlayer.transform.position;
            Destroy(_modelPlayer);

        }
        else
        {
            Destroy(playerTransform.GetChild(0));
        }
        var selectedHero = GameManager.Instance.playerSettings.SelectedHero;
        //if (selectedHero == null)
        //{
        //    selectedHero = GameManager.Instance.baseHeroSetting;
        //}
        _modelPlayer =
            Instantiate(selectedHero.heroModelPrefab, Vector3.zero, playerTransform.rotation, playerTransform);
        _aniamtorPlayer = _modelPlayer.GetComponentInChildren<Animator>();
    }

    ///<summary> Put all script that have SaveSettings() or like it in here </summary>
    public void SaveSettings()
    {
        playerSettings.SaveSettings();
    }

    ///<summary> Put all script that have LoadSettings or like it in here </summary>
    public void LoadSettings()
    {
        //Application.targetFrameRate = 60;
        playerSettings.LoadSettings();
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveSettings();
            isPaused = pauseStatus;
            if (State == GameState.Playing) UpdateGameState(GameState.Pause);
        }
    }

    private void OnDestroy()
    {
        SaveSettings();
    }
    public void ChangeVibration()
    {
        isVibration = !isVibration;
    }

    public void Vibrate()
    {
        if (isVibration)
        {
            Vibration.Vibrate(50);
        }
    }
    public void ConfigSound()
    {
        isSoundOn = !isSoundOn;
        SoundSwicth();
    }

    public void SoundSwicth()
    {
        float volume = isSoundOn ? 0 : -80;
        //Debug.Log("Volume" + volume);
        audioMixer.SetFloat("Volume", volume);
    }
}
