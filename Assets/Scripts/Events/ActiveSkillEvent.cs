using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillEvent : IEvent
{
    public string name;
    public ActiveSkillEvent(string name)
    {
        this.name = name;
    }
}


