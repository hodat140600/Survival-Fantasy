using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ChapterBossComing : MonoBehaviour
{
    [SerializeField] 
    private int _timeDisable;
    [SerializeField] 
    private int _timeAnimation;
    

    [SerializeField] 
    private Image _imagePanel;

    

    [SerializeField] 
    private GameObject _bossActiveImage;//image boss tren thanh slider

    private const float ALPHA_COLOR = 0.3f;
    private void Awake()
    {
        MessageBroker.Default.Receive<ChapterStartEvent>().Subscribe(chapterStartEvent =>
        {
            _bossActiveImage.SetActive(false);
        }).AddTo(gameObject);
    }
    private const string WarningBoss = "WarningBoss";
    private void OnEnable()
    {
        Color alpha = _imagePanel.color;
        alpha.a = ALPHA_COLOR;
        DOTween.To(() => _imagePanel.color, value => _imagePanel.color = value, alpha, _timeAnimation).SetLoops(-1, LoopType.Yoyo);
        Invoke("DisableGameObject",_timeDisable);
        _bossActiveImage.SetActive(true); 
        MessageBroker.Default.Publish(new PlaySoundEventWithTimeLife(WarningBoss, _timeDisable));
    }

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        /*_bossActiveImage.SetActive(false);*/
        _imagePanel.color = new Color(1,0,0,0);
        DOTween.Kill(this);
        //MessageBroker.Default.Publish(new StopSoundEvent(WarningBoss));
    }

}
