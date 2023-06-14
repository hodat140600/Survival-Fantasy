using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeroShopItem
{
    public int heroID;
    public string heroName;
    public int level;

    public bool isUnlock;

    public void Init(HeroSettings heroSettings, string heroName, bool isUnlock)
    {
        heroID = heroSettings.heroID;
        level = heroSettings.level;
        this.heroName = heroName;
        this.isUnlock = isUnlock;
    }
}
