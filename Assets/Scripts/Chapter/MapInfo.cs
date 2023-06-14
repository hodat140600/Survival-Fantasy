using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapInfo : MonoBehaviour
{
    [SerializeField] Renderer _rendererMap;
    public bool Top;
    public bool Right;
    public bool Down;
    public bool Left;

    [HideInInspector]
    public Int2? CurrentPosition;

    public void ChangeMaterialMap(Material rendererMapMaterial)
    {
        _rendererMap.material = rendererMapMaterial;
    }
}
