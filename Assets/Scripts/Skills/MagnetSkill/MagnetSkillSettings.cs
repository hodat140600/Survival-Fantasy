using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using Skills.Interfaces;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/MagnetSkillSettings", fileName = "MagnetSkillSettings")]
public class MagnetSkillSettings : ScriptableObject, IBaseSettings, IPercentSettings
{
    const float DEFAULT_SCAN_REWARD_DISTANCE = 2.0f;

    public Radius experienceRadius;
    public Radius rewardRadius;

    [SerializeField] private int _level;

    public int Level
    {
        get { return _level; }
        set { _level = value; }
    }
    [SerializeField] private string _id;
    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }
    private MagnetSkillBehavior _skill;
    private GameObject _gameObject;


    private void Start()
    {
        rewardRadius = new Radius();

        rewardRadius.BasePoint = DEFAULT_SCAN_REWARD_DISTANCE;
        rewardRadius.Percent = 0;
    }

    public void LevelUp(GameObject gameObject)
    {
        _gameObject = gameObject;
        _skill = _gameObject.GetComponent<MagnetSkillBehavior>();

        if (_skill == null)
        {
            _skill = gameObject.AddComponent<MagnetSkillBehavior>();
            ApplyBaseSettings();
        }
        UpdateSkill();
    }

    private void UpdateSkill()
    {
        _skill.radius.BasePoint += experienceRadius.BasePoint;
        _skill.radius.Percent += experienceRadius.Percent;


        //Không c?ng vùng c?m nh?n ??i v?i reward item
        _skill.rewardRadius.BasePoint += rewardRadius.BasePoint;
        _skill.rewardRadius.Percent += rewardRadius.Percent;
    }

    public void UpdateSkill(IBaseSettings skill)
    {
        var magnetSkillSettings = skill as MagnetSkillSettings;

        experienceRadius.BasePoint += magnetSkillSettings.experienceRadius.BasePoint;
        experienceRadius.Percent += magnetSkillSettings.experienceRadius.Percent;

        rewardRadius.BasePoint += magnetSkillSettings.rewardRadius.BasePoint;
        rewardRadius.Percent += magnetSkillSettings.rewardRadius.Percent;
    }

    private void ApplyBaseSettings()
    {
        SkillSystem skillSystem = _gameObject.GetComponent<SkillSystem>();
    }

    public void AddPercent(int addedPercent)
    {
        experienceRadius.Percent += addedPercent;
    }

    public void SetPercent(int percent)
    {
        experienceRadius.Percent = percent;
    }

    public int GetAddedPercent()
    {
        return experienceRadius.Percent;
    }

    public string Description
    {
        get
        {
            string description = "";
            if (experienceRadius.Percent != 0)
            {
                description += "<color=white>Magnet <color=green>+" + experienceRadius.Percent.ToString();
            }
            return description;
        }
    }
}
