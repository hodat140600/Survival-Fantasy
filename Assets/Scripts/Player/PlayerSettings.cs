using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Hero;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Skills.LiveSkill;
using Sirenix.OdinInspector;
using Skills.Interfaces;
using UniRx;
using UnityEngine;


[CreateAssetMenu(menuName = "PlayStats")]
public class PlayerSettings : SerializedScriptableObject
{
    public int Gold;
    //public Dictionary<int, int> adsWatchedOnHero;
    public Dictionary<string, int> adsViewTime = new Dictionary<string, int>();
    [SerializeField] private HeroSettings selectedHero;
    public Dictionary<string, HeroSettings> AvailableHeroesData = new Dictionary<string, HeroSettings>();
    
    public Dictionary<string,ISkillSettings> availableSkillsData = new Dictionary<string, ISkillSettings>();
    [NonSerialized] public List<ISkillSettings> availableSkills = new List<ISkillSettings>();
    public int lastWinChapter;
    [HideInInspector]public int GoldInChapter;
    [HideInInspector]public int GoldKillEnemiesInChapter => EnemySpawner.Instance.EnemiesKilled / enemiesPerGold;

    [RangeReactiveProperty(1,10000)] public int enemiesPerGold = 10;

    public int LastWinChapter => LevelManager.Instance.CurrentChapter;

    public HeroSettings SelectedHero { get => selectedHero == null ? HeroLibrary.Instance.baseHeroSetting : selectedHero; set => selectedHero = value; }

    [SerializeField]
    private DatabaseSettings databaseSettings => Resources.Load<ScriptableObject>("Database Asset") as DatabaseSettings;

    #region Bought Skill Settings attributes
    private DamageSettings _boughtDamageSkillSettings;
    private LiveSkillSettings _boughtLiveSkillSettings;
    private LootSkillSettings _boughtLootSkillSettings;

    public DamageSettings BoughtDamageSkillSettings
    {
        get
        {
            if (_boughtDamageSkillSettings == null)
            {
                return GetSettings<DamageSettings>(0);
            }

            return _boughtDamageSkillSettings;
        }

        set => _boughtDamageSkillSettings = value;
    }
    public LiveSkillSettings BoughtLiveSkillSettings
    {
        get
        {
            if (_boughtLiveSkillSettings == null)
            {
                return GetSettings<LiveSkillSettings>(0);
            }

            return _boughtLiveSkillSettings;
        }

        set => _boughtLiveSkillSettings = value;
    }
    public LootSkillSettings BoughtLootSkillSettings
    {
        get
        {
            if (_boughtLootSkillSettings == null)
            {
                return GetSettings<LootSkillSettings>(0);
            }

            return _boughtLootSkillSettings;
        }

        set => _boughtLootSkillSettings = value;
    }

    #endregion //Bought Skill Settings

    ///<returns> Type of settings </returns>
    private T GetSettings<T>(int percent) where T : ScriptableObject, ISkillSettings, IPercentSettings
    {
        T settings = ScriptableObject.CreateInstance<T>();
        settings.SetPercent(percent);

        return settings;
    }

    public void BoughtSkill(SkillPrice boughtSkill)
    {
        if (boughtSkill.Skill is DamageSettings)
        {
            BoughtDamageSkillSettings = (DamageSettings)boughtSkill.Skill;
        }

        if (boughtSkill.Skill is LiveSkillSettings)
        {
            _boughtLiveSkillSettings = (LiveSkillSettings)boughtSkill.Skill;
        }

        if (boughtSkill.Skill is LootSkillSettings)
        {
            _boughtLootSkillSettings = (LootSkillSettings)boughtSkill.Skill;
        }
    }

    //private void OnEnable()
    //{
    //    Gold = 0;
    //    if (selectedHero == null)
    //    {
    //        /*HeroLibrary.Instance.SelectedHero(1);*/
    //        //Debug.Log("Selected hero 1");
    //    }
    //    LoadAvailableSkillsData();
    //}

    public void LoadAvailableSkillsData()
    {
        availableSkills.Clear();

        foreach (var item in availableSkillsData)
        {
            availableSkills.Add(/*item.Value as ISkillSettings*/ /*item.GetType().Name,*/ item.Value);
        }
    }

    public void AddGold(int addGold)
    {
        Gold += addGold;
    }
    public void IncreaseGoldInChapter(int addGold)
    {
        GoldInChapter+=addGold;
    }

    public void IncreaseAdsView(int id)
    {
        adsViewTime[id.ToString()]++;
    }

    public HeroSettings GetAvailableHero(int heroID)
    {
        foreach (var heroAvailableSettings in AvailableHeroesData)
        {
            if (heroAvailableSettings.Value.heroID == heroID)
            {
                return heroAvailableSettings.Value;
            }
        }

        return null;
    }

    public bool IsSelectedHero(int id)
    {
        //if (SelectedHero == null) return GameManager.Instance.baseHeroSetting.heroID == id;
        return SelectedHero.heroID == id;
    }


    public void BuyHero(int heroId, int money)
    {
        var hero = HeroLibrary.Instance.GetHero(heroId);
        Gold -= money;
        AvailableHeroesData.Add(hero.heroID.ToString(), hero);
        MessageBroker.Default.Publish(new UpdateGoldEvent { gold = Gold });
    }

    public void LevelUp(int heroId, int money)
    {
        var hero = AvailableHeroesData.FirstOrDefault(hero => hero.Value.heroID == heroId);
        AvailableHeroesData.Remove(hero.Key);
        var nextLvHero = HeroLibrary.Instance.GetHero(heroId, hero.Value.level + 1);
        AvailableHeroesData.Add(nextLvHero.heroID.ToString(), nextLvHero);
        if (heroId == SelectedHero.heroID)
        {
            HeroLibrary.Instance.SelectedHero(heroId);
        }

        Gold -= money;
        MessageBroker.Default.Publish(new UpdateGoldEvent { gold = Gold });
    }

    public int GetLevelHero(int heroId)
    {
        var hero = AvailableHeroesData.FirstOrDefault(hero => hero.Value.heroID == heroId);
        return hero.Value == null ? 0 : hero.Value.level;
    }

    public bool CheckMoney(int money, bool isAds, int heroID)
    {
        return isAds ? money <= adsViewTime[heroID.ToString()] : money <= Gold;
    }


    #region Save Load
    private const string FILE_NAME_PLAYER_SETTINGS = "PlayerSettingsStats";

    public void Init(PlayerSettingsData playerSettingsStats)
    {
        #region set base data
        Gold = playerSettingsStats.Gold;
        AvailableHeroesData = playerSettingsStats.availableHeroesData;
        //Debug.Log("Hero :" + AvailableHeros.ToString());
        availableSkillsData = playerSettingsStats.availableSkillsData;
        BoughtDamageSkillSettings = GetSettings<DamageSettings>(playerSettingsStats.DamageSkillPercent);
        BoughtLiveSkillSettings = GetSettings<LiveSkillSettings>(playerSettingsStats.LiveSkillPercent);
        BoughtLootSkillSettings = GetSettings<LootSkillSettings>(playerSettingsStats.LootSkillPercent);

        //Hero Selected ScriptableObject
        //Debug.Log("I got data hero settings : " + SelectedHero.ToString());
        lastWinChapter = playerSettingsStats.LastWinChapter;
        LevelManager.Instance.CurrentChapter = lastWinChapter;
        HeroLibrary.Instance.SelectedHero(playerSettingsStats.heroID);
        #endregion //set base data

        // add data to list for any class get data
        adsViewTime = playerSettingsStats.TimeViewAds;
        LoadAvailableSkillsData();
        GameManager.Instance.LoadModelPlayer();
    }

    ///<summary> Call save data to json file </summary> 
    public void SaveSettings()
    {
        var saveLoad_Json = new SaveLoad_Json<PlayerSettingsData>();
        saveLoad_Json.data = new PlayerSettingsData(this);

        saveLoad_Json.SaveData(FILE_NAME_PLAYER_SETTINGS);
        //Debug.Log("Saved!");
    }

    ///<summary> Call read file and load to data if file json exists </summary> 
    public void LoadSettings()
    {
        //Debug.Log("Load data");
        var saveLoad_Json = new SaveLoad_Json<PlayerSettingsData>();

        if (saveLoad_Json.LoadData(FILE_NAME_PLAYER_SETTINGS))
        {
            //Debug.Log("Load data from Json");
            Init(saveLoad_Json.data);

            return;
        }
        //Debug.Log("Load Hero");
        //* Don't load from Json so get default selected hero
        var thisScript = this;
        databaseSettings.DefaultDatabaseAsset(ref thisScript);
        LoadAvailableSkillsData();
        HeroLibrary.Instance.SelectedHero(6);
        GameManager.Instance.LoadModelPlayer();
    }
    #endregion //Save Load

    public void AddSkill(ISkillSettings skill)
    {
        if (availableSkillsData.ContainsKey(/*skill as ScriptableObject*/skill.GetType().Name))
        {
            return;
        }
        availableSkills.Add(skill);
        availableSkillsData.Add(/*skill as ScriptableObject*/ skill.GetType().Name, skill);
    }
    public void ResetGoldInChapter()
    {
        GoldInChapter = 0;
    }

    public void RewardAdsMoney()
    {
        AddGold(GoldInChapter*3);
        MessageBroker.Default.Publish(new UpdateGoldEvent { gold = Gold });
    }
}
