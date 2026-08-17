using UnityEngine;

public class SharkEnemy : MonoBehaviour
{
    [Header("References")]
    private OceanScroller oceanScroller;
    private Transform submarine;
    private SubmarineHide submarineHide;
    private Camera mainCamera;

    [Header("Movement")]
    [SerializeField] private float patrolSpeed = 4f;
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float detectionDistance = 7f;
    private float currentPatrolSpeed;

    [Header("Lifetime")]
    [SerializeField] private float destroyDistance = 15f;
    private SubmarineHealth submarineHealth;
    private KrakenEvent krakenEvent;
    [SerializeField] private float hitCooldown = 2f;

    private float hitTimer;

    private Rigidbody2D rb;

    private GameUIManager gameUIManager;

    private void Awake()
    {
        currentPatrolSpeed =
            Random.Range(
                patrolSpeed - 0.5f,
                patrolSpeed + 0.5f);

        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        oceanScroller = GameReferences.Instance.oceanScroller;
        submarine = GameReferences.Instance.submarine;
        submarineHide = GameReferences.Instance.submarineHide;

        krakenEvent = FindFirstObjectByType<KrakenEvent>();

        submarineHealth =
            GameReferences.Instance.submarine
            .GetComponent<SubmarineHealth>();

        gameUIManager =
            FindFirstObjectByType<GameUIManager>();
    }

    private void FixedUpdate()
    {
        if (submarine == null)
            return;

        CheckLifetime();

        if (hitTimer > 0f)
        {
            hitTimer -= Time.fixedDeltaTime;
            Patrol();
            return;
        }

        // =====================================================
        // HOME BACKGROUND MODE
        // =====================================================
        // When the Game scene is running behind the Home menu,
        // sharks should NEVER chase the invisible submarine.
        // They simply continue patrolling.
        // =====================================================

        if (gameUIManager != null &&
            gameUIManager.IsBackgroundMode)
        {
            Patrol();
            return;
        }

        // =====================================================
        // KRAKEN EVENT
        // =====================================================

        if (krakenEvent != null &&
            krakenEvent.IsKrakenActive)
        {
            Patrol();
            return;
        }

        // =====================================================
        // HIDDEN SUBMARINE
        // =====================================================

        if (submarineHide != null &&
            submarineHide.IsHidden)
        {
            Patrol();
            return;
        }

        // =====================================================
        // NORMAL GAMEPLAY
        // =====================================================

        float distance =
            Vector2.Distance(
                transform.position,
                submarine.position
            );

        if (distance <= detectionDistance)
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    // =========================================================
    // PATROL
    // =========================================================

    private void Patrol()
    {
        if (rb == null)
            return;

        Vector2 movement =
            Vector2.left *
            (oceanScroller.ScrollSpeed + currentPatrolSpeed) *
            Time.fixedDeltaTime;

        rb.MovePosition(
            rb.position + movement
        );

        float targetRotation = 0f;

        rb.MoveRotation(
            Mathf.LerpAngle(
                rb.rotation,
                targetRotation,
                6f * Time.fixedDeltaTime
            )
        );
    }

    // =========================================================
    // CHASE
    // =========================================================

    private void Chase()
    {
        if (rb == null || submarine == null)
            return;

        Vector2 direction =
            (
                (Vector2)submarine.position -
                rb.position
            ).normalized;

        Vector2 movement =
            direction *
            chaseSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(
            rb.position + movement
        );

        float angle =
            Mathf.Atan2(
                direction.y,
                direction.x
            ) * Mathf.Rad2Deg;

        // Prevent the shark from turning upside down
        if (angle > 90f)
            angle -= 180f;
        else if (angle < -90f)
            angle += 180f;

        rb.MoveRotation(
            Mathf.LerpAngle(
                rb.rotation,
                angle,
                8f * Time.fixedDeltaTime
            )
        );
    }

    // =========================================================
    // REFERENCES
    // =========================================================

    public void SetReferences(
        OceanScroller oceanScroller,
        Transform submarine,
        SubmarineHide submarineHide
    )
    {
        this.oceanScroller = oceanScroller;
        this.submarine = submarine;
        this.submarineHide = submarineHide;
    }

    // =========================================================
    // LIFETIME
    // =========================================================

    private void CheckLifetime()
    {
        if (mainCamera == null)
            return;

        float leftEdge =
            mainCamera.transform.position.x
            - mainCamera.orthographicSize
            * mainCamera.aspect;

        if (transform.position.x <
            leftEdge - destroyDistance)
        {
            GameUIManager ui =
                FindFirstObjectByType<GameUIManager>();

            if (ui != null)
                ui.AddThreatScore(2);

            Destroy(gameObject);
        }
    }

    // =========================================================
    // PLAYER COLLISION
    // =========================================================

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        SubmarineHealth health =
            other.GetComponent<SubmarineHealth>();

        if (health != null)
        {
            health.TakeDamage(60);
        }

        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.PlaySharkHit();
        }

        hitTimer = hitCooldown;
    }
}

