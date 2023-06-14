using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Skills.LiveSkill;
using DG.Tweening;
using UnityEngine;

public class EarthquakeBehavior : DeathCircleBehavior
{

    private void Start()
    {
        base.Start();
        _skillObject = transform.GetChild(0).gameObject;
    }

    protected override void OnCheckSphereState() { HitOnce(); }

}
