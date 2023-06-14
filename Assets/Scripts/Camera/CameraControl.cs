using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;

    public Vector3 cameraOffset;
    void Start()
    {
        cameraOffset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPos = player.transform.position + cameraOffset;
        transform.position = newPos;
    }
}
