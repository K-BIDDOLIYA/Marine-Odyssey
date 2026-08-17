using UnityEngine;

public class SeaMine : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private float bobHeight = 0.2f;

    [SerializeField] private float bobSpeed = 2f;

    [SerializeField] private float rotationAmount = 8f;

    [SerializeField] private float rotationSpeed = 1.5f;
    [SerializeField] private float moveSpeed = 6f;

    [SerializeField] private Transform submarine;

    private Vector3 startPosition;
    private float randomTimeOffset;

    private void Awake()
    {
        if (submarine == null)
            submarine = GameReferences.Instance.submarine;
    }

    private void Start()
    {
        randomTimeOffset = Random.Range(0f, 100f);
        startPosition = transform.position;

        transform.rotation =
            Quaternion.Euler(
                0,
                0,
                Random.Range(0f, 360f));
    }

    private void Update()
    {
        MoveLeft();
        Float();
        Rotate();
    }

    private void MoveLeft()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < submarine.position.x - 15f)
        {
            GameUIManager ui =
                FindFirstObjectByType<GameUIManager>();

            if (ui != null)
                ui.AddThreatScore(5);
                
            Destroy(gameObject);
        }
    }

    private void Float()
    {
        float offset = Mathf.Sin((Time.time + randomTimeOffset) * bobSpeed) * bobHeight;

        Vector3 pos = transform.position;
        pos.y = startPosition.y + offset;

        transform.position = pos;

        startPosition.x = transform.position.x;
    }

    private void Rotate()
    {
        float angle =
            Mathf.Sin(Time.time * rotationSpeed)
            * rotationAmount;

        transform.rotation =
            Quaternion.Euler(
                0,
                0,
                angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        SubmarineHealth health =
            other.GetComponent<SubmarineHealth>();

        if (health != null)
        {
            health.TakeDamage(500);
        }

        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.PlaySeaMineExplosion();
        }

        Destroy(gameObject);
    }
}
