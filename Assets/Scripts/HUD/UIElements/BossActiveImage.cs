using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossActiveImage : MonoBehaviour
{
    [SerializeField]
    private Image _image;
    [SerializeField] 
    private float _timeAnimation;
    

    private void OnEnable()
    {
       DOTween.To(() => new Color(1,1,1,0), value => _image.color = value, new Color(1,1,1,1), _timeAnimation).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        DOTween.Kill(this);
    }
}
