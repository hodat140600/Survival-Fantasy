using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeHeroPanel : MonoBehaviour,IDragHandler
{
    [SerializeField] 
    private GameObject modelHero;

    private Quaternion rotation;
    [SerializeField] 
    private float speed;
    public void OnDrag(PointerEventData eventData)
    {
        var vector = eventData.delta;
        rotation = Quaternion.Euler(0f, -vector.x * speed, 0f);
        modelHero.transform.rotation = rotation * modelHero.transform.rotation;
    }

    
}
