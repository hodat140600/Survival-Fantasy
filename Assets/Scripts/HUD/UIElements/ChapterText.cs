using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class ChapterText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private void Awake()
    {
        _text = gameObject.GetComponent<TextMeshProUGUI>();
        MessageBroker.Default.Receive<ChapterStartEvent>().Subscribe(chapterStartEvent =>
        {
            //Debug.Log("Chapter Text : " + chapterStartEvent.CurrentChapter.ToString());
            _text.text = "STAGE "+chapterStartEvent.CurrentChapter.ToString();
        });
    }
}
