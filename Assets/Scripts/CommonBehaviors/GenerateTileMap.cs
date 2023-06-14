using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateTileMap : MonoBehaviour
{
    public float sizeTile = 400f;
    public float scaleTile = 20f;
    public int cellToPos = 10;
    public int activeTileCount;
    public MyPooler.ObjectPooler tileMapPool;
    public GameObject prefabTile;
    [SerializeField]
    private List<GameObject> tileMapList;
    [SerializeField]
    private List<MapInfo> tileMapInfoList;
    private Transform _playerTransForm;
    private float _safeAreaMin;
    private float _safeAreaMax;
    private List<Int2> _coefficientAround;
    public const string NamePrefab = "Tile";
    Int2 currentPos;

    private void Reset()
    {
        scaleTile = 20f;
        cellToPos = 10;
        tileMapPool = GetComponent<MyPooler.ObjectPooler>();
    }

    private void Awake()
    {
        _playerTransForm = GameManager.Instance.playerTransform;
        var tileMapPercent = scaleTile / (sizeTile/*/2*/);
        _safeAreaMin = .5f - tileMapPercent;
        _safeAreaMax = .5f + tileMapPercent;

        _coefficientAround = new List<Int2>();
        _coefficientAround.Add(new Int2(-1,-1));
        _coefficientAround.Add(new Int2(-1,0));
        _coefficientAround.Add(new Int2(-1,1));
        _coefficientAround.Add(new Int2(0,-1));
        _coefficientAround.Add(new Int2(0,0));
        _coefficientAround.Add(new Int2(0,1));
        _coefficientAround.Add(new Int2(1,-1));
        _coefficientAround.Add(new Int2(1,0));
        _coefficientAround.Add(new Int2(1,1));
    }

    private void Start()
    {
        tileMapList = new List<GameObject>(tileMapPool.poolDictionary[NamePrefab].ToArray());

        currentPos = CalculateCurrentPosition();
    }
    //private void FixedUpdate()
    //{
    //    UpdateMap();
    //}
    //private void InstantiateHomeMapPart()
    //{
    //    var homeMapPart = tileMapPool.GetFromPool(NamePrefab, Vector3.zero, prefabTile.transform.rotation).GetComponent<MapInfo>();
    //    homeMapPart.CurrentPosition = new Int2(0, 0);
    //}
    //bool firstLoad = true;
    //private void UpdateMap()
    //{
    //    ReturnTileToPool();
    //    if (tileMapPool.activeObjects[NamePrefab].Count >= activeTileCount)
    //    {
    //        //Debug.Log("Tao " + currentPos + "Check " + firstLoad);
    //        return;
    //    }
    //    currentPos = CalculateCurrentPosition();
    //    //Debug.Log("Current Pos: " + currentPos);                      
    //    var nextPosition = CalculateNextPositions();

    //    for (int indexPos = 0; indexPos < nextPosition.Count; indexPos++)
    //    {
    //        //Debug.Log("Tao tile Map : " + nextPosition[indexPos]);
    //        if(!tileMapInfoList.Any(x => x.CurrentPosition == nextPosition[indexPos]))
    //        {
    //            var mapPart = tileMapPool.GetFromPool(NamePrefab, Vector3.zero, Quaternion.identity).GetComponent<MapInfo>();
    //            tileMapInfoList.Add(mapPart);
    //            mapPart.transform.position = new Vector3(nextPosition[indexPos].X * (scaleTile * cellToPos), 0, nextPosition[indexPos].Y * (scaleTile * cellToPos));
    //            mapPart.ChangeMaterialMap(LevelManager.Instance.ChapterList[LevelManager.Instance.IndexCurrentChap].Map.materialRoad);
    //            mapPart.CurrentPosition = nextPosition[indexPos];
    //        }
    //    }
    //    //var mapPart = tileMapPool.GetFromPool(NamePrefab, Vector3.zero, Quaternion.identity).GetComponent<MapInfo>();
    //    //tileMapInfoList.Add(mapPart);
    //    //mapPart.transform.position = new Vector3(currentPos.X * (scaleTile * cellToPos * 2), 0, currentPos.Y * (scaleTile * cellToPos * 2));

    //    //mapPart.CurrentPosition = currentPos;
    //    firstLoad = false;
    //}
    public void StartToGenerateMap()
    {
        StopAllCoroutines();
        DisableAllTile();
        //Awake();
        StartCoroutine(GenerateMap());
    }

    IEnumerator GenerateMap()
    {
        while (true)
        {
            //while(tileMapPool.activeObjects[NamePrefab].Count == activeTileCount)
            //{
                ReturnTileToPool();
            //}
            //yield return new WaitUntil(() => tileMapPool.activeObjects[NamePrefab].Count < activeTileCount);
            if (tileMapPool.activeObjects[NamePrefab].Count < activeTileCount)
            {
                currentPos = CalculateCurrentPosition();
                var nextPosition = CalculateNextPositions();

                for (int indexPos = 0; indexPos < nextPosition.Count; indexPos++)
                {
                    if (!tileMapInfoList.Any(x => x.CurrentPosition == nextPosition[indexPos]))
                    {
                        //Debug.Log("1111");
                        var mapPart = tileMapPool.GetFromPool(NamePrefab, Vector3.zero, Quaternion.identity).GetComponent<MapInfo>();
                        tileMapInfoList.Add(mapPart);
                        mapPart.transform.position = new Vector3(nextPosition[indexPos].X * (scaleTile * cellToPos), 0, nextPosition[indexPos].Y * (scaleTile * cellToPos));
                        mapPart.ChangeMaterialMap(LevelManager.Instance.ChapterList[LevelManager.Instance.IndexCurrentChap].Map.materialRoad);
                        mapPart.CurrentPosition = nextPosition[indexPos];
                    }
                }
            }
            yield return null;
        }
    }

    void ReturnTileToPool()
    {
        //Debug.Log("3333");
        for (int indexTile = 0; indexTile < tileMapInfoList.Count; indexTile++)
        {
            Int2 distance = tileMapInfoList[indexTile].CurrentPosition.Value - CalculateCurrentPosition();
            if (Mathf.Abs(distance.X) > 1 || Mathf.Abs(distance.Y) > 1 /*|| (Mathf.Abs(distance.X) >= 1 && Mathf.Abs(distance.Y) >= 1)*/)
            {
                //Debug.Log("2222");
                tileMapInfoList[indexTile].CurrentPosition = null;
                tileMapPool.ReturnToPool(NamePrefab, tileMapInfoList[indexTile].gameObject);
                tileMapInfoList.RemoveAt(indexTile);
            }
        }
    }
    void DisableAllTile()
    {
        foreach(var item in tileMapInfoList)
        {
            tileMapPool.ReturnToPool(NamePrefab, item.gameObject);
        }
        tileMapInfoList.Clear();
    }
    List<Int2> CalculateNextPositions()
    {
        var x = (_playerTransForm.position.x + ((scaleTile * cellToPos) / 2)) / (scaleTile * cellToPos);
        var y = (_playerTransForm.position.z + ((scaleTile * cellToPos) / 2)) / (scaleTile * cellToPos);

        var currentPosition = CalculateCurrentPosition();
        List<Int2> nextPosition = new List<Int2>();
        nextPosition = GetNineNextPosition(currentPosition, x, y);

        return nextPosition;
    }

    Int2 CalculateCurrentPosition()
    {
        Int2 position = new Int2
        {
            X = Mathf.FloorToInt((_playerTransForm.position.x + ((scaleTile * cellToPos) / 2)) / (scaleTile * cellToPos)),
            Y = Mathf.FloorToInt((_playerTransForm.position.z + ((scaleTile * cellToPos) / 2)) / (scaleTile * cellToPos))
        };
        return position;
    }
    List<Int2> GetNineNextPosition(Int2 currentValue, float x, float y)
    {
        List<Int2> nextPosition = new List<Int2>();
        for(int indexCoefficient = 0; indexCoefficient < _coefficientAround.Count; indexCoefficient++)
        {
            Int2 newPos = currentValue + _coefficientAround[indexCoefficient];
            nextPosition.Add(newPos);
        }

        return nextPosition;
    }

    //int horizontalX, verticalY;
    //List<Int2> GetThreeNextPosition(Int2 currentValue, float x, float y)
    //{
    //    List<Int2> nextPosition = new List<Int2>();
    //    bool horizontal = x < 0;
    //    bool vertical = y < 0;

    //    var decimalPartOfHorizontalAbsVal = Mathf.Abs(x) - (int)Mathf.Abs(x);
    //    var decimalPartOfVerticalAbsVal = Mathf.Abs(y) - (int)Mathf.Abs(y);
    //    horizontalX = 0;
    //    verticalY = 0;
    //    if (decimalPartOfHorizontalAbsVal > _safeAreaMax)
    //    {
    //        horizontalX = horizontal ? currentValue.X - 1 : currentValue.X + 1;
    //    }
    //    else if (decimalPartOfHorizontalAbsVal < _safeAreaMin)
    //    {
    //        horizontalX = horizontal ? currentValue.X + 1 : currentValue.X - 1;
    //    }

    //    if (decimalPartOfVerticalAbsVal > _safeAreaMax)
    //    {
    //        verticalY = horizontal ? currentValue.Y - 1 : currentValue.Y + 1;
    //    }
    //    else if (decimalPartOfVerticalAbsVal < _safeAreaMin)
    //    {
    //        verticalY = horizontal ? currentValue.Y + 1 : currentValue.Y - 1;
    //    }
    //    Debug.Log("Horizontal: " + horizontalX + " && " + "Vertical: " + verticalY);
    //    Int2 pos1 = new(currentValue.X + horizontalX, currentValue.Y);
    //    Int2 pos2 = new(currentValue.X, currentValue.Y + verticalY);
    //    Int2 pos3 = new(currentValue.X + horizontalX, currentValue.Y + verticalY);
    //    //Debug.Log("Horizontal: " + decimalPartOfHorizontalAbsVal + " && "+ "Vertical: " + decimalPartOfVerticalAbsVal);
    //    Debug.Log("CurrentValue: " + currentValue);
    //    nextPosition.Add(pos1);
    //    nextPosition.Add(pos2);
    //    nextPosition.Add(pos3);
    //    for (int indexPos = 0; indexPos < nextPosition.Count; indexPos++)
    //    {
    //        Debug.Log("Next " + indexPos + " position : " + nextPosition[indexPos]);
    //    }
    //    //Debug.Log("Pos " + result);
    //    return nextPosition;
    //}

    //int GetFixedCalculatedNextPosition(int currentValue, float val)
    //{
    //    bool negative = val < 0;

    //    var decimalPartOfAbsVal = Mathf.Abs(val) - (int)Mathf.Abs(val);

    //    int retVal = currentValue; //return current value if user in safe area

    //    //if user not in safe area
    //    if (decimalPartOfAbsVal > _safeAreaMax)
    //    {
    //        //make its absolute value bigger
    //        retVal = negative
    //            ? currentValue - 1
    //            : currentValue + 1;
    //    }
    //    else if (decimalPartOfAbsVal < _safeAreaMin)
    //    {
    //        //Debug.Log("Min:" + _safeAreaMin + " && " + "Max:" + _safeAreaMax);
    //        //make its absolute value smaller
    //        retVal = negative
    //            ? currentValue + 1
    //            : currentValue - 1;
    //    }
    //    return retVal;
    //}
}
