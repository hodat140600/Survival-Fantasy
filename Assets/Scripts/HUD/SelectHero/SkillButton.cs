using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using Skills.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] 
    private Image _icon;
    [SerializeField] 
    private TextMeshProUGUI _skillName;
    [SerializeField] 
    private TextMeshProUGUI _percent;

    [SerializeField] 
    private GameObject _lockGameObject;
    [SerializeField]
    private Material _lockMaterial;

    [SerializeField] 
    private GameObject allSkillIcon;

    [SerializeField] 
    private GameObject _rankGameObject;
    

    private SkillDescriptionSystem _skillDescriptionSystem;
   
    

    public void SetSkill(IBaseSettings skill,bool isLock,bool isAllSkill)
    {
        _skillDescriptionSystem = UIManager.Instance.gameObject.GetComponent<SkillDescriptionSystem>();
        _icon.sprite = _skillDescriptionSystem.GetSkillIcon(skill.GetType().Name);
        _skillName.text = _skillDescriptionSystem.GetName(skill.GetType().Name);
        _icon.material = isLock ? _lockMaterial : null;
        var percentSkill = skill as IPercentSettings;
        _percent.text = percentSkill.GetAddedPercent().ToString() +" %";
        _percent.gameObject.SetActive(!isLock);
        _lockGameObject.SetActive(isLock);
        _rankGameObject.SetActive(isLock);
        allSkillIcon.SetActive(isAllSkill);
        
    }
    
}
