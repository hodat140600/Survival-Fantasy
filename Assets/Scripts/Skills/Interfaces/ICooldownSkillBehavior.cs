using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICooldownSkillBehavior
{
    public void IncreaseCooldownPercent(int percent);
}