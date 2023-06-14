using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibrationButton : MonoBehaviour
{

    [SerializeField] 
    private Image _imageButton;
    [SerializeField] 
    private Sprite _onSprite;

    [SerializeField] 
    private Sprite _offSprite;

    private Sprite GetSprite(bool isVibration)
    {
        if (isVibration)
        {
            return _onSprite;
        }

        return _offSprite;
    }

    private void Start()
    {
        _imageButton.sprite = GetSprite(GameManager.Instance.isVibration);
    }

    public void OnClickVibrationButton()
    {
        GameManager.Instance.ChangeVibration();
        _imageButton.sprite=GetSprite(GameManager.Instance.isVibration);
    }
}
