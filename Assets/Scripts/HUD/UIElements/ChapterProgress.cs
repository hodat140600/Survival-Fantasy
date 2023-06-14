using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Manager.HUD.UIElements
{
    public class ChapterProgress: MonoBehaviour
    {
        private Slider _slider;
        private int _lastIdWave;
        private void Start()
        {
            _slider = gameObject.GetComponent<Slider>();
            /*MessageBroker.Default.Receive<TimeSpawnBossEvent>().Subscribe(timeSpawnBossEvent =>
            {
                _timeBoss = timeSpawnBossEvent.timeSpawnBoss;
            }).AddTo(gameObject);
            MessageBroker.Default.Receive<TickEvent>().Subscribe(tickEvent =>
            {
                OnTickEvent();
            }).AddTo(gameObject);*/
            
            MessageBroker.Default.Receive<CurrentWave>().Subscribe(currentWave => OnCurrentWave(currentWave.curWave)).AddTo(gameObject);
            MessageBroker.Default.Receive<ChapterStartEvent>().Subscribe(chapterStartEvent => { ResetChapterProgress(); }).AddTo(gameObject);
        }

        /*void OnTickEvent()
        {
            //int timer = numberTime == 0 ? 60 : numberTime;
            //_tickTime = _tickTime - _tickTime % 60 + timer;
            _tickTime += _stepCount;
            ChangeValueSlider((float)_tickTime / _timeBoss);
        }*/
        private void OnCurrentWave(int currentWave)
        {
            //if (_lastIdWave < currentWave)
            //{
                ChangeValueSlider((float)currentWave/EnemySpawner.Instance.TotalWavesInChapter);
                //_lastIdWave = currentWave;
            //}
        }
        private void ChangeValueSlider(float percent)
        {
            _slider.value = percent;
        }
        private void ResetChapterProgress()
        {
            _lastIdWave = 0;
            ChangeValueSlider(0);
        }
    }
}