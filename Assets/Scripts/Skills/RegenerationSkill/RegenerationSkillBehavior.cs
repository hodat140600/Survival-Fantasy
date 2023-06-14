using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using UnityEngine;

public class RegenerationSkillBehavior : SkillBehavior, ICooldownSkillBehavior
{
    public Health health;
    public Cooldown cooldown;

    private void Awake()
    {
        health = new Health();
        cooldown = new Cooldown();
    }

    private void Start()
    {
        StartCoroutine(Regenerate());
    }

    IEnumerator Regenerate()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown.RealPoint);

            if (health.BasePoint != 0 && health.Percent != 0)
            {
                GameManager.Instance.skillSystem.ApplyAttribute(health);
            }
        }
    }

    public void IncreaseCooldownPercent(int percent)
    {
        cooldown.Percent += percent;
    }
}