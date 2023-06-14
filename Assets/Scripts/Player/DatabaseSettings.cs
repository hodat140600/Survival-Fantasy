using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Assets.Scripts.Skills;

[CreateAssetMenu(menuName = "DatabaseAsset")]
public class DatabaseSettings : SerializedScriptableObject
{
    public int Gold;
    public Dictionary<string, int> AdsViewTime = new Dictionary<string, int>();
    public List<HeroSettings> AvailableHeroes = new List<HeroSettings>();
    public List<HeroSettings> AllHeros = new List<HeroSettings>();
    public Dictionary<string, HeroSettings> AvailableHeroesData = new Dictionary<string, HeroSettings>();
    public Dictionary<string, HeroSettings> AllHeroesData = new Dictionary<string, HeroSettings>();
    public List<ISkillSettings> AvailableSkills= new List<ISkillSettings>();
    public List<ISkillSettings> AllSkills= new List<ISkillSettings>();
    public Dictionary<string, ISkillSettings> AvailableSkillsData = new Dictionary<string, ISkillSettings>();
    public Dictionary<string, ISkillSettings> AllSkillsData = new Dictionary<string, ISkillSettings>();
    public int OnChapter;

    private void OnEnable()
    {
        AdsViewTime.Clear();
        AllSkillsData.Clear();
        AvailableSkillsData.Clear();
        AllHeroesData.Clear();
        AvailableHeroesData.Clear();
        foreach(var item in AvailableSkills)
        {
            AvailableSkillsData.Add(item.GetType().Name, item);
        }
        foreach(var item in AllSkills)
        {
            AllSkillsData.Add(item.GetType().Name, item);
        }        
        foreach(var item in AvailableHeroes)
        {
            AvailableHeroesData.Add(item.heroID.ToString(), item);
        }
        foreach(var item in AllHeros)
        {
            AllHeroesData.Add(item.heroID.ToString(), item);
            AdsViewTime.Add(item.GetHeroIDToString(), 0);
        }
    }
    public void DefaultDatabaseAsset(ref PlayerSettings playerSettings)
    {
        playerSettings.availableSkillsData = AvailableSkillsData;
        playerSettings.AvailableHeroesData = AvailableHeroesData;
        playerSettings.Gold = Gold;
        playerSettings.adsViewTime = AdsViewTime;
        playerSettings.lastWinChapter = OnChapter;
    }
}
