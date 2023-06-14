using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skills/ChainLightningSkillSettings", fileName = "ChainLightningSkillSettingLevel")]
public class ChainLightningSkillSetting : ScriptableObject, IDamageSettings

{

    [SerializeField] private Damage _damage;

    [SerializeField] private NumberProjectiles _numberProjectiles;


    [SerializeField] private Radius _radius;

    [SerializeField] private Cooldown _cooldown;

    [SerializeField] private int _timeBouncing;
    private ChainLightningSkillBehavior _skill;
    private GameObject _gameObject;

    [SerializeField]
    private int _level;
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
    public void LevelUp(GameObject gameObject)
    {
        _gameObject = gameObject;
        _skill = _gameObject.GetComponent<ChainLightningSkillBehavior>();

        if (_skill == null)
        {
            _skill = gameObject.AddComponent<ChainLightningSkillBehavior>();
            UpdateSkill();
            ApplyBaseSettings();
        }
        else
        {
            UpdateSkill();
        }

    }
    private void UpdateSkill()
    {
        _skill.damage.BasePoint += _damage.RealPoint;
        _skill.radius.BasePoint += _radius.RealPoint;
        _skill.numberProjectiles.BasePoint += _numberProjectiles.RealPoint;
        _skill.cooldown.BasePoint += _cooldown.RealPoint;
        _skill.timeBouncing += _timeBouncing;
    }


    private void ApplyBaseSettings()
    {
        SkillSystem skillSystem = _gameObject.GetComponent<SkillSystem>();
        skillSystem.GetObjectSelectedSkil("ChainLightningSkill").GetComponent<ChainLightningController>().Init(_skill);
        skillSystem.ApplyDamageSetting(_skill);
        skillSystem.ApplyProjectilesSetting(_skill);
        skillSystem.ApplyCooldownSetting(_skill);
    }
    public string Description
    {
        get
        {
            string description = "";
            if (_damage.BasePoint != 0)
            {
                description += "<color=white>Damage <color=green>+" + _damage.BasePoint.ToString() + "<br>";
            }
            if (_radius.BasePoint != 0)
            {
                description += "<color=white>Radius <color=green>+" + _radius.BasePoint.ToString() + "<br>";
            }

            if (_numberProjectiles.BasePoint != 0)
            {
                description += "<color=white>Projectiles <color=green>+" + _numberProjectiles.BasePoint.ToString() + "<br>";
            }
            if (_cooldown.BasePoint != 0)
            {
                description += "<color=white>Cooldown <color=green>" + _cooldown.BasePoint.ToString() + "<br>";
            }
            if (_timeBouncing != 0)
            {
                description += "<color=white>Jump <color=green>+" + _timeBouncing.ToString() + "<br>";
            }
            return description;
        }

    }


}