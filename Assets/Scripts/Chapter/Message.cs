using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkillMessage
{
    public string SkillName;
    public ActiveSkillMessage(string skillName)
    {
        this.SkillName = skillName;
    }
}

public class StartCoolDownMessage
{
    public string skillName;
    public float coolDownTime;
}
