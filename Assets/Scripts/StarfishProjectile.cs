using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StarfishProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float minRotationSpeed = 540f;
    [SerializeField] private float maxRotationSpeed = 1080f;


    [Header("Lifetime")]
    [SerializeField] private float destroyDistance = 3f;
    [SerializeField] private float minMoveSpeed = 10f;
    [SerializeField] private float maxMoveSpeed = 16f;

    private float moveSpeed;
    private Rigidbody2D rb;
    private Camera mainCamera;

    private float rotationSpeed;
    private void Awake()
    {
        rotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

        if (Random.value < 0.5f)
            rotationSpeed *= -1f;
          
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        Move();

        CheckLifetime();
    }

    private void Move()
    {
        Vector2 movement =
            Vector2.left *
            moveSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);

        rb.MoveRotation(
            rb.rotation +
            rotationSpeed *
            Time.fixedDeltaTime
        );
    }

    private void CheckLifetime()
    {
        float leftEdge =
            mainCamera.transform.position.x
            - mainCamera.orthographicSize
            * mainCamera.aspect;

        if (transform.position.x < leftEdge - destroyDistance)
        {
            GameUIManager ui =
                FindFirstObjectByType<GameUIManager>();

            if (ui != null)
                ui.AddThreatScore(1);

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        other.GetComponent<SubmarineHealth>()
            .TakeDamage(60);

        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.PlayStarfishHit();
        }
    }
}

