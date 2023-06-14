using System;
using Assets._SDK.Ads.AdsClient;
using Assets._SDK.Analytics;
using Assets.Scripts.Events;
using DG.Tweening;
using RocketSg.Sdk.AdsClient;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using static MaxSdkCallbacks;

namespace Manager.HUD.UIElements
{
    public class RevivePanel : MonoBehaviour
    {
        [SerializeField]
        private Image _heartFill;

        [SerializeField]
        private int _timeToLose;

        [SerializeField]
        private Button _reviveButton;

        [SerializeField]
        private GameObject _revivePanel;
        private void OnEnable()
        {
            _reviveButton.interactable = true;
            _heartFill.fillAmount = 1f;
            DOTween.To(() => _heartFill.fillAmount, value => _heartFill.fillAmount = value, 0f, _timeToLose)
                .OnComplete(OnLose).SetUpdate(true).SetId(1);
        }

        private void OnLose()
        {
            DeactivePannel();
            GameManager.Instance.UpdateGameState(GameState.Lose);
        }

        private void DeactivePannel()
        {
            _revivePanel.SetActive(false);
            _reviveButton.interactable = false;
        }

        public void OnReviveButton()
        {
            if (AdsClientManager.Instance.AdsClient.IsRewardedVideoReady)
            {
                AdsClientManager.Instance.AdsClient.ShowRewardedVideo(LevelManager.Instance.CurrentChapter, AnalyticsEvent.REWARDED_VIDEO_SHOW, rewarded =>
                {
                    DOTween.Pause(1);
                    if (rewarded == ShowResult.Failed || rewarded == ShowResult.Skipped)
                    {
                        DOTween.Play(1);
                        return;
                    }
					GetReward();
				});
            }else
            {
                DOTween.Pause(1);
                GetReward();
			}

        }

        private void GetReward()
        {
			DeactivePannel();
			DOTween.Kill(1);
			GameManager.Instance.UpdateGameState(GameState.Playing);
			MessageBroker.Default.Publish(new OnReviveEvent());
		}
    }
}