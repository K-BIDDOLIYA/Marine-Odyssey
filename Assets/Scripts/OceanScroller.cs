using UnityEngine;

public class OceanScroller : MonoBehaviour
{
    public float speed = 3f;

    public float ScrollSpeed => speed;

    public Vector3 GetMovementThisFrame()
    {
        return Vector3.left * speed * Time.deltaTime;
    }
}

