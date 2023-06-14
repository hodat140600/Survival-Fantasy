using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCoolDownEvent : IEvent
{
    public string skillName;
    public float time;

    public UpdateCoolDownEvent(string skillName, float time)
    {
        this.skillName = skillName;
        this.time = time;
    }
}


