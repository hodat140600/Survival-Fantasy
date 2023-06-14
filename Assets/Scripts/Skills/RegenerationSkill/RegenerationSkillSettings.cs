using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using Skills.Interfaces;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/RegenerationSkillSettings", fileName = "RegenerationSkillSettings")]
public class RegenerationSkillSettings : ScriptableObject, IBaseSettings, IPercentSettings
{
    public Health health;
    public Cooldown cooldown;

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
    private RegenerationSkillBehavior _skill;
    private GameObject _gameObject;


    private void Start()
    {
        health = new Health();
        cooldown = new Cooldown();
    }

    public void LevelUp(GameObject gameObject)
    {
        _gameObject = gameObject;
        _skill = _gameObject.GetComponent<RegenerationSkillBehavior>();

        if (_skill == null)
        {
            _skill = gameObject.AddComponent<RegenerationSkillBehavior>();
        }
        UpdateSkill();
    }

    private void UpdateSkill()
    {
        _skill.health.BasePoint += health.BasePoint;
        _skill.health.Percent += health.Percent;

        _skill.cooldown.BasePoint += cooldown.BasePoint;
        _skill.cooldown.Percent += cooldown.Percent;
    }

    public void UpdateSkill(IBaseSettings skill)
    {
        var regenerationSkillSettings = skill as RegenerationSkillSettings;

        health.BasePoint += regenerationSkillSettings.health.BasePoint;
        health.Percent += regenerationSkillSettings.health.Percent;

        cooldown.BasePoint += regenerationSkillSettings.cooldown.BasePoint;
        cooldown.Percent += regenerationSkillSettings.cooldown.Percent;
    }

    public void AddPercent(int addedPercent)
    {
        health.Percent += addedPercent;
    }

    public void SetPercent(int percent)
    {
        health.Percent = percent;
    }

    public int GetAddedPercent()
    {
        return health.Percent;
    }

    public string Description
    {
        get
        {
            string description = "";
            if (health.Percent != 0)
            {
                description += "<color=white>Regeneration <color=green>+" + health.Percent.ToString() + "%";
            }
            return description;
        }
    }
}