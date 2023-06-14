using Assets._SDK.Ads.AdsClient;
using Assets._SDK.Analytics;
using Assets.Scripts.Skills;
using RocketSg.Sdk.AdsClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUpgradePanel : MonoBehaviour
{
	[SerializeField]
	private Image _iconSkill;

	[SerializeField]
	private TextMeshProUGUI _currentText;

	[SerializeField]
	private TextMeshProUGUI _nextText;

	[SerializeField]
	private TextMeshProUGUI _skillNameText;

	[SerializeField]
	private TextMeshProUGUI _upgradeText;

	[SerializeField]
	private Button _clamButton;

	[SerializeField]
	private GameObject _auraLight;

	private float _speed = 2f;

	private Vector3[] zAxis = { new Vector3(0, 0, 1), new Vector3(0, 0, -1) };

	void Update()
	{
		for (int i = 0; i < _auraLight.transform.childCount; i++)
		{
			_auraLight.transform.GetChild(i).transform.RotateAround(_auraLight.transform.position, zAxis[i], _speed);
		}
	}

	public void SetPanel(ISkillSettings skillSettings)
	{

		SkillDescriptionSystem skillDescriptionSystem =
			UIManager.Instance.gameObject.GetComponent<SkillDescriptionSystem>();
		_currentText.text = "LV " + (skillSettings.Level - 1).ToString();
		_nextText.text = "LV " + (skillSettings.Level).ToString();
		_upgradeText.text = skillSettings.Description;
		_iconSkill.sprite = skillDescriptionSystem.GetSkillIcon(skillSettings.GetType().Name);
		_skillNameText.text = skillDescriptionSystem.GetName(skillSettings.GetType().Name);
		_clamButton.onClick.RemoveAllListeners();
		_clamButton.onClick.AddListener(() =>
		{
			if (AdsClientManager.Instance.AdsClient.IsRewardedVideoReady)
			{
				AdsClientManager.Instance.AdsClient.ShowRewardedVideo(LevelManager.Instance.CurrentChapter, AnalyticsEvent.REWARDED_VIDEO_SHOW, rewarded =>
				{
					if (rewarded == ShowResult.Failed || rewarded == ShowResult.Skipped) return;
					GetReward(skillSettings);
				});
			}
			else
			{
				GetReward(skillSettings);
			}
		});
	}

	private void GetReward(ISkillSettings skillSettings)
	{
		UIManager.Instance.OnUpgradeButton();
		GameManager.Instance.skillSystem.SelectSkill(skillSettings);
	}

	public void OnNoThankButtonClick()
	{
		UIManager.Instance.OnUpgradeButton();
	}

}
