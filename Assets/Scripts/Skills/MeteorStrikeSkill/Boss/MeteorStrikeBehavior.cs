using System.Collections;
using Assets.Scripts.Skills.Attributes;
using UnityEngine;
using DG.Tweening;

public class MeteorStrikeBehavior : DeathCircleBehavior
{
    

    protected override void OnCheckSphereState()
    {
        //Debug.Log("Boss skill fireball");

        HitOnce();
        transform.GetChild(0).gameObject.SetActive(true);
    }


}
