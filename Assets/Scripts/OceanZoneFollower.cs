using UnityEngine;

public class OceanZoneFollower : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    private void LateUpdate()
    {
        if (targetCamera == null)
            return;

        transform.position = new Vector3(
            targetCamera.transform.position.x,
            targetCamera.transform.position.y,
            0f
        );
    }
}
