using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour
{
    public string dialogTxt;
    public TextMeshProUGUI dialog;
    public string heroNameTxt;
    public TextMeshProUGUI heroName;
    public Image image;
    public Sprite heroSprite;
    public GameObject buttonContinue;

    public float delay = 0.1f;
    private string fullText;
    private string currentText = "";

    private void OnEnable()
    {
        dialog.text = dialogTxt;
        heroName.text = heroNameTxt;
        image.sprite = heroSprite;
        buttonContinue.SetActive(false);
        StartCoroutine(SetEnableButton());
        fullText = dialog.text;
        StartCoroutine(ShowText());
    }

    IEnumerator SetEnableButton()
    {
        float time = dialogTxt.Length * delay;
        yield return new WaitForSecondsRealtime(time);
        buttonContinue.SetActive(true);
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i < fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i + 1);
            dialog.text = currentText;
            yield return new WaitForSecondsRealtime(delay);
        }
    }
}
