
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using Skills.Interfaces;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/ArmorSkillSettings", fileName = "ArmorSkillSettings")]
public class ArmorSkillSettings : ScriptableObject, IBaseSettings, IPercentSettings
{
    [Range(0.0f, 10000f)] public int addedPercent;
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
    public void LevelUp(GameObject gameObject)
    {
        IArmorSkillBehavior[] behaviors = gameObject.GetComponents<IArmorSkillBehavior>();
        foreach (var behavior in behaviors)
        {
            behavior.IncreaseArmorPercent(addedPercent);
        }
    }

    public void UpdateSkill(IBaseSettings skill)
    {
        var armorSkillSettings = skill as ArmorSkillSettings;

        addedPercent += armorSkillSettings.addedPercent;
    }


    public string Description
    {
        get
        {
            string description = "";
            if (addedPercent != 0)
            {
                description += "<color=white>Armor <color=green>+" + addedPercent.ToString() +"%";
            }

            return description;
        }

    }

    

    public void AddPercent(int addedPercent)
    {
        addedPercent += addedPercent;
    }

    public int GetAddedPercent()
    {
        return addedPercent;
    }

    public void SetPercent(int percent)
    {
        addedPercent = percent;
    }
}