using UnityEngine;

[System.Serializable]
public class ExperienceItem
{
    public int expPoint;
    public GameObject gameObjectPrefab;
    private Transform _transform;
    public Vector3 currentPosition
    {
        get { return _transform.position; }
        set { _transform.position = value; }
    }
    public bool isActives;

    public void Init(int expPoint, GameObject gameObject)
    {
        isActives = false;
        this.expPoint = expPoint;
        gameObjectPrefab = gameObject;
        _transform = gameObjectPrefab.transform;
    }
}
