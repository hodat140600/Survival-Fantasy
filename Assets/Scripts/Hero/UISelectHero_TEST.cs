using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using UniRx;
using UnityEngine;
using Sirenix.OdinInspector;
using Assets.Scripts.Skills.LiveSkill;

public class UISelectHero_TEST : SerializedMonoBehaviour
{
    [Header("Test get skills")]
    public List<IBaseSettings> listIBaseSettings = new List<IBaseSettings>();
    public int heroIDBaseSetting;
    [Range(1, 10)] public int levelHeroBaseSetting;
    [ContextMenu("Test get skills")]
    public void GetBaseSkillSettings()
    {

        // listIBaseSettings.Add(new LiveSkillSettings());
        listIBaseSettings = HeroLibrary.Instance.GetHeroSkillSettings(heroIDBaseSetting, levelHeroBaseSetting - 1);

        //Debug.Log("I got " + listIBaseSettings.ToString());
    }




    [Space(10)]
    [Header("Test get selected hero")]
    public int heroID;

    [ContextMenu("Test get selected hero")]
    public void UI_Selected_Hero()
    {
        HeroLibrary.Instance.SelectedHero(heroID);
    }





    [Space(10)]
    [Header("Test get list hero")]
    public List<HeroShopItem> listUIHeroData;
    [ContextMenu("Test get list hero")]
    public void UI_List_Hero()
    {
        listUIHeroData = HeroLibrary.Instance.GetListHero();
    }

}
