using UnityEngine;

[System.Serializable]
public class RewardItem
{
    public GameObject gameObjectPrefab;
    public Vector3 currentPosition
    {
        get { return gameObjectPrefab.transform.position; }
        set { gameObjectPrefab.transform.position = value; }
    }
    public string name
    {
        get { return gameObjectPrefab.name; }
    }
    public bool isActives;

    public void Init(GameObject gameObjectPrefab)
    {
        isActives = false;
        this.gameObjectPrefab = gameObjectPrefab;
    }
}