using System;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using UnityEngine;

[System.Serializable]
public class PlayerSettingsData : ISerializationCallbackReceiver
{
    #region SerializeDictionary
    public List<string> _keysSkill = new List<string>();
    public List<ScriptableObject> _valuesSkill = new List<ScriptableObject>();    
    public List<string> _keysHero = new List<string>();
    public List<ScriptableObject> _valuesHero = new List<ScriptableObject>();
    public List<string> _keysIDHero = new List<string>();
    public List<int> _valuesIDHero = new List<int>();

    private DatabaseSettings databaseSettings => Resources.Load<ScriptableObject>("Database Asset") as DatabaseSettings;
    public void OnBeforeSerialize()
    {
        _keysSkill.Clear();
        _valuesSkill.Clear();

        foreach (var kvp in availableSkillsData)
        {
            _keysSkill.Add(kvp.Key);
            _valuesSkill.Add(kvp.Value as ScriptableObject);
        }
        _keysHero.Clear();
        _valuesHero.Clear();

        foreach (var kvp in availableHeroesData)
        {
            _keysHero.Add(kvp.Key);
            _valuesHero.Add(kvp.Value);
        }

        _keysIDHero.Clear();
        _valuesIDHero.Clear();
        foreach(var kvp in TimeViewAds)
        {
            _keysIDHero.Add(kvp.Key);
            _valuesIDHero.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        #region Heroes Data
        availableHeroesData = new Dictionary<string, HeroSettings>();
        for(int i = 0; i != Math.Min(_keysHero.Count, _valuesHero.Count); i++)
        {
            //Debug.Log("Asset ID: " + AvailableHeros[i].GetInstanceID() + " | DB ID : " + databaseSettings.AvailableHeroes[i].GetInstanceID());
            var item = databaseSettings.AllHeroesData[_keysHero[i]];
            if (item.GetInstanceID() != _valuesHero[i].GetInstanceID())
            {
                _valuesHero[i] = item;
            }
            availableHeroesData.Add(_keysHero[i], _valuesHero[i] as HeroSettings);
        }
        if(availableHeroesData.Count < databaseSettings.AvailableHeroes.Count)
        {
            availableHeroesData = databaseSettings.AvailableHeroesData;
        }
        #endregion

        #region Skills Data
        availableSkillsData = new Dictionary<string, ISkillSettings>();
        for (int i = 0; i != Math.Min(_keysSkill.Count, _valuesSkill.Count); i++)
        {
            var item = databaseSettings.AllSkillsData[_keysSkill[i]] as ScriptableObject;
            if (item.GetInstanceID() != _valuesSkill[i].GetInstanceID())
            {
                _valuesSkill[i] = item;
            }
            availableSkillsData.Add(_keysSkill[i], _valuesSkill[i] as ISkillSettings);
        }
        if(availableSkillsData.Count < databaseSettings.AvailableSkillsData.Count)
        {
            availableSkillsData = databaseSettings.AvailableSkillsData;
        }
        #endregion

        #region View Ads Data
        TimeViewAds = new Dictionary<string, int>();
        for (int i = 0; i != Math.Min(_keysIDHero.Count, _valuesIDHero.Count); i++)
        {
            var item = databaseSettings.AdsViewTime[_keysIDHero[i]];
            TimeViewAds.Add(_keysIDHero[i], _valuesIDHero[i]);
        }
        if (TimeViewAds.Count < databaseSettings.AdsViewTime.Count)
        {
            TimeViewAds = databaseSettings.AdsViewTime;
        }
        #endregion
    }
    #endregion

    public int Gold;
    public Dictionary<string, int> TimeViewAds = new Dictionary<string, int>();
    //* Hero Selected ScriptableObject
    public int heroID;
    public Dictionary<string, HeroSettings> availableHeroesData = new Dictionary<string, HeroSettings>();
    public Dictionary<string, ISkillSettings> availableSkillsData = new Dictionary<string, ISkillSettings>();
    public int LastWinChapter;
    public int LiveSkillPercent, DamageSkillPercent, LootSkillPercent;
    public PlayerSettingsData(PlayerSettings playerSettings)
    {
        Gold = playerSettings.Gold;
        TimeViewAds = playerSettings.adsViewTime;
        //* Hero Selected ScriptableObject
        heroID = playerSettings.SelectedHero.heroID;

        availableHeroesData = playerSettings.AvailableHeroesData;
        availableSkillsData = playerSettings.availableSkillsData;

        LastWinChapter = playerSettings.LastWinChapter;

        LiveSkillPercent = playerSettings.BoughtLiveSkillSettings.addedPercent;
        DamageSkillPercent = playerSettings.BoughtDamageSkillSettings.AddedPercent;
        LootSkillPercent = playerSettings.BoughtLootSkillSettings.AddedPercent;
    }
}