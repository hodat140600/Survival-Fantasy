using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject _info;

    [SerializeField]
    private GameObject _lock;

    public string skillName;
    public bool isUnlock;

    private SelectSkillButton _selectSkillButton;
    public void Unlock(ISkillSettings skill)
    {
        skillName = skill.GetType().Name;
        isUnlock = true;
        _lock.SetActive(false);
        _info.SetActive(true);

        if (_selectSkillButton == null)
        {
            _selectSkillButton = gameObject.GetComponent<SelectSkillButton>();
        }

        _selectSkillButton.LoadSelecSkill(skill, true);
    }

}
