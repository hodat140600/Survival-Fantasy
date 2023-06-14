using Assets.Scripts.Skills.SmashSkill;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Smashing : MonoBehaviour
{
    [SerializeField] List<EnemyBehavior> enemyListSmash;
    [SerializeField] ParticleSystem smashingEffect;
    [SerializeField] bool isSmashing;
    [SerializeField] Transform _player;
    [SerializeField] AudioSource _audioSource;
    SmashSkillBehavior _smashSkillBehavior;

    float timerCooldown = 0f;
    void Update()
    {
        timerCooldown += Time.deltaTime;
        if (timerCooldown >= _smashSkillBehavior.cooldown.RealPoint)
        {
            timerCooldown = 0;
            smashingEffect.gameObject.SetActive(false);
            isSmashing = false;
        }
        if (!isSmashing)
        { 
            enemyListSmash = CheckInRange.
                CheckEnemyInRange(_smashSkillBehavior.radius.RealPoint);
            Smash();
        }
    }
    void Smash()
    {
        MessageBroker.Default.Publish(new UpdateCoolDownEvent(nameof(SmashSkillSettings),_smashSkillBehavior.cooldown.RealPoint));
        isSmashing = true;
        _audioSource.Play();
        //cycle through all the enemies
        
        for (int i = 0; i < enemyListSmash.Count; i++)
        {
            if (enemyListSmash[i] != null)
            {
                enemyListSmash[i].TakeDamage(_smashSkillBehavior.damage.RealPoint);
            }
        }
        smashingEffect.gameObject.SetActive(true);
        //We are no longer smashing, so set the boolean to false
        //Invoke(nameof(ResetSmash), powerUp.TimeBetweenSmash);

    }
    public void Init(SmashSkillBehavior smashSkillBehavior)
    {
        _smashSkillBehavior = smashSkillBehavior;
        _smashSkillBehavior.smashVFX = this.gameObject;
        this.gameObject.SetActive(true);
    }
}
