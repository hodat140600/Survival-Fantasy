using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Assets.Scripts.Skills;
using Manager.HUD.UIElements;
using Skills.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockPanel : MonoBehaviour
{
    [SerializeField]
    private Sprite _unlockSprite;
    [SerializeField]
    private Sprite _lockSprite;
    [SerializeField]
    private StarContainer _starContainer;

    [SerializeField]
    private TextMeshProUGUI _heroName;

    [SerializeField] 
    private GameObject _skillContainer;
    private const int RANKFIRST = 5;
    private const int RANKSECOND = 10;

    public void SetLock(bool isUnlock)
    {
        var image = gameObject.GetComponent<Image>();
        if (!isUnlock)
        {
            image.sprite = _lockSprite;
            _starContainer.SetLock();
        }
        else
        {
            image.sprite = _unlockSprite;
        }
    }

    
    public void SetPanel(int heroID,int heroLevel,bool isUnlock)
    {
        _heroName.text = HeroLibrary.Instance.GetNameHero(heroID);
        SetLock(isUnlock);
        if (!isUnlock)
        {
            UpdateLock(heroID);
        }
        else
        {
            UpdateSkillContainer(heroID,GameManager.Instance.playerSettings.GetLevelHero(heroID));
            _starContainer.SetStar(heroLevel);
        }
    }
    private void UpdateLock(int heroId)
    {
        var skillLock=SkillLock(heroId);
        var allSkill = HeroLibrary.Instance.GetSkillAll(heroId).ToString();
        for (int i = 0; i < skillLock.Count; i++)
        {
            var skillButton = _skillContainer.transform.GetChild(i).gameObject.GetComponent<SkillButton>();
            skillButton.SetSkill(skillLock[i],true,allSkill==skillLock[i].GetType().Name);
        }
    }

    
    private List<IBaseSettings> SkillLock(int heroId)
    {
        
        //lay skill cua level 1
        var skillRank = HeroLibrary.Instance.GetHeroSkillSettings(heroId, 0);
        
        //lay skill cua level 5
        var skillsRankFirst = HeroLibrary.Instance.GetHeroSkillSettings(heroId, RANKFIRST - 1);
        //lay skill cua level 10
        var skillsRankSecond = HeroLibrary.Instance.GetHeroSkillSettings(heroId, RANKSECOND - 1);
        //lay skill lock cua level 5
        IBaseSettings skillLockFirst = skillsRankFirst.
            Find(skill => !GetNameSkill(skillRank).Contains(skill.GetType().Name));
        //lay skill lock cua level 10
        IBaseSettings skillLockSecond=skillsRankSecond.
            Find(skill => !GetNameSkill(skillsRankFirst).Contains(skill.GetType().Name));
        
        //level skill >=5
        var skillsLock = new List<IBaseSettings>();
        //level skill <5
        skillsLock.Add(skillRank[0]);
        skillsLock.Add(skillLockFirst);
        skillsLock.Add(skillLockSecond);
        return skillsLock;
    }

    


    private List<string> GetNameSkill(List<IBaseSettings> skills)
    {
        List<string> skillsName = new List<string>();
        foreach (var item in skills)
        {
            skillsName.Add(item.GetType().Name);
        }

        return skillsName;
    }

    private List<IBaseSettings> ArrangeSkill(List<IBaseSettings> skills, List<string> nameSkills)
    {
        var skillArrange = new List<IBaseSettings>();
        foreach (var item in nameSkills)
        {
            var skill = skills.Find(skill => skill.GetType().Name == item);
            if (skill != null)
            {
                skillArrange.Add(skill);
            }
        }
        return skillArrange;
    }

    private void RemoveSkillUnlock(ref List<IBaseSettings> skillsLock, List<string> skillsName)
    {
        skillsLock.RemoveAll(skill => skillsName.Contains(skill.GetType().Name));
    }

    public void UpdateSkillContainer(int heroId,int level)
    {
        _starContainer.SetStar(level);
        var heroSettings = HeroLibrary.Instance.GetHeroSkillSettings(heroId,level-1);
        var skillLock = SkillLock(heroId);
        heroSettings = ArrangeSkill(heroSettings, GetNameSkill(skillLock));
        RemoveSkillUnlock(ref skillLock,GetNameSkill(heroSettings));
        /*var skillLock = SkillLock(heroId,level,heroSettings);*/
        var allSkill = HeroLibrary.Instance.GetSkillAll(heroId).ToString();
        for (int i = 0; i < _skillContainer.transform.childCount; i++)
        {
            var skillButton = _skillContainer.transform.GetChild(i).gameObject.GetComponent<SkillButton>();
            if(heroSettings.Any())
            {
                skillButton.SetSkill(heroSettings[0],false,allSkill==heroSettings[0].GetType().Name);
                heroSettings.RemoveAt(0);
            }
            else
            {
                skillButton.SetSkill(skillLock[0],true,allSkill==skillLock[0].GetType().Name);
                skillLock.RemoveAt(0);
            }
            
        }
    }

}