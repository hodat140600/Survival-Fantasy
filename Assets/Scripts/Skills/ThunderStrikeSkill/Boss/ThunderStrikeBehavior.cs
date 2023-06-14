using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using Assets.Scripts.Skills.Attributes;
using Assets.Scripts.Skills.LiveSkill;
using DG.Tweening;
using UnityEngine;

public class ThunderStrikeBehavior : DeathCircleBehavior
{
    private bool _isActive;

    private void Start()
    {
        base._skillObject = transform.GetChild(0).gameObject;

        base.Start();
    }

    protected override void OnCheckSphereState() { HitOnce(); }


    protected override void OnFireState(float time)
    {
        base.OnFireState(time);

        _isActive = true;
        StartCoroutine(DpsDealer(0.5f));
    }

    IEnumerator DpsDealer(float time)
    {
        while (_isActive)
        {
            base.HitOnce();
            yield return new WaitForSeconds(time);
        }
    }

    protected override void OnCompleteFireState()
    {
        base.OnCompleteFireState();
        _isActive = false;
    }
}
