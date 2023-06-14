using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddExperienceEvent : IEvent
{
    public int toLvUp;
    public int current;
    public AddExperienceEvent(int toLvUp, int current)
    {
        this.toLvUp = toLvUp;
        this.current = current;
    }
    //public float ExpChangePct { get { return _expCurrent / _expToLvUp; } }
}
