using System;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.GoldSkill;
using Assets.Scripts.Skills.Heal;
using Assets.Scripts.Skills.LiveSkill;
using Assets.Scripts.Skills.ProjectilesSkill;
using Assets.Scripts.Skills.SlowSkill;
using Assets.Scripts.Skills.SmashSkill;
using Assets.Scripts.Skills.SplashSkill;
using Assets.Scripts.Skills.TargetSkill;
using heroSkills = DatHQ.Skill.Hero;
using UnityEngine;
using DatHQ.Skill.Hero;

[Serializable]
public class SkillDescription
{
    [SerializeField]
    public string name;
    [SerializeField]
    public Sprite sprite;
    [SerializeField]
    public string descriptionText;
}

public class SkillDescriptionSystem : MonoBehaviour
{
    private Dictionary<string, SkillDescription> skillDescriptions = new Dictionary<string, SkillDescription>();

    [SerializeField] public SkillDescription orbsSkillDescription;
    [SerializeField] public SkillDescription radiusSkillDescription;
    [SerializeField] public SkillDescription moveSkillDescription;
    [SerializeField] public SkillDescription damageSkillDescription;
    [SerializeField] public SkillDescription liveSkillDescription;
    [SerializeField] public SkillDescription targetSkillDescription;
    [SerializeField] public SkillDescription blastSkillDescription;
    [SerializeField] public SkillDescription splashSkillDescription;
    [SerializeField] public SkillDescription slowSkillDescription;
    [SerializeField] public SkillDescription smashSkillDescription;
    [SerializeField] public SkillDescription chainLightningSkillDescription;
    [SerializeField] public SkillDescription lootSkillDescription;
    [SerializeField] public SkillDescription projectilesSkillDescription;
    [SerializeField] public SkillDescription goldSkillDescription;
    [SerializeField] public SkillDescription magnetSkillDescription;
    [SerializeField] public SkillDescription armorSkillDescription;
    [SerializeField] public SkillDescription regenerationSkillSettings;
    [SerializeField] public SkillDescription experienceSkillSettings;
    [SerializeField] public SkillDescription cooldownSettings;
    [SerializeField] public SkillDescription meteorStrikeSkillSettings;
    [SerializeField] public SkillDescription laserBeamSkillSettings;
    [SerializeField] public SkillDescription frostFieldSkillSettings;
    [SerializeField] public SkillDescription earthquakeSkillSettings;
    [SerializeField] public SkillDescription healSkillSettings;
    [SerializeField] public SkillDescription poisonSkillSettings;
    [SerializeField] public SkillDescription pryoSkillSettings;

    private void Awake()
    {
        skillDescriptions.Add(nameof(OrbsSkillSettings), orbsSkillDescription);
        skillDescriptions.Add(nameof(TargetSkillSettings), targetSkillDescription);
        skillDescriptions.Add(nameof(BlastSkillSettings), blastSkillDescription);
        skillDescriptions.Add(nameof(SplashSkillSettings), splashSkillDescription);
        skillDescriptions.Add(nameof(SlowSkillSettings), slowSkillDescription);
        skillDescriptions.Add(nameof(SmashSkillSettings), smashSkillDescription);
        skillDescriptions.Add(nameof(ChainLightningSkillSetting), chainLightningSkillDescription);
        skillDescriptions.Add(nameof(LootSkillSettings), lootSkillDescription);
        skillDescriptions.Add(nameof(LiveSkillSettings), liveSkillDescription);
        skillDescriptions.Add(nameof(ProjectilesSettings), projectilesSkillDescription);
        skillDescriptions.Add(nameof(DamageSettings), damageSkillDescription);
        skillDescriptions.Add(nameof(MoveSkillSettings), moveSkillDescription);
        skillDescriptions.Add(nameof(RadiusSettings), radiusSkillDescription);
        skillDescriptions.Add(nameof(GoldSettings), goldSkillDescription);
        skillDescriptions.Add(nameof(MagnetSkillSettings), magnetSkillDescription);
        skillDescriptions.Add(nameof(ArmorSkillSettings), armorSkillDescription);
        skillDescriptions.Add(nameof(RegenerationSkillSettings), regenerationSkillSettings);
        skillDescriptions.Add(nameof(ExperienceSkillSettings), experienceSkillSettings);
        skillDescriptions.Add(nameof(CooldownSettings), cooldownSettings);
        skillDescriptions.Add(nameof(heroSkills.MeteorStrikeSkillSettings), meteorStrikeSkillSettings);
        skillDescriptions.Add(nameof(heroSkills.LaserBeamSkillSettings), laserBeamSkillSettings);
        skillDescriptions.Add(nameof(heroSkills.FrostFieldSkillSettings), frostFieldSkillSettings);
        skillDescriptions.Add(nameof(heroSkills.EarthquakeSkillSettings), earthquakeSkillSettings);
        skillDescriptions.Add(nameof(heroSkills.PoisonSkillSettings), poisonSkillSettings);
        skillDescriptions.Add(nameof(PryoSkillSettings), pryoSkillSettings);
        skillDescriptions.Add(nameof(HealSkillSettings),healSkillSettings);
    }

    public Sprite GetSkillIcon(string skillSettingsName)
    {
        return skillDescriptions[skillSettingsName].sprite;
    }

    public string GetDescription(ISkillSettings skill, bool isPause)
    {
        if (isPause)
        {
            return skillDescriptions[skill.GetType().Name].descriptionText;
        }

        if (skill.Level != 1)  //chi show thong so Added khi level khac 1
        {
            return skill.Description;
        }
        return skillDescriptions[skill.GetType().Name].descriptionText;
    }


    public string GetName(string skillSettingsName)
    {
        return skillDescriptions[skillSettingsName].name;
    }
}
