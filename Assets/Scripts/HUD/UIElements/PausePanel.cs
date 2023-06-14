using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    [SerializeField]
    private List<SkillInfo> _damageSkillInfos;

    [SerializeField]
    private List<SkillInfo> _attributeSkillInfos;

    private void OnEnable()
    {
        SkillSystem skillSystem = GameManager.Instance.skillSystem;
        foreach (var item in skillSystem.selectedSkillSettings.Values)
        {
            UnlockSkill(item is IDamageSettings ? _damageSkillInfos : _attributeSkillInfos, item);
        }
    }

    public void UnlockSkill(List<SkillInfo> skillInfos, ISkillSettings skill)
    {

        var skillInfo = skillInfos.Find(info => info.isUnlock == false || info.skillName == skill.GetType().Name);
        skillInfo.Unlock(skill);
    }
    

}
