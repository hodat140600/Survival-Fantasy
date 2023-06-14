using System.Collections;
using UnityEngine;
using UniRx;
using Assets.Scripts.Skills.Attributes;
using System;
using System.Collections.Generic;
using Assets.Scripts.Events;
using Assets.Scripts.Utils;
using Assets.Scripts.Skills.Heal;

namespace Assets.Scripts.Skills.LiveSkill
{
    public class LiveSkillBehavior : SkillBehavior, ITakeDamageable, IArmorSkillBehavior,IHealthSkillBehavior
    {
        public Health health;
        public Armor armor;
        public event Action<float> OnHealthPctChanged;
        private Animator _animator;
        private int _numberDie;

        private float _timeHeal = 10f;

        private CompositeDisposable _disposables = new CompositeDisposable();

        void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            health = new Health();
            armor = new Armor();

            var chapterStartStream = MessageBroker.Default.Receive<ChapterStartEvent>()
                                        .Subscribe(chapterStartEvent => { OnResetLoad(); });
            var reviveEventStream = MessageBroker.Default.Receive<OnReviveEvent>().Subscribe(reviveEvent => { OnResetLoad(); });

            chapterStartStream.AddTo(_disposables);
            reviveEventStream.AddTo(_disposables);
        }

        private void Start()
        {
            _numberDie = 0;
            this.health.SetFullPoint();
        }

        private void OnDead()
        {
            _numberDie++;
            /*MessageBroker.Default.Publish(new PlayerDeadEvent { timeDied = _timeDie });*/
            if (_numberDie == 1)
            {
                GameManager.Instance.UpdateGameState(GameState.Revive);
            }
            else
            {
                GameManager.Instance.UpdateGameState(GameState.Lose);
            }
            _animator.SetBool("IsDead", true);
            //Invoke(nameof(TimeScale), 5f);
        }

        /*private void TimeScale()
        {
            Time.timeScale = 0;
        }*/

        private void OnResetLoad()
        {
            this.health.SetFullPoint();
            UpdateHealthHandle();
        }

        public void TakeDamage(int damage)
        {
            if (health.IsDead)
            {
                return;
            }
            if (armor != null)
            {
                damage -= MathUtils.GetPercentOfValue(armor.Percent, damage);
            }

            this.health.DecreaseHealth(damage);
            GameManager.Instance.Vibrate();
            UpdateHealthHandle();
        }

        public void Heal(Health health)
        {
            this.health.IncreaseHealth(health);
            UpdateHealthHandle();
        }

        ///<summary> Invoke to HealthSidler class, update health slider UI. Manager in LiveSkillBehavior : animator and logic IsDead </summary>
        private void UpdateHealthHandle()
        {
            OnHealthPctChanged?.Invoke(this.health.RemainingPercent);

            _animator.SetBool("IsDead", this.health.IsDead);
            //Time.timeScale = 1;
            if (this.health.IsDead) { OnDead(); }
        }


        private void OnEnable()
        {
            gameObject.GetComponentInChildren<HealthSlider>()?.Init(this);
        }

        public void IncreaseArmorPercent(int percent)
        {
            armor.Percent += percent;
        }

        private void OnDestroy()
        {
            _disposables.Clear();
        }

        IEnumerator HealthRegeneration()
        {
            while (true)
            {
                MessageBroker.Default.Publish(new UpdateCoolDownEvent(nameof(HealSkillSettings), _timeHeal));
                health.Heal();
                UpdateHealthHandle();
                yield return new WaitForSeconds(_timeHeal);
            }
        }

        private bool checkFirstInit=true;
        public void IncreaseHealth(int percent)
        {
            health.PercentHeal += percent;
            if (checkFirstInit)
            {
                StartCoroutine(HealthRegeneration());
                checkFirstInit = false;
            }

            
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}