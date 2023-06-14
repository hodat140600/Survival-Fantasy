using System.Collections.Generic;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Skills.LiveSkill;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(menuName = "Hero/CreateHero", fileName = "Hero")]
[ShowOdinSerializedPropertiesInInspector]
public class HeroSettings : SerializedScriptableObject
{
    public enum CurrencyType
    {
        Gold=0,
        Ads=1
    }

    public LiveSkillSettings live;

    public DamageSettings damage;
    public MoveSkillSettings move;
    public MagnetSkillSettings magnet;
    public ArmorSkillSettings armor;
    public RadiusSettings radius;
    public LootSkillSettings loot;
    public CooldownSettings coolDown;
    public RegenerationSkillSettings regeneration;
    public ExperienceSkillSettings exp;

    public int heroID;
    public int level;
    public CurrencyType currency;
    public int adsCost;
    public int moneyCost;
    public GameObject heroModelPrefab;

    public void Init()
    {
        //Debug.Log("Khoi tao settings");

        live = ScriptableObject.CreateInstance<LiveSkillSettings>();
        damage = ScriptableObject.CreateInstance<DamageSettings>();
        move = ScriptableObject.CreateInstance<MoveSkillSettings>();
        magnet = ScriptableObject.CreateInstance<MagnetSkillSettings>();

        magnet.rewardRadius = new Radius();
        magnet.experienceRadius = new Radius();

        armor = ScriptableObject.CreateInstance<ArmorSkillSettings>();

        radius = ScriptableObject.CreateInstance<RadiusSettings>();

        loot = ScriptableObject.CreateInstance<LootSkillSettings>();

        coolDown = ScriptableObject.CreateInstance<CooldownSettings>();

        regeneration = ScriptableObject.CreateInstance<RegenerationSkillSettings>();
        regeneration.health = new Health();
        regeneration.cooldown = new Cooldown();

        exp = ScriptableObject.CreateInstance<ExperienceSkillSettings>();
    }

    #region TEST GAME

    public override string ToString()
    {
        string s = "";

        if (live != null) s += "Live : " + live.addedPercent + " - " + live.addedPoint + ", ";
        if (damage != null) s += "Damage : " + damage.AddedPercent + ", ";
        if (move != null) s += "Move : " + move.addedPercent + " - " + move.addedPoint + ", ";
        if (magnet != null) s += "Magnet : " + magnet.experienceRadius.BasePoint + " - " + magnet.experienceRadius.Percent + ", ";
        if (armor != null) s += "Armor : " + armor.addedPercent;
        if (radius != null) s += "Radius : " + radius.AddedPercent + ", ";
        if (loot != null) s += "Loot : " + loot.AddedPercent + ", ";
        if (coolDown != null) s += "CoolDown : " + radius.AddedPercent + ", ";
        if (exp != null) s += "Exp : " + exp.addedPercent + ", ";
        if (regeneration != null) s += "Regeneration : " + regeneration.health.BasePoint + " - " + regeneration.health.Percent;

        return s;
    }
    #endregion

    public string GetHeroIDToString()
    {
        return heroID.ToString();
    }

    public void SumHeroSettings(HeroSettings heroSettings)
    {
        CheckNullAndUpdateSkill(live, heroSettings.live);
        CheckNullAndUpdateSkill(damage, heroSettings.damage);
        CheckNullAndUpdateSkill(move, heroSettings.move);
        CheckNullAndUpdateSkill(magnet, heroSettings.magnet);
        CheckNullAndUpdateSkill(armor, heroSettings.armor);
        CheckNullAndUpdateSkill(radius, heroSettings.radius);
        CheckNullAndUpdateSkill(loot, heroSettings.loot);
        CheckNullAndUpdateSkill(coolDown, heroSettings.coolDown);
        CheckNullAndUpdateSkill(regeneration, heroSettings.regeneration);
        CheckNullAndUpdateSkill(exp, heroSettings.exp);
    }

    private void CheckNullAndUpdateSkill(IBaseSettings skill, IBaseSettings heroBaseSkillSettings)
    {
        if (heroBaseSkillSettings != null)
        {
            skill.UpdateSkill(heroBaseSkillSettings);
        }
    }

    public List<IBaseSettings> GetBaseSettingsAvailable()
    {
        var listBaseSettings = new List<IBaseSettings>();

        AddBaseSettings(live, ref listBaseSettings);
        AddBaseSettings(damage, ref listBaseSettings);
        AddBaseSettings(move, ref listBaseSettings);
        AddBaseSettings(magnet, ref listBaseSettings);
        AddBaseSettings(armor, ref listBaseSettings);
        AddBaseSettings(radius, ref listBaseSettings);
        AddBaseSettings(loot, ref listBaseSettings);
        AddBaseSettings(coolDown, ref listBaseSettings);
        AddBaseSettings(regeneration, ref listBaseSettings);
        AddBaseSettings(exp, ref listBaseSettings);

        return listBaseSettings;
    }

    private void AddBaseSettings(IBaseSettings settings, ref List<IBaseSettings> listBaseSettings)
    {
        if (settings != null)
        {
            // Debug.Log("ADD " + settings.Description);
            listBaseSettings.Add(settings);
        }
    }

    public void AddedSettings(IBaseSettings skill)
    {
        switch (skill)
        {
            case LiveSkillSettings:
                this.live.UpdateSkill(skill);
                break;
            case DamageSettings:
                this.damage.UpdateSkill(skill);
                break;
            case MoveSkillSettings:
                this.move.UpdateSkill(skill);
                break;
            case MagnetSkillSettings:
                this.magnet.UpdateSkill(skill);
                break;
            case ArmorSkillSettings:
                this.armor.UpdateSkill(skill);
                break;
            case RadiusSettings:
                this.radius.UpdateSkill(skill);
                break;
            case LootSkillSettings:
                this.loot.UpdateSkill(skill);
                break;
            case CooldownSettings:
                this.coolDown.UpdateSkill(skill);
                break;
            case RegenerationSkillSettings:
                this.regeneration.UpdateSkill(skill);
                break;
            case ExperienceSkillSettings:
                this.exp.UpdateSkill(skill);
                break;
        }
    }
}