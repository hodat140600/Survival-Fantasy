using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SpinMovement : MonoBehaviour
{
    private string[] _listItemIgnore;
    [SerializeField] private float _degreesPerSecond = 350.0f;
    public bool isActive;

    public void Init(string[] listItemIgnore)
    {
        isActive = true;
        _listItemIgnore = listItemIgnore;

        MainThreadDispatcher.StartUpdateMicroCoroutine(Run());
    }

    IEnumerator Run()
    {
        while (isActive)
        {
            for (int i = 0; i < RewardSpawner.Instance.listReward.Count; i++)
            {

                if (RewardManager.Instance.IsStaticItem(RewardSpawner.Instance.listReward[i].name))
                {
                    continue;
                }

                // Spin object around Y-Axis
                RewardSpawner.Instance.listReward[i].gameObjectPrefab.transform.GetChild(0).Rotate(new Vector3(0f, Time.deltaTime * _degreesPerSecond, 0f), Space.World);
            }

            yield return null;
        }
    }
}
