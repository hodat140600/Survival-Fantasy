using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Chapter/LevelConfig", fileName ="LevelConfig")]
public class ExpToLevelUp : ScriptableObject
{
    public List<int> expToLevelUp;
    public int GetExpCurrentLevel(int _currentLevel)
    {
        //int expLevelUp = 0;
        //for(int index = 0; index < _currentLevel; index++)
        //{
        //    expLevelUp += expToLevelUp[index];
        //}
        return expToLevelUp[_currentLevel - 1]/*expLevelUp*/;
    }
}
