using Assets.Scripts.Skills.LiveSkill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    public Slider HealthSlider3D;
    // TODO: Chuyen ve dung LiveSkillBehavior
    [SerializeField] LiveSkillBehavior healthScript;
    [SerializeField] private float updateSpeedSeconds = 0.5f;
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
    public void Init(LiveSkillBehavior _health)
    {
        healthScript = _health;
        //healthSlider3D.maxValue = statsScript.maxHealth;
        healthScript.OnHealthPctChanged += HandleHealthChanged;
        HealthSlider3D.maxValue = 1;
    }

    private void HandleHealthChanged(float pct)
    {
        //if(pct != 100)
        //{
        //    StartCoroutine(ChangeToPct(pct));
        //}
        HealthSlider3D.value = pct;
    }

    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = HealthSlider3D.value;
        float elapsed = 0f;

        while (elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            HealthSlider3D.value = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }
        HealthSlider3D.value = pct;
    }
}
