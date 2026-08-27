using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StarfishProjectile : MonoBehaviour
{
    public float minRotSpeed = 540f;
    public float maxRotSpeed = 1080f;

    public float destroyDistance = 3f;

    public float minSpeed = 10f;
    public float maxSpeed = 16f;

    float speed;
    float rotSpeed;

    Rigidbody2D rb;
    Camera cam;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;

        speed = Random.Range(minSpeed, maxSpeed);
        rotSpeed = Random.Range(minRotSpeed, maxRotSpeed);

        if (Random.value < 0.5f)
            rotSpeed = -rotSpeed;
    }

    void FixedUpdate()
    {
        rb.MovePosition(
            rb.position + Vector2.left * speed * Time.fixedDeltaTime
        );

        rb.MoveRotation(
            rb.rotation + rotSpeed * Time.fixedDeltaTime
        );

        float edge = cam.transform.position.x -
                     cam.orthographicSize * cam.aspect;

        if (transform.position.x < edge - destroyDistance)
        {
            GameUIManager ui = FindFirstObjectByType<GameUIManager>();

            if (ui != null)
                ui.AddThreatScore(1);

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
            return;

        SubmarineHealth player = col.GetComponent<SubmarineHealth>();

        if (player != null)
            player.TakeDamage(60);

        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlayStarfishHit();
    }
}

