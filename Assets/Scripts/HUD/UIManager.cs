using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Skills;
using UnityEngine;
using Manager.HUD.UIElements;
using TMPro;
using UniRx;
using Assets._SDK.Analytics;
using Assets._SDK.Ads.AdsClient;

public class UIManager : Singleton<UIManager>
{
    
    [SerializeField]
    private int numberLevelUpgrade;
    
    [Header("Panel :")]
    public GameObject chapterSelectSkillPanel;
    public GameObject chapterStartPanel;
    public GameObject chapterVictoryPanel;
    public GameObject chapterLosePanel;
    public GameObject chapterRewardPanel;
    public GameObject chapterRevivePanel;
    public GameObject chapterPause;
    public GameObject chapterStore;
    public GameObject chapterBossComing;
    public GameObject chapterUpgrade;
    [Header("Elements")] public SkillContainer skillContainer;
    public GameObject skillContainerPrefabs;
    public GameObject pauseButton;
    public TextMeshProUGUI levelText;
    public GameObject modelStore;

    private int _lastLevelUpgrade;


    private Queue<int> nextLevels = new Queue<int>();
    private bool _isSelected;
    private bool _isActiveSkillPanelVisible;


    public void Start()
    {
        _lastLevelUpgrade = 0;
        chapterStartPanel.SetActive(true);
        //Select Skill Panel
        MessageBroker.Default.Receive<LevelUpEvent>()
            .Subscribe((levelUpEvent) =>
            {
                //Debug.Log("Len Level");
                nextLevels.Enqueue(levelUpEvent.toLevel);
                if (!_isActiveSkillPanelVisible)
                {
                    StartCoroutine(ActiveSkillSelectPanel());
                }
            })
            .AddTo(gameObject);
        MessageBroker.Default.Receive<LoseEvent>().Subscribe(loseEvent => { OnChapterLose(); })
            .AddTo(gameObject);
        //Reward Panel
        MessageBroker.Default.Receive<RewardEvent>().Subscribe(rewardEvent => { OnRewardChapter(); })
            .AddTo(gameObject);
        //chapterRevivePanel
        MessageBroker.Default.Receive<ReviveEvent>()
            .Subscribe(playerDeadEvent => { OnReviveChapter(); });
        //chapterBoss
        MessageBroker.Default.Receive<BossComeEvent>().Subscribe(bossEvent => { BossComeEvent(); })
            .AddTo(gameObject);
    }

    public void BossComeEvent()
    {
        chapterBossComing.SetActive(true);
    }

    public void ReloadUI()
    {
        _lastLevelUpgrade = 0;
        chapterBossComing.SetActive(false);
        ReloadSkillContainer();
        pauseButton.SetActive(false);
        levelText.text = "Level 1";
    }

    public void OnChapterStart()
    {
        chapterStartPanel.SetActive(true);
        ReloadUI();
        chapterPause.GetComponent<ChapterPause>().ReloadPause();
    }

    public void OnTabToStart()
    {
        GameManager.Instance.UpdateGameState(GameState.SelectSkill);
        pauseButton.SetActive(true);
        chapterStartPanel.SetActive(false);
        chapterSelectSkillPanel.SetActive(true);
        chapterSelectSkillPanel.GetComponent<ChapterSelectSkillPanel>().SetPanel(1); //startLevel
    }

    IEnumerator ActiveSkillSelectPanel()
    {
        _isActiveSkillPanelVisible = true;
        //g
        AdsClientManager.Instance.AdsClient.ShowInterstitial(nextLevels.Peek(), "LevelUp");
        while (nextLevels.Count > 0)
        {
            GameManager.Instance.UpdateGameState(GameState.SelectSkill);
            var level = nextLevels.Dequeue();
            ActiveLevelUpPanel(level);
            _lastLevelUpgrade++;
            yield return new WaitUntil(() => _isSelected);
            yield return new WaitForSecondsRealtime(0.5f); //doi 1s panel xuat hien lan sau
        }
        var iSkillSettings = GameManager.Instance.skillSystem.GetSkillUpgrade();
        if (_lastLevelUpgrade >= numberLevelUpgrade && nextLevels.Count == 0 && iSkillSettings != null)
        {
            GameManager.Instance.UpdateGameState(GameState.SelectSkill);
            _isSelected = false;
            chapterUpgrade.SetActive(true);
            chapterUpgrade.GetComponent<SkillUpgradePanel>().SetPanel(iSkillSettings);
            yield return new WaitForSecondsRealtime(0.5f);
            _lastLevelUpgrade = 0;
            yield return new WaitUntil(() => _isSelected);
        }
        _isActiveSkillPanelVisible = false;
    }
    float _timeDelayPanel = 2.5f;
    IEnumerator DelayPanel(float delayTime, GameObject panel, bool setActive)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        panel.SetActive(setActive);
    }
    public void OnUpgradeButton()
    {
        _isSelected = true;
        chapterUpgrade.SetActive(false);
        GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    public void OnSelectSkill(ISkillSettings skill)
    {
        _isSelected = true;
        chapterSelectSkillPanel.SetActive(false);
        skillContainer.SetActive(skill);
    }

    public void ActivePausePanel(bool isPause)
    {
        chapterPause.SetActive(isPause);
    }

    public void OnChapterVictory()
    {
        chapterSelectSkillPanel.SetActive(false);
        chapterBossComing.SetActive(false);
        StartCoroutine(DelayPanel(_timeDelayPanel, chapterVictoryPanel, true));
        //chapterVictoryPanel.SetActive(true);
    }

    public void OnChapterLose()
    {
        chapterSelectSkillPanel.SetActive(false);
        chapterBossComing.SetActive(false);
        chapterRevivePanel.SetActive(false);
        //StartCoroutine(DelayPanel(_timeDelayPanel, chapterLosePanel, true));
        chapterLosePanel.SetActive(true);
    }

    private void OnRewardChapter()
    {
        chapterRewardPanel.SetActive(true);
        GameManager.Instance.TogglePausing(true);
    }

    private void OnReviveChapter()
    {
        StartCoroutine(DelayPanel(_timeDelayPanel, chapterRevivePanel, true));
        //chapterRevivePanel.SetActive(true);
    }

    public void ExitRewardChapter()
    {
        chapterRewardPanel.SetActive(false);
        GameManager.Instance.TogglePausing(false);
    }
    private const string ShowUISkillSound = "ShowUISKill";
    public void ActiveLevelUpPanel(int currentLevel)
    {
        _isSelected = false;
        chapterSelectSkillPanel.SetActive(true);
        chapterSelectSkillPanel.GetComponent<ChapterSelectSkillPanel>().SetPanel(currentLevel);
        MessageBroker.Default.Publish(new PlaySoundEvent(ShowUISkillSound));
    }

    public void ReloadSkillContainer()
    {
        Vector3 skillContainerPosition = skillContainer.transform.position;
        Transform chapterHeader = skillContainer.gameObject.transform.parent;
        Destroy(skillContainer.gameObject);
        skillContainer = Instantiate(skillContainerPrefabs, skillContainerPosition, Quaternion.identity, chapterHeader)
            .GetComponent<SkillContainer>();
    }

    public void OnSelectHero()
    {
        MessageBroker.Default.Publish(new PlaySoundEvent("ShopUI"));
        chapterStore.SetActive(true);
        modelStore.SetActive(true);
    }
}