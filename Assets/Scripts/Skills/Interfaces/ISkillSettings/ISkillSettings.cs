using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Skills
{
    
    public interface ISkillSettings
    {
        void LevelUp(GameObject gameObject);
        public int Level { get; }
        public string Description { get; }

        public string Id { get; }

    }
}
