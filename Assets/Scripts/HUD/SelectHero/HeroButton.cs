using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HeroButton : MonoBehaviour
{
    [SerializeField]
    private StarContainer _stars;

    [SerializeField]
    private GameObject _Lock;

    [SerializeField] 
    private Material _lockMaterial;

    [SerializeField] 
    private TextMeshProUGUI _nameText;

    [SerializeField] 
    private GameObject _pick;
    [SerializeField] 
    private Image _image;

    public void SetButton(HeroShopItem heroData, Sprite spriteHero,bool iselected)
    {
        _image.sprite = spriteHero;
        _pick.SetActive(iselected);
        _Lock.SetActive(!heroData.isUnlock);
        _image.material = heroData.isUnlock ? null : _lockMaterial;
        _nameText.text = heroData.heroName;
        if (!heroData.isUnlock)
        {
            _stars.SetLock();
        }
        else
        {
            _stars.SetStar(heroData.level);
        }
        
        

    }

    public void AddListeners(UnityAction action)
    {
        //MessageBroker.Default.Publish(new PlaySoundEvent("SelectButton"));
        gameObject.GetComponent<Button>().onClick.AddListener(action);
    }
}
