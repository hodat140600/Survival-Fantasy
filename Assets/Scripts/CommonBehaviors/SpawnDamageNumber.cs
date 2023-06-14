using DamageNumbersPro;
using DamageNumbersPro.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDamageNumber : Singleton<SpawnDamageNumber>
{
    public DamageNumber numberPrefab;
    public DNP_PrefabSettings settings;    
    public DamageNumber healNumberPrefab;
    public DNP_PrefabSettings healSettings;
    public Color color;
    public void SpawnDamageNumberMesh(Vector3 position, Transform followedTransform, float number)
    {
        DamageNumber damageNumber = numberPrefab.Spawn(position + (Vector3.up * 5), number);
        settings.Apply(damageNumber);
        //damageNumber.PrewarmPool();
    }
    public void SpawnHealNumberMesh(float number)
    {
        DamageNumber damageNumber = healNumberPrefab.Spawn(GameManager.Instance.playerTransform.position + (Vector3.up * 10), number);
        healSettings.Apply(damageNumber);
    }

}
