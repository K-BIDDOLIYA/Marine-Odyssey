using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OceanScroller oceanScroller;

    [Header("Loop Settings")]
    [SerializeField] private float panelWidth = 18f;
    [SerializeField] private int totalPanels = 3;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (oceanScroller == null || mainCamera == null)
            return;

        Move();
        CheckForLoop();
    }

    private void Move()
    {
        transform.position += oceanScroller.GetMovementThisFrame();
    }

    private void CheckForLoop()
    {
        float cameraLeftEdge =
            mainCamera.transform.position.x
            - mainCamera.orthographicSize * mainCamera.aspect;

        float panelRightEdge =
            transform.position.x
            + panelWidth * 0.5f;

        if (panelRightEdge < cameraLeftEdge)
        {
            transform.position +=
                Vector3.right * panelWidth * totalPanels;
        }
    }
}
