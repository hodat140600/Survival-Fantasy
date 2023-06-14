using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

///<summary> Player ( Hero ) use this class </summary>
public class ExperienceSystem : MonoBehaviour
{
    [SerializeField] private int _level = 1;
    [SerializeField] private int _currentExp;
    public ExpToLevelUp expToLevelUp;

    //public int kinhnghiemchoNam = 0;
    public int ToLevelUp
    {
        get { return expToLevelUp.GetExpCurrentLevel(_level); }
    }
    private void Start()
    {
        MessageBroker.Default.Receive<ChapterStartEvent>().Subscribe(chapterStartEvent => { ResetExpPointAndLevel(); });
    }
    private const string Name = "ExpGem";
    private const string LevelUp = "LevelUp";
    public void AddExperience(int amount)
    {
        if (GameManager.Instance.State != GameState.Playing) return;
        _currentExp += amount;
        //kinhnghiemchoNam += amount;
        MessageBroker.Default.Publish(new PlaySoundEvent(Name));
        MessageBroker.Default.Publish(new AddExperienceEvent(ToLevelUp, _currentExp));
        CheckLevelUp();
    }

    void CheckLevelUp()
    {
        if (_currentExp >= ToLevelUp)
        {
            _currentExp -= ToLevelUp;
            _level += 1;
            MessageBroker.Default.Publish(new LevelUpEvent { toLevel = _level, currentExp = _currentExp, expToLvUp = ToLevelUp });
            MessageBroker.Default.Publish(new PlaySoundEvent(LevelUp));
        }
    }

    void ResetExpPointAndLevel()
    {
        _currentExp = 0;
        _level = 1;
        MessageBroker.Default.Publish(new ResetLevelEVent { toLevel = _level, currentExp = _currentExp, expToLvUp = ToLevelUp });
    }
}
