using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SelectSkillPanel : MonoBehaviour
{
    // load cac skill tren UI
    public void LoadPanel()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            var obj = gameObject.transform.GetChild(i).gameObject;
            obj.SetActive(false);
        }
        var skills = GameManager.Instance.skillSystem.GetNextThreeSkillSettings();
        for (int buttonIndex = 0; buttonIndex < skills.Count; buttonIndex++)
        {
            
            var selectBtn = gameObject.transform.GetChild(buttonIndex).gameObject.GetComponent<SelectSkillButton>();
            selectBtn.gameObject.SetActive(true);
            selectBtn.LoadSelecSkill(skills[buttonIndex], false);
            selectBtn.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            var index = buttonIndex;
            selectBtn.gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(DelaySelectSkillButton(skills[index]));
            });
        }
    }
    WaitForSecondsRealtime secondsRealtime = new WaitForSecondsRealtime(0.4f);
    IEnumerator DelaySelectSkillButton(ISkillSettings skill)
    {
        yield return secondsRealtime;
        OnButtonSelectSkill(skill);
    }
    void OnButtonSelectSkill(ISkillSettings skill)
    {
        GameManager.Instance.UpdateGameState(GameState.Playing);
        UIManager.Instance.OnSelectSkill(skill);
        GameManager.Instance.skillSystem.SelectSkill(skill);
    }
}
