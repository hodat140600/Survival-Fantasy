using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkText : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    private void Reset()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    [Range(0f, 1f)] public float minAlpha = 0.1f;

    [Range(0f, 1f)] public float maxAlpha = 0.9f;

    [Tooltip("The fade duration in seconds")]
    public float fadeOutDuration = 1.0f;

    [Tooltip("blinking speed in blinks per second")]
    public float blinkingSpeed = 1.0f;

    // for controlling the end of blinking on click
    private bool blink;
    private Color originalColor;
    private float lerpFactor;

    private void OnEnable()
    {
        // start blinking
        StopAllCoroutines();
        StartCoroutine(BlinkingRoutine());
    }
    private const float FPS_CONFIG = 0.02f;
    private IEnumerator BlinkingRoutine()
    {
        // get the original color and setup min, max and fadeout color once
        originalColor = textMeshProUGUI.color;
        var maxColor = new Color(originalColor.r, originalColor.g, originalColor.b, maxAlpha);
        var minColor = new Color(originalColor.r, originalColor.g, originalColor.b, minAlpha);
        var fadeOutColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.0f);

        blink = true;

        var passedTime = 0f;
        while (blink)
        {
            // always loops forth and back between 0 and 1
            lerpFactor = Mathf.PingPong(passedTime, 1);
            // optionaly add ease-in and ease-out
            //lerpFactor = Mathf.SmoothStep(0, 1, lerpFactor);

            // change the order of minColor and maxColor
            // depending on the one you want to start with
            textMeshProUGUI.color = Color.Lerp(maxColor, minColor, lerpFactor);

            // add time passed since last frame multiplied by blinks/second
            passedTime += /*Time.deltaTime*/FPS_CONFIG * blinkingSpeed;

            // allow this frame to be rendered and continue
            // from here in the next frame
            yield return new WaitForSecondsRealtime(FPS_CONFIG);
        }

        // set alpha to max
        textMeshProUGUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, maxAlpha);

        // start fading out
        passedTime = 0f;
        while (passedTime < fadeOutDuration)
        {
            var lerpFactor = passedTime / fadeOutDuration;
            // optionally add ease-in and ease-out
            //lerpFactor = Mathf.SmoothStep(0f, 1f, lerpFactor);

            textMeshProUGUI.color = Color.Lerp(maxColor, fadeOutColor, lerpFactor);

            passedTime += /*Time.deltaTime*/FPS_CONFIG;
            yield return null;
        }

        // just to be sure set it to 0 hard
        textMeshProUGUI.color = fadeOutColor;

        // optionally hide or destroy this gameObject when done
        //gameObject.SetActive(false);
        //Destroy(gameObject);
    }

}
