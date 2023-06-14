using System;
using Assets.Scripts.Skills;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class SkillIcon : MonoBehaviour
{
    [SerializeField] private Image cooldownImage;
    public string name;
    public bool isActivated = false;
    public bool isDamage;
    [SerializeField] private Image _image;

    private void Reset()
    {
        _image = gameObject.GetComponent<Image>();
    }

    public void SetCoolDown(float time)
    {
        cooldownImage.gameObject.SetActive(true);
        if (!isActivated && time == 0)
        {
            return;
        }
        DOTween.Kill(this);
        DOTween.To(() => 1f, cooldownImageFillAmount => cooldownImage.fillAmount = cooldownImageFillAmount, 0f, time)
            .OnComplete(() => cooldownImage.fillAmount = 0f);
    }

    
    public void SetActive(ISkillSettings skill)
    {
        SkillDescriptionSystem skillDescriptionSystem = UIManager.Instance.gameObject.GetComponent<SkillDescriptionSystem>();
        _image.sprite = skillDescriptionSystem.GetSkillIcon(skill.GetType().Name);
        this.name = skill.GetType().Name;
        this.isActivated = true;
    }

}
