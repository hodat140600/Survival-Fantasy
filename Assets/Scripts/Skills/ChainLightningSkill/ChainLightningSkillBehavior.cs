using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using Skills.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLightningSkillBehavior : SkillBehavior, IDamageSkillBehavior, IProjectilesSkillBehavior, ICooldownSkillBehavior,IRadiusSkillBehavior
{
    public Cooldown cooldown;
    public int timeBouncing;
    void Awake()
    {
        damage = new Damage();
        numberProjectiles = new NumberProjectiles();
        cooldown = new Cooldown();
        radius = new Radius();
        timeBouncing = 0;
    }

    // TODO Chuyen code cua Circular Motion va inject OrbSkill GameObject vao day
    public void IncreaseDamagePercent(int percent)
    {
        damage.Percent += percent;

    }

    public void IncreaseRadiusPercent(int percent)
    {
        radius.Percent += percent;
    }

    public void IncreaseProjectilesPoint(int point)
    {
        numberProjectiles.AddPoint += point;
    }

    public void IncreaseCooldownPercent(int percent)
    {
        cooldown.Percent += percent;
    }
}
