using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SpriteHero
{
    public string name;
    public Sprite sprite;
}

public class ChapterStorePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _getThisHero;

    [SerializeField]
    private List<SpriteHero> _spriteHeroes;

    [SerializeField]
    private GameObject _heroButtonContainer;

    [SerializeField]
    private ModelHero _modelHero;

    private PlayerSettings _playerSettings;
    [SerializeField]
    private LockPanel _lockPanel;

    [SerializeField]
    private SelectContainer _selectContainer;

    [SerializeField]
    private BuyButton _buyButton;
    [SerializeField] 
    private GameObject _modelStore;

    [SerializeField]
    private ScrollRect _scrollRect;

    private void OnEnable()
    {
        UpdateStore();
    }

    public void UpdateStore()
    {
        var hero = GameManager.Instance.playerSettings.SelectedHero;
        _modelHero.SetModel(hero.heroID);
        _lockPanel.SetPanel(hero.heroID, hero.level, true);
        _buyButton.SetButton(true, hero.heroID);
        UpdateHeroContainer();
        _selectContainer.SetSelected();
        _scrollRect.verticalNormalizedPosition = 1;
    }


    public void UpdateHeroContainer()
    {
        _playerSettings = GameManager.Instance.playerSettings;
        int gold = _playerSettings.Gold;
        var heroSettings = HeroLibrary.Instance.GetListHero();
        for (int i = 0; i < _heroButtonContainer.transform.childCount; i++)
        {
            HeroButton heroButton = _heroButtonContainer.transform.GetChild(i).gameObject.GetComponent<HeroButton>();
            heroButton.SetButton(heroSettings[i], GetSpriteHero(heroSettings[i].heroName),_playerSettings.SelectedHero.heroID==heroSettings[i].heroID);
            var heroData = heroSettings[i];
            heroButton.AddListeners(() =>
            {
                MessageBroker.Default.Publish(new PlaySoundEvent("HoverUI"));
                _modelHero.SetModel(heroData.heroID);
                _lockPanel.SetPanel(heroData.heroID, heroData.level, heroData.isUnlock);
                _selectContainer.SetPanel(heroData.isUnlock, _playerSettings.IsSelectedHero(heroData.heroID), heroData.heroID);
                _buyButton.SetButton(heroData.isUnlock, heroData.heroID);
            });


        }
    }

    public Sprite GetSpriteHero(string name)
    {
        foreach (var item in _spriteHeroes)
        {
            if (item.name == name)
            {
                return item.sprite;
            }
        }
        Debug.LogError("Error Sprite " + name.ToString());
        return null;
    }
    public void OnBackButton()
    {
        MessageBroker.Default.Publish(new PlaySoundEvent("Back&TryAgainButton"));
        gameObject.SetActive(false);
        GameManager.Instance.LoadModelPlayer();
        _modelStore.SetActive(false);
    }

    public void OnBuyButton()
    {
        _getThisHero.SetActive(true);
    }

    public void OnExitGetThisOne()
    {
        _getThisHero.SetActive(false);
    }
}