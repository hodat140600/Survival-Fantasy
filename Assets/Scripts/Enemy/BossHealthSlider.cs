using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(100)]
public class BossHealthSlider : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private float updateSpeedSeconds = 0.5f;
    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }

    public Slider healthSlider3D;

    [SerializeField] GameObject enemyObj;
    [SerializeField] EnemyVisitor statsScript;

    public void Init(EnemyVisitor _enemyHealth)
    {
        statsScript = _enemyHealth;
        //healthSlider3D.maxValue = statsScript.maxHealth;
        statsScript.OnHealthPctChanged += HandleHealthChanged;
        healthSlider3D.maxValue = 1;

        statsScript.health = statsScript.maxHealth;
    }

    private void HandleHealthChanged(float pct)
    {
        StartCoroutine(ChangeToPct(pct));
    }
    
    private IEnumerator ChangeToPct(float pct)
    {
        float preChangePct = healthSlider3D.value;
        float elapsed = 0f;

        while(elapsed < updateSpeedSeconds)
        {
            elapsed += Time.deltaTime;
            healthSlider3D.value = Mathf.Lerp(preChangePct, pct, elapsed / updateSpeedSeconds);
            yield return null;
        }
        healthSlider3D.value = pct;
    }
}
