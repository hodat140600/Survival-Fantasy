using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundButton : MonoBehaviour
{
    [SerializeField]
    private Image _imageButton;
    [SerializeField]
    private Sprite _onSprite;

    [SerializeField]
    private Sprite _offSprite;

    private Sprite GetSprite(bool isSoundOn)
    {
        if (isSoundOn)
        {
            return _onSprite;
        }

        return _offSprite;
    }

    private void Start()
    {
        _imageButton.sprite = GetSprite(GameManager.Instance.isSoundOn);
    }

    public void OnClickSoundButton()
    {
        GameManager.Instance.ConfigSound();
        _imageButton.sprite = GetSprite(GameManager.Instance.isSoundOn);
    }
}
