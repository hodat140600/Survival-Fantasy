using UniRx;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetThisHero : MonoBehaviour
{
    [SerializeField] 
    private Button _getHeroButton;

    [SerializeField] 
    private TextMeshProUGUI _moneyBuy;

    [SerializeField] 
    private ChapterStorePanel _chapterStorePanel;
    [SerializeField] 
    private NewHeroPanel _newHeroPanel;

    [SerializeField] 
    private SelectContainer _selectContainer;
    [SerializeField] 
    private BuyButton _buyButton;

    [SerializeField] private LockPanel _lockPanel;
    [SerializeField] private ModelHero _modelHero;


    private void OnEnable()
    {
        _modelHero.ResetModelRotation();
    }

    public void SetBuyButton(int heroId,int money)
    {
        _getHeroButton.onClick.RemoveAllListeners();
        _moneyBuy.text = money.ToString();
        _getHeroButton.onClick.AddListener((() =>
        {
            MessageBroker.Default.Publish(new PlaySoundEvent("AcceptButton"));
            GameManager.Instance.playerSettings.BuyHero(heroId,money);
            gameObject.SetActive(false);
            _newHeroPanel.SetName(HeroLibrary.Instance.GetNameHero(heroId));
            _newHeroPanel.gameObject.SetActive(true);
            _chapterStorePanel.UpdateHeroContainer();
            _selectContainer.SetPanel(true,false,heroId);//set select
            _buyButton.SetButton(true,heroId);
            _lockPanel.SetLock(true);
            _lockPanel.UpdateSkillContainer(heroId,1);//update lock Panel level 1
        }));
        
    }
    public void GetHero(int heroId, int money)
    {
        GameManager.Instance.playerSettings.BuyHero(heroId, money);
        gameObject.SetActive(false);
        _newHeroPanel.SetName(HeroLibrary.Instance.GetNameHero(heroId));
        _newHeroPanel.gameObject.SetActive(true);
        _chapterStorePanel.UpdateHeroContainer();
        _selectContainer.SetPanel(true, false, heroId);//set select
        _buyButton.SetButton(true, heroId);
        _lockPanel.SetLock(true);
        _lockPanel.UpdateSkillContainer(heroId, 1);//update lock Panel level 1
    }
}
