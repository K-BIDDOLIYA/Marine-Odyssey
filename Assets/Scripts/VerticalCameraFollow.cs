using UnityEngine;

public class VerticalCameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Follow")]
    [SerializeField] private float smoothTime = 0.18f;

    [Header("Camera Y Limits")]
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;

    [Header("Shake")]
    [SerializeField] private float shakeMagnitude = 0.08f;

    private int activeShakes = 0;

    private float velocityY;

    // Background mode
    private bool backgroundMode = false;
    private float backgroundY;

    public void StartShake()
    {
        activeShakes++;
    }

    public void StopShake()
    {
        activeShakes = Mathf.Max(0, activeShakes - 1);
    }

    // =========================================================
    // BACKGROUND MODE
    // =========================================================

    public void EnableBackgroundMode()
    {
        backgroundMode = true;

        // Completely freeze the camera's Y position
        // at the position it currently has.
        backgroundY = transform.position.y;

        // Stop SmoothDamp from trying to move the camera.
        velocityY = 0f;
    }

    public void DisableBackgroundMode()
    {
        backgroundMode = false;

        velocityY = 0f;
    }

    // =========================================================
    // CAMERA UPDATE
    // =========================================================

    private void LateUpdate()
    {
        // -----------------------------------------------------
        // BACKGROUND MODE
        // -----------------------------------------------------

        if (backgroundMode)
        {
            Vector3 fixedPosition = new Vector3(
                transform.position.x,
                backgroundY,
                transform.position.z
            );

            // Still allow camera shake during events.
            if (activeShakes > 0)
            {
                Vector2 offset =
                    Random.insideUnitCircle * shakeMagnitude;

                fixedPosition +=
                    new Vector3(offset.x, offset.y, 0f);
            }

            transform.position = fixedPosition;

            return;
        }

        // -----------------------------------------------------
        // NORMAL GAME MODE
        // -----------------------------------------------------

        if (target == null)
            return;

        float targetY = Mathf.Clamp(
            target.position.y,
            minY,
            maxY
        );

        float smoothY = Mathf.SmoothDamp(
            transform.position.y,
            targetY,
            ref velocityY,
            smoothTime
        );

        Vector3 finalPosition = new Vector3(
            transform.position.x,
            smoothY,
            transform.position.z
        );

        // Camera shake
        if (activeShakes > 0)
        {
            Vector2 offset =
                Random.insideUnitCircle * shakeMagnitude;

            finalPosition +=
                new Vector3(offset.x, offset.y, 0f);
        }

        transform.position = finalPosition;
    }
}

