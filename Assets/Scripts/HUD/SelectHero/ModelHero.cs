using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class Model
{
    public int id;
    public GameObject model;
}

public class ModelHero : MonoBehaviour
{
    [SerializeField]
    private List<Model> _models;

    public void SetModel(int heroId)
    {
       
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        foreach (var item in _models)
        {
            if (item.id == heroId)
            {
                item.model.SetActive(true);
            }
        }

    }
    public void ResetModelRotation()
    {
        gameObject.transform.rotation=Quaternion.Euler(0,0,0);
    }
    
}
