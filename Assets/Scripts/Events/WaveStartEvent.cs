using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveStartEvent : IEvent
{
    public int CurrentWaveId;
    public EnemyWave CurrentWave;

    public WaveStartEvent(int currentWaveId, EnemyWave currentWave)
    {
        this.CurrentWaveId = currentWaveId;
        this.CurrentWave = currentWave;
    }
}
