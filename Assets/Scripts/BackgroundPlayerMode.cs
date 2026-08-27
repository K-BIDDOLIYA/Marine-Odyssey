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

    public void EnableBackgroundMode()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        if (cameraFollow != null)
        {
            cameraFollow.EnableBackgroundMode();
        }

        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.enabled = false;
        }

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }

        foreach (Behaviour behaviour in behaviours)
        {
            if (behaviour == this)
                continue;

            behaviour.enabled = false;
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisableBackgroundMode()
    {
        if (rb != null)
        {
            rb.simulated = true;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (cameraFollow != null)
        {
            cameraFollow.DisableBackgroundMode();
        }

        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.enabled = true;
        }

        foreach (Collider2D collider in colliders)
        {
            collider.enabled = true;
        }

        foreach (Behaviour behaviour in behaviours)
        {
            if (behaviour == this)
                continue;

            behaviour.enabled = true;
        }
    }
}
