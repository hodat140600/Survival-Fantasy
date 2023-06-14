using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.LiveSkill;
using UnityEngine;
using Assets.Scripts.Hero;
using UniRx;

///<summary> The library container data and control hero settings </summary>
public class HeroLibrary : Singleton<HeroLibrary>
{
    private const int NUMBER_SKILL_SETTINGS = 3;

    ///<summary> The array control one hero have 10 level settings </summary>
    [SerializeField] private ListHeroSettings[] _listHero;
    public HeroSettings baseHeroSetting;

    ///<summary> Change Selected Hero in PlayerSettings.cs </summary>
    public void SelectedHero(int heroID)
    {
        if (GameManager.Instance.playerSettings == null) GameManager.Instance.playerSettings = Resources.Load<ScriptableObject>("Player Settings") as PlayerSettings;
        int level = GameManager.Instance.playerSettings.GetAvailableHero(heroID).level - 1;
        // Debug.Log("Please select hero " + heroName.ToString() + " at level " + (level + 1));

        var selectedHeroSettings = ScriptableObject.CreateInstance<HeroSettings>();
        selectedHeroSettings.Init();

        foreach (ListHeroSettings heroSettings in _listHero)
        {
            //* Only choice right hero
            if (heroSettings.heroID == heroID)
            {
                selectedHeroSettings.heroID = heroID;
                selectedHeroSettings.level = level + 1;
                selectedHeroSettings.heroModelPrefab = heroSettings.heroLevelSettings[level].heroModelPrefab;
                selectedHeroSettings.moneyCost = heroSettings.heroLevelSettings[level].moneyCost;
                selectedHeroSettings.SumHeroSettings(heroSettings.heroLevelSettings[level]);
                break;
            }
        }

        //* add all hero setting
        foreach (var heroSettings in GameManager.Instance.playerSettings.AvailableHeroesData)
        {
            if (heroSettings.Value.heroID != heroID)
            {
                // Debug.Log("Hero other " + heroSettings.heroID + " settings " + heroSettings.ToString());
                selectedHeroSettings.SumHeroSettings(GetMainHeroSettingsByLevel(heroSettings.Value.heroID, heroSettings.Value.level));
            }
        }

        //Debug.Log("I create hero settings to PlayerSettings : " + selectedHeroSettings.ToString());


        selectedHeroSettings.SumHeroSettings(baseHeroSetting);
        GameManager.Instance.playerSettings.SelectedHero = selectedHeroSettings;
    }


    ///<summary> Calculator get main skill from all hero settings  </summary>
    ///<returns> The hero settings applied from all hero </returns>
    public HeroSettings GetMainHeroSettingsByLevel(int heroID, int levelHero)
    {
        var heroSettings = ScriptableObject.CreateInstance<HeroSettings>();
        heroSettings.Init();

        for (int i = 0; i < _listHero.Length; i++)
        {
            // ignore selected hero
            if (_listHero[i].heroID == heroID)
            {
                //* get the main skill and add to local variable
                _listHero[i].Init();
                IBaseSettings baseSetting = _listHero[i].GetMainSettings(levelHero);

                heroSettings.AddedSettings(baseSetting);

                break;
            }
        }
        return heroSettings;
    }

    /*
        Get data from PlayerSettings and load to list 
    */
    ///<summary> Use this to get all hero settings for UI </summary>
    ///<returns> List hero: id, name, level  lock and unlock </returns>
    public List<HeroShopItem> GetListHero()
    {
        var listHero = new List<HeroShopItem>();

        for (int i = 0; i < _listHero.Length; i++)
        {
            var availableHero = GameManager.Instance.playerSettings.GetAvailableHero(_listHero[i].heroID);
            var UIHeroData = new HeroShopItem();

            bool isUnlock = availableHero != null;

            availableHero = isUnlock ? availableHero : _listHero[i].heroLevelSettings[0]; // 0 is get the fist level if is locked

            UIHeroData.Init(availableHero, _listHero[i].heroName, isUnlock);

            listHero.Add(UIHeroData);
        }

        return listHero;
    }


    // TODO: 1. GetCurrentHeroSettings(heroId) --> return current level hero setting 
    // TODO: 2. GetHeroSettingsByLevel(heroId, levelNumber) --> return level hero setting by levelNumber 
    // TODO: 3. GetSettingsForAll(heroId) --> return Damage 
    // heroSkills cua 10 level - settings

    //* 2.
    public List<IBaseSettings> GetHeroSkillSettings(int heroID, int level)
    {
        List<IBaseSettings> listSkillSettings = new List<IBaseSettings>();

        //* find hero and get skills data
        for (int i = 0; i < _listHero.Length; i++)
        {
            if (_listHero[i].heroID == heroID)
            {
                // Debug.Log("Hero skills : " + _listHero[i].heroLevelSettings[level].ToString());
                var heroSettings_abc = _listHero[i].heroLevelSettings[level].GetBaseSettingsAvailable();

                foreach (IBaseSettings baseSettings in heroSettings_abc)
                {
                    // Debug.Log("I have skill : " + baseSettings.Description);
                    listSkillSettings.Add(baseSettings);
                }
            }
        }

        return listSkillSettings;
    }

    public HeroSettings GetHero(int heroId)
    {
        return GetHero(heroId, 1);
    }
    public HeroSettings GetHero(int heroId, int heroLevel)
    {
        return _listHero.ToList().Find(hero => heroId == hero.heroID).heroLevelSettings.ToList().
            Find(heroLevels => heroLevels.level == heroLevel);
    }

    public string GetNameHero(int heroId)
    {
        return _listHero.ToList().Find(hero => hero.heroID == heroId).heroName;
    }

    public BuffAllSkillHero GetSkillAll(int heroId)
    {
        return _listHero.ToList().Find(hero => hero.heroID == heroId).buffAllSkillHero;
    }

}
