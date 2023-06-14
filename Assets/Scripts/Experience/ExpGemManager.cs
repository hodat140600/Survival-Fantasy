using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UniRx;

public class ExpGemManager : Singleton<ExpGemManager>
{
    private const float POSITION_HEIGHT = 3.0f;
    private const float RISE_TIME = 0.5f;
    private const float PLAYER_SENSOR_AREA = 1.5f;
    private const float FPS_CONFIG = 0.02f;
    private Transform _destinationObject;

    private void Start()
    {
        _destinationObject = GameManager.Instance.playerTransform;
    }

    public void GetGemsNearByPlayer(float playerRadius)
    {
        if (ExpSpawner.Instance.GetExpGemsInactive() == null)
        {
            return;
        }

        foreach (ExperienceItem expGem in ExpSpawner.Instance.GetExpGemsInactive())
        {
            if (IsInRange(expGem.currentPosition, playerRadius))
            {
                Rise(expGem);
            }
        }
    }

    public void GetAllGems()
    {
        if (ExpSpawner.Instance.GetExpGemsInactive() == null)
        {
            return;
        }

        foreach (ExperienceItem expGem in ExpSpawner.Instance.GetExpGemsInactive())
        {
            Rise(expGem);
        }
    }


    private bool IsInRange(Vector3 expGemVector, float distance)
    {
        return Vector3.Distance(expGemVector, _destinationObject.position) < distance;
    }

    private void Rise(ExperienceItem expGem)
    {
        expGem.isActives = true;

        //* Rise exp gem to the air with POSITION_HEIGHT and RISE_TIME
        DOTween.To(() => expGem.currentPosition, position => expGem.currentPosition = position,
            new Vector3(expGem.currentPosition.x, POSITION_HEIGHT, expGem.currentPosition.z), RISE_TIME)
            .OnComplete(() =>
            {
                //* Start Curve movement
                StartCoroutine(MoveToPlayer(expGem));
                // MainThreadDispatcher.StartUpdateMicroCoroutine(MoveToPlayer(expGem));
            }
        );
    }

    float returnTime;

    ///<summary> The exp gem will move to player by time </summary>
    IEnumerator MoveToPlayer(ExperienceItem expGem)
    {
        float returnTime = 0.0f;

        while (true)
        {
            //* Get and calculate position 
            expGem.currentPosition = Calculate(returnTime, expGem.currentPosition, new Vector3(_destinationObject.position.x, 0, _destinationObject.position.z));

            //* if the exp gem is near player then disappear
            if (Vector3.Distance(expGem.currentPosition, _destinationObject.position) < PLAYER_SENSOR_AREA)
            {
                ExpSpawner.Instance.DestroyExpGem(expGem);
                MessageBroker.Default.Publish(new AddExpEvent() { addExp = expGem.expPoint });

                break;
            }

            returnTime = Mathf.Clamp(returnTime + FPS_CONFIG, 0, 1); //* condition 0 < t < 1
            yield return new WaitForSeconds(FPS_CONFIG);
            // returnTime = Mathf.Clamp(returnTime + Time.deltaTime, 0, 1); //* condition 0 < t < 1
            // yield return null;
        }
        yield return null;
    }

    public Vector3 Calculate(float t, Vector3 p0, Vector3 p1)
    {
        float u = 1 - t;

        return ((1 - t) * p0) + t * p1;
    }
}
