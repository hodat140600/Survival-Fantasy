using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSelectSkillPanel : MonoBehaviour
{
    [SerializeField] 
    private GameObject _startingText;
    [SerializeField] 
    private GameObject _levelUpPanel;

    [SerializeField] private SelectSkillPanel _selectSkillPanel;
    

    public void SetPanel(int currentLevel)
    {
        _startingText.SetActive(currentLevel==1);//active starting text khi level1
        _levelUpPanel.SetActive(currentLevel>1);//levelUp show Pannel
        _selectSkillPanel.LoadPanel();
    }
}
