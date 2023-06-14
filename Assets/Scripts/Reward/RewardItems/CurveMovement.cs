using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CurveMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition, _destinationPosition;
    private const float POSITION_HEIGHT = 3.0f;
    private const float FPS_CONFIG = 0.02f;
    [SerializeField] private GameObject _destinationObject;
    [SerializeField] private bool _isArrived;

    public bool IsArrived() => _isArrived;

    public void StartMove(GameObject gameObject)
    {
        _isArrived = false;
        _destinationObject = gameObject;

        Rise();
    }

    private void Rise()
    {
        _startPosition = transform.position;

        DOTween.To(() => transform.position, position => transform.position = position,
                    new Vector3(_startPosition.x, POSITION_HEIGHT, _startPosition.z), 0.5f)
                    .OnComplete(() =>
                    {
                        if (!_isArrived && gameObject.activeSelf)
                        {
                            StartCoroutine(MoveToPlayer());
                        }
                    }
                    );
    }

    IEnumerator MoveToPlayer()
    {
        float returnTime = 0.0f;

        while (!_isArrived)
        {
            _startPosition = gameObject.transform.position;
            _destinationPosition = _destinationObject.transform.position;

            //TODO tinh khoang cach xem gan player hay khong sao do destroy hay vi dung collision
            gameObject.transform.position = Calculate(returnTime, _startPosition, new Vector3(_destinationPosition.x, 0, _destinationPosition.z));

            returnTime = Mathf.Clamp(returnTime + FPS_CONFIG, 0, 1); //* condition 0 < t < 1
            yield return new WaitForSeconds(FPS_CONFIG);
        }
    }

    public Vector3 Calculate(float t, Vector3 p0, Vector3 p1)
    {
        float u = 1 - t;

        return ((1 - t) * p0) + t * p1;
    }

    private void OnEnable()
    {
        _isArrived = true;
    }

    private void OnDisable()
    {
        _isArrived = false;
    }
}
