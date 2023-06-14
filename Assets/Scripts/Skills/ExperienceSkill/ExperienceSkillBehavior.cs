using System.Collections;
using UnityEngine;
using UniRx;
using Assets.Scripts.Skills.Attributes;
using System;
using Assets.Scripts.Events;
using Assets.Scripts.Skills;
using Assets.Scripts.Utils;

public class ExperienceSkillBehavior : SkillBehavior
{
    [Range(0.0f, 10000f)] public int addedPercent;
    [SerializeField] private ExperienceSystem _experienceSystem;
    private CompositeDisposable _disposables = new CompositeDisposable();

    private void Start()
    {
        var addExpEvent = MessageBroker.Default.Receive<AddExpEvent>().Subscribe(addExpEvent => { AddExperience(addExpEvent.addExp); });
        addExpEvent.AddTo(_disposables);

        _experienceSystem = gameObject.GetComponent<ExperienceSystem>();
    }

    public void AddExperience(int amount)
    {
        //Debug.Log("Exp value : " + amount + ", skill percent : " + addedPercent);
        amount += MathUtils.GetPercentOfValue(addedPercent, amount);
        //Debug.Log("=> " + amount);
        _experienceSystem.AddExperience(amount);
    }

    private void OnDestroy()
    {
        _disposables.Clear();
    }
}