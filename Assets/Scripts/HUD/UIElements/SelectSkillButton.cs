using Assets.Scripts.Skills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SelectSkillButton : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _nameText;
    [SerializeField]
    private Image _icon;

    [SerializeField]
    private TextMeshProUGUI _levelText;
    [SerializeField]
    private TextMeshProUGUI _descriptionText;

    public void LoadSelecSkill(ISkillSettings skill,bool isPause)
    {
        SkillDescriptionSystem skillDescriptionSystem = UIManager.Instance.gameObject.GetComponent<SkillDescriptionSystem>();
        _nameText.text = skillDescriptionSystem.GetName(skill.GetType().Name);
        if (skill.Level < 1)
        {
            _levelText.gameObject.SetActive(false);
        }
        else
        {
            _levelText.gameObject.SetActive(true);
            _levelText.text = "Lv " + skill.Level.ToString();
        }
        
        _icon.sprite = skillDescriptionSystem.GetSkillIcon(skill.GetType().Name);
        _descriptionText.text = skillDescriptionSystem.GetDescription(skill,isPause);
    }

    
}
