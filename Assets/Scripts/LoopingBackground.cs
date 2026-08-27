using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    public OceanScroller ocean;

    public float width = 18f;
    public int panels = 3;

    Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (ocean == null || cam == null)
            return;

        transform.position += ocean.GetMovementThisFrame();

        float left = cam.transform.position.x -
                     cam.orthographicSize * cam.aspect;

        float right = transform.position.x + width / 2f;

        if (right < left)
        {
            transform.position += Vector3.right * width * panels;
        }
    }
}
