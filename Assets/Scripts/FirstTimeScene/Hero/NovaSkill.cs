using Assets.Scripts.Skills.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovaSkill : MonoBehaviour
{
    [SerializeField] ParticleSystem smashingEffect;
    [SerializeField] bool isSmashing;
    [SerializeField] AudioSource _audioSource;

    public void NovaSkillInCutScene()
    {
        smashingEffect.Play();
    }
}
