using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Chapter/SpawnConfig", fileName = "Spawn")]
public class SpawnSettings : ScriptableObject
{
    public List<EnemyWave> waves;
    public List<EnemyData> enemies;
    public EnemyData boss;
    public int GetNumberPerWave(int waveID)
    {
        return this.waves[waveID].numberPerWave;
    }
    public float GetTimeWaveStart(int waveID)
    {
        return this.waves[waveID].timeForSpawnWave;
    }
    public float GetTimeBetweenWave(int waveID)
    {
        return this.waves[waveID].timeBetweenWave;
    }
    public int GetIdEnemyInWave(int waveID)
    {
        return this.waves[waveID].enemyTypeInWaves - 1;
    }
    public int GetNumberWave(int waveID)
    {
        return this.waves[waveID].waveTotal;
    }
    public void InitStatusEnemy()
    {
        foreach (EnemyData enemy in this.enemies)
        {
            enemy.enemy.GetComponent<EnemyVisitor>().enemyStatus = enemy.enemyStatus;
        }
    }
}
[System.Serializable]
public class EnemyWave
{
    public int idWave;
    public int enemyTypeInWaves;
    public int numberPerWave;
    public int waveTotal;
    public float timeForSpawnWave;
    public float timeBetweenWave;

    public int GetIdEnemyInWave(int idWave)
    {
        return this.enemyTypeInWaves - 1;
    }
}
