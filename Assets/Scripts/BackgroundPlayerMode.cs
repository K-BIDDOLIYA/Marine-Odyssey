using UnityEngine;

public class BackgroundPlayerMode : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;
    private Collider2D[] colliders;
    private Behaviour[] behaviours;

    private Rigidbody2D rb;

    private VerticalCameraFollow cameraFollow;

    private void Awake()
    {
        spriteRenderers =
            GetComponentsInChildren<SpriteRenderer>(true);

        colliders =
            GetComponentsInChildren<Collider2D>(true);

        behaviours =
            GetComponents<Behaviour>();

        rb =
            GetComponent<Rigidbody2D>();

        cameraFollow =
            FindFirstObjectByType<VerticalCameraFollow>();
    }

    // =========================================================
    // ENABLE BACKGROUND MODE
    // =========================================================

    public void EnableBackgroundMode()
    {
        // -----------------------------------------------------
        // STOP SUBMARINE PHYSICS
        // -----------------------------------------------------

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            // Completely stop Rigidbody2D simulation.
            rb.simulated = false;
        }

        // -----------------------------------------------------
        // FREEZE CAMERA
        // -----------------------------------------------------

        if (cameraFollow != null)
        {
            cameraFollow.EnableBackgroundMode();
        }

        // -----------------------------------------------------
        // HIDE SUBMARINE VISUALS
        // -----------------------------------------------------

        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.enabled = false;
        }

        // -----------------------------------------------------
        // DISABLE SUBMARINE COLLIDERS
        // -----------------------------------------------------

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        // -----------------------------------------------------
        // DISABLE PLAYER SCRIPTS
        // -----------------------------------------------------

        foreach (Behaviour behaviour in behaviours)
        {
            if (behaviour == this)
                continue;

            behaviour.enabled = false;
        }

        // -----------------------------------------------------
        // CURSOR
        // -----------------------------------------------------

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // =========================================================
    // DISABLE BACKGROUND MODE
    // =========================================================

    public void DisableBackgroundMode()
    {
        // -----------------------------------------------------
        // RESTORE PHYSICS
        // -----------------------------------------------------

        if (rb != null)
        {
            rb.simulated = true;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // -----------------------------------------------------
        // RESTORE CAMERA
        // -----------------------------------------------------

        if (cameraFollow != null)
        {
            cameraFollow.DisableBackgroundMode();
        }

        // -----------------------------------------------------
        // SHOW SUBMARINE
        // -----------------------------------------------------

        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.enabled = true;
        }

        // -----------------------------------------------------
        // RESTORE COLLIDERS
        // -----------------------------------------------------

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }

        // -----------------------------------------------------
        // RESTORE PLAYER SCRIPTS
        // -----------------------------------------------------

        foreach (Behaviour behaviour in behaviours)
        {
            if (behaviour == this)
                continue;

            behaviour.enabled = true;
        }

        // -----------------------------------------------------
        // CURSOR
        // -----------------------------------------------------

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
