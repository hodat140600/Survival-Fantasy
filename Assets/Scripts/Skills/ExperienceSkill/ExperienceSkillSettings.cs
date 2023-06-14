using System.Collections;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using Skills.Interfaces;
using UnityEngine;


[CreateAssetMenu(menuName = "Skills/ExperienceSkillSettings", fileName = "ExperienceSkillSettings")]
public class ExperienceSkillSettings : ScriptableObject, IBaseSettings, IPercentSettings
{
    [Range(0.0f, 10000f)] public int addedPercent;
    [SerializeField] private int _level;
    public int Level
    {
        get { return _level; }
    }
    [SerializeField] private string _id;
    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public string Description
    {
        get
        {
            string description = "";
            if (addedPercent != 0)
            {
                description += "<color=white>Experience <color=green>+" + addedPercent.ToString() + "%";
            }

            return description;
        }

    }


    private ExperienceSkillBehavior _skill;
    private GameObject _gameObject;

    public void LevelUp(GameObject gameObject)
    {
        _gameObject = gameObject;
        _skill = _gameObject.GetComponent<ExperienceSkillBehavior>();

        if (_skill == null)
        {
            _skill = gameObject.AddComponent<ExperienceSkillBehavior>();
        }

        UpdateSkill();
    }
    private void UpdateSkill()
    {
        _skill.addedPercent += addedPercent;
    }

    public void AddPercent(int addedPercent)
    {
        this.addedPercent += addedPercent;
    }

    public void SetPercent(int percent)
    {
        addedPercent = percent;
    }

    public int GetAddedPercent()
    {
        return addedPercent;
    }

    public void UpdateSkill(IBaseSettings skill)
    {
        var experienceSkillSettings = skill as ExperienceSkillSettings;

        this.addedPercent += experienceSkillSettings.addedPercent;
    }
}