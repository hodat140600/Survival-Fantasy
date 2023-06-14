using Assets.Scripts.Skills;
using Skills.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skills/IncreaseCooldownSettings", fileName = "IncreaseCooldownSettings")]
public class CooldownSettings : ScriptableObject, IBaseSettings, IPercentSettings
{
    [Range(0.0f, 10000f)]
    public int addedPercent;
    [SerializeField]
    private int _level;
    public int Level { get => _level; }
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
                description += "<color=white>Cooldown <color=green>-" + addedPercent.ToString() + "%";
            }

            return description;
        }

    }

    public void AddPercent(int addedPercent)
    {
        this.addedPercent += addedPercent;
    }

    public int GetAddedPercent()
    {
        return addedPercent;
    }

    public void SetPercent(int percent)
    {
        addedPercent = percent;
    }

    public void LevelUp(GameObject gameObject)
    {
        ICooldownSkillBehavior[] behaviors = gameObject.GetComponents<ICooldownSkillBehavior>();
        foreach (var behavior in behaviors)
        {
            behavior.IncreaseCooldownPercent(addedPercent);
        }
    }

    public void UpdateSkill(IBaseSettings skill)
    {
        var cooldownSettings = skill as CooldownSettings;

        this.addedPercent += cooldownSettings.addedPercent;
    }
}