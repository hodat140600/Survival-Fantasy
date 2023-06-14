using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using Skills.Interfaces;
using UnityEngine;


[CreateAssetMenu(menuName = "Skills/LootSkillSettings", fileName = "LootSkillSettings")]
public class LootSkillSettings : ScriptableObject, IBaseSettings, IPercentSettings
{
    public int AddedPercent;
    [SerializeField] private string _id;
    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public void LevelUp(GameObject gameObject)
    {
        //! Never use this function
        // throw new System.NotImplementedException();
    }

    public void UpdateSkill(IBaseSettings skill)
    {
        var lootSkillPercent = skill as LootSkillSettings;

        this.AddedPercent += lootSkillPercent.AddedPercent;
    }

    public int Level { get; }
    public void AddPercent(int addedPercent)
    {
        this.AddedPercent += addedPercent;
    }

    public void SetPercent(int percent)
    {
        this.AddedPercent = percent;
    }

    public int GetAddedPercent()
    {
        return this.AddedPercent;
    }
    public string Description
    {
        get => "";
    }
}
