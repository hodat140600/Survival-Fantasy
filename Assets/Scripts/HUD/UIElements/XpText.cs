using System;
using TMPro;
using UniRx;
using UnityEngine;

public class XpText : MonoBehaviour
{
    private TextMeshProUGUI _xpText;
    private void Awake()
    {
        MessageBroker.Default.Receive<LevelUpEvent>().Subscribe(levelUpEvent => UpdateLevelText(levelUpEvent.toLevel)).AddTo(gameObject);
    }

    void UpdateLevelText(int level)
    {
        if (_xpText == null)
        {
            _xpText = gameObject.GetComponent<TextMeshProUGUI>();
        }

        _xpText.text = "Level " + level.ToString();
    }
}
