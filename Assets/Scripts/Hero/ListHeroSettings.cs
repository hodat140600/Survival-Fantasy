using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Hero;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.LiveSkill;
using UnityEngine;

///<summary> The main skill that hero apply to all hero </summary>
[System.Serializable]
public enum BuffAllSkillHero
{
    LiveSkillSettings,
    DamageSettings,
    MoveSkillSettings,
    MagnetSkillSettings,
    ArmorSkillSettings,
    RadiusSettings,
    LootSkillSettings,
    CooldownSettings,
    RegenerationSkillSettings,
    ExperienceSkillSettings
}


///<summary> Hero have 10 level settings </summary>
[System.Serializable]
public class ListHeroSettings //TODO change name to Hero
{
    public int heroID;
    public string heroName;
    public BuffAllSkillHero buffAllSkillHero;
    public HeroSettings[] heroLevelSettings;
    private Dictionary<string, IBaseSettings> _dictionarySkill;
    public void Init()
    {
        _dictionarySkill = new Dictionary<string, IBaseSettings>();

        for (int i = 0; i < heroLevelSettings.Length; i++)
        {
            AddBaseSkillSettings<LiveSkillSettings>(i, heroLevelSettings[i].live);
            AddBaseSkillSettings<DamageSettings>(i, heroLevelSettings[i].damage);
            AddBaseSkillSettings<MoveSkillSettings>(i, heroLevelSettings[i].move);
            AddBaseSkillSettings<MagnetSkillSettings>(i, heroLevelSettings[i].magnet);
            AddBaseSkillSettings<ArmorSkillSettings>(i, heroLevelSettings[i].armor);
            AddBaseSkillSettings<RadiusSettings>(i, heroLevelSettings[i].radius);
            AddBaseSkillSettings<LootSkillSettings>(i, heroLevelSettings[i].loot);
            AddBaseSkillSettings<CooldownSettings>(i, heroLevelSettings[i].coolDown);
            AddBaseSkillSettings<RegenerationSkillSettings>(i, heroLevelSettings[i].regeneration);
            AddBaseSkillSettings<ExperienceSkillSettings>(i, heroLevelSettings[i].exp);
        }
    }


    ///<summary> The skill will have Key = (T - BaseSkill name) + (id position)  </summary>
    private void AddBaseSkillSettings<T>(int idPosition, IBaseSettings baseSettings)
    {
        // Check input, if the skill don't have setting, return
        if (baseSettings == null)
        {
            return;
        }

        _dictionarySkill.Add(typeof(T).Name + idPosition, baseSettings);
    }

    public IBaseSettings GetMainSettings(int idPosition)
    {
        string skillKey = buffAllSkillHero.ToString() + (idPosition - 1);
        //* Check in data
        if (_dictionarySkill.ContainsKey(skillKey) == false)
        {
            return null;
        }

        return _dictionarySkill[skillKey];
    }
}
