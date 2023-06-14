using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chapter/ChapterConfig", fileName = "Chapter")]
public class ChapterSettings : ScriptableObject
{
    public int Index;
    public SpawnSettings Spawn;
    public Map Map;
    public ScriptableObject skillEarn;
}
[System.Serializable]
public class EnemyData
{
    public EnemyStatus enemyStatus;
    public GameObject enemy;
}
[System.Serializable]
public class Map
{
    public Material materialRoad;
}