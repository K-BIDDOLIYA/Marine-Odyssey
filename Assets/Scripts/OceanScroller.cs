using UnityEngine;

public class OceanScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] private float scrollSpeed = 3f;

    public float ScrollSpeed => scrollSpeed;

    public Vector3 GetMovementThisFrame()
    {
        return Vector3.left * scrollSpeed * Time.deltaTime;
    }
}
