using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuySkillButton : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI nameText;
    
    [SerializeField] 
    private Image icon;

    [SerializeField] 
    private TextMeshProUGUI percentText;
    
    [SerializeField] 
    private GameObject moneyContainer;
    [SerializeField] 
    private TextMeshProUGUI moneyText;
    
    [SerializeField] 
    private GameObject qcImage;
    

    public void LoadSkill(SkillPrice skillPrice, bool isBuyable)
    {
        
        SkillDescriptionSystem skillDescriptionSystem = UIManager.Instance.gameObject.GetComponent<SkillDescriptionSystem>();
        nameText.text = skillDescriptionSystem.GetName(skillPrice.Skill.GetType().Name);
        icon.sprite = skillDescriptionSystem.GetSkillIcon(skillPrice.Skill.GetType().Name);
        percentText.text = skillPrice.PercentToString();
        moneyText.text = skillPrice.Price.ToString();
        if (isBuyable)
        {
            moneyContainer.SetActive(true);
            qcImage.SetActive(false);
        }
        else
        {
            moneyContainer.SetActive(false);
            qcImage.SetActive(true);
        }
    }
}
