using UniRx;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets._SDK.Analytics;
using Assets._SDK.Ads.AdsClient;
using RocketSg.Sdk.AdsClient;

public class BuyButton : MonoBehaviour
{
    [SerializeField]
    private ChapterStorePanel _chapterStorePanel;
    [SerializeField]
    private Sprite _buyAdSprite;
    [SerializeField]
    private Sprite _buyMoneySprite;
    [SerializeField]
    private Sprite _rankUpSprite;
    [SerializeField]
    private TextMeshProUGUI _money;
    [SerializeField]
    private Material _cantBuyMaterial;
    [SerializeField]
    private Material _buyMaterial;
    [SerializeField]
    private GetThisHero _getThisHeroPanel;

    [SerializeField]
    private LockPanel _lockPanel;

    //private void OnEnable()
    //{
    //    var hero = GameManager.Instance.playerSettings.SelectedHero;
    //}
    private void Start()
    {
        transform.localScale=Vector3.one;
        var mysquence = DOTween.Sequence();
        mysquence.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f).SetDelay(3f));
        mysquence.Append(transform.DOScale(Vector3.one, 0.3f));
        mysquence.SetLoops(-1, LoopType.Restart);
    }
    private bool IsAdsBuyButtonType(HeroSettings heroSettings)
    {
        if(heroSettings.currency == HeroSettings.CurrencyType.Ads)
        {
            return GameManager.Instance.playerSettings.adsViewTime[heroSettings.GetHeroIDToString()] < heroSettings.adsCost;
        }
        return false;
    }

    private Sprite GetBuyButtonType(bool adsButton)
    {
        Sprite spriteButton = null;
        switch (adsButton)
        {
            case false :
                spriteButton = _buyMoneySprite;
                break;
            case true :
                spriteButton = _buyAdSprite;
                break;
        }
        return spriteButton;
    }
    private int HeroPrice(HeroSettings heroSettings)
    {
        if (heroSettings.currency == HeroSettings.CurrencyType.Ads)
        {
            if(GameManager.Instance.playerSettings.adsViewTime[heroSettings.GetHeroIDToString()] < heroSettings.adsCost) 
            {
                return heroSettings.adsCost;
            }
        }
        return heroSettings.moneyCost;
    }
    private string HeroPriceText(bool adsButton, int cost, int heroID)
    {
        return adsButton ? (GameManager.Instance.playerSettings.adsViewTime[heroID.ToString()] + "/" + cost) : cost.ToString();
    }
    public void SetButton(bool isUnlock/*, bool isAdBuy*/, int heroID)
    {
        gameObject.SetActive(true);
        var level = GameManager.Instance.playerSettings.GetLevelHero(heroID);
        var hero = HeroLibrary.Instance.GetHero(heroID, level + 1);
        if (hero == null)
        {
            gameObject.SetActive(false);
            return;
        }
        bool isAdsButton = IsAdsBuyButtonType(hero);
        var money = HeroPrice(hero/*.moneyCost*/);
        _money.text = /*money.ToString()*/ HeroPriceText(isAdsButton, money, heroID);
        var image = gameObject.GetComponent<Image>();
        var button = gameObject.GetComponent<Button>();
        image.sprite = isUnlock ? _rankUpSprite : /*_buyMoneySprite*/ GetBuyButtonType(isAdsButton);

        button.onClick.RemoveAllListeners();
        var check = GameManager.Instance.playerSettings.CheckMoney(money , isAdsButton, heroID);
        //if ads=>
        if (!check && !isAdsButton)
        {
            image.material = _cantBuyMaterial;
            image.material.mainTexture = _cantBuyMaterial.mainTexture;
            transform.localScale=Vector3.one;
            DOTween.Kill(this);
            return;
        }
        image.material = _buyMaterial;
        button.onClick.AddListener(() =>
        {
            MessageBroker.Default.Publish(new PlaySoundEvent("BuyableSkill"));
            if (!isUnlock && !isAdsButton)
            {
                _chapterStorePanel.OnBuyButton();
                _getThisHeroPanel.SetBuyButton(heroID, money);
            }
            else if (isUnlock && !isAdsButton)
            {
                GameManager.Instance.playerSettings.LevelUp(heroID, money);
                _lockPanel.UpdateSkillContainer(heroID, level + 1);//da update level
                SetButton(true, heroID);
                _chapterStorePanel.UpdateHeroContainer();
            }
            else
            {
                if (AdsClientManager.Instance.AdsClient.IsRewardedVideoReady)
                {
                    AdsClientManager.Instance.AdsClient.ShowRewardedVideo(LevelManager.Instance.CurrentChapter, AnalyticsEvent.REWARDED_VIDEO_SHOW, rewarded =>
                    {
                        if (rewarded == ShowResult.Failed || rewarded == ShowResult.Skipped) return;
						GetReward(heroID);
					});
                }else
                {
                    GetReward(heroID);
				}

            }
        });
    }

	private void GetReward(int ID)
    {
		GameManager.Instance.playerSettings.IncreaseAdsView(ID);
        if (HeroLibrary.Instance.GetHero(ID).adsCost == GameManager.Instance.playerSettings.adsViewTime[ID.ToString()])
        {
            _getThisHeroPanel.GetHero(ID, 0);
            return;
        }
		SetButton(false, ID);
	}


}
