using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KrakenTentacle : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float tipOffset = 0.5f;
    [SerializeField] private float attackSpeed = 45f;
    [SerializeField] private float retractSpeed = 55f;

    [Header("Timing")]
    [SerializeField] private float tipPause = 0.3f;

    [Header("Rotation")]
    [SerializeField] private float minRotation = -30f;
    [SerializeField] private float maxRotation = 30f;
    [SerializeField] private float rotationSpeed = 90f;

    [SerializeField] private int attackCount = 3;
    [SerializeField] private float strikeDuration = 0.15f;
    [SerializeField] private float returnDuration = 0.2f;

    private Collider2D Collider2D;

    private Vector3 hiddenPosition;
    private Vector3 tipPosition;
    private Vector3 attackPosition;

    private VerticalCameraFollow cameraFollow;

    public bool IsExtended { get; private set; }

    private void Awake()
    {
        Collider2D = GetComponent<Collider2D>();

        hiddenPosition = transform.position;

        Collider2D.enabled = false;
        IsExtended = false;

        cameraFollow = Camera.main.GetComponent<VerticalCameraFollow>();
    }

    public void Attack(float height)
    {
        StopAllCoroutines();

        tipPosition = hiddenPosition + Vector3.up * tipOffset;
        attackPosition = hiddenPosition + Vector3.up * height;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        Collider2D.enabled = false;
        IsExtended = false;

        // ==========================================
        // STEP 1: Slowly come out of the ground
        // ==========================================

        cameraFollow.StartShake();

        while (Vector3.Distance(transform.position, tipPosition) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                tipPosition,
                attackSpeed * 0.35f * Time.deltaTime
            );

            yield return null;
        }

        transform.position = tipPosition;

        cameraFollow.StopShake();

        // Pause after first appearance
        yield return new WaitForSeconds(tipPause);


        // ==========================================
        // STEP 2: MOVE UP 3 TIMES
        // ==========================================

        for (int i = 0; i < attackCount; i++)
        {
            // Move from the tip position to the
            // randomized attack height
            cameraFollow.StartShake();

            while (Vector3.Distance(transform.position, attackPosition) > 0.02f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    attackPosition,
                    attackSpeed * Time.deltaTime
                );

                yield return null;
            }

            transform.position = attackPosition;

            cameraFollow.StopShake();

            // Small pause at the top of the strike
            yield return new WaitForSeconds(strikeDuration);


            // Don't return after the final movement.
            // The tentacle stays at its final height.
            if (i < attackCount - 1)
            {
                cameraFollow.StartShake();

                while (Vector3.Distance(transform.position, tipPosition) > 0.02f)
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        tipPosition,
                        retractSpeed * Time.deltaTime
                    );

                    yield return null;
                }

                transform.position = tipPosition;

                cameraFollow.StopShake();

                // Short pause before the next strike
                yield return new WaitForSeconds(returnDuration);
            }
        }


        // ==========================================
        // STEP 3: FINAL POSITION
        // ==========================================

        transform.position = attackPosition;

        Collider2D.enabled = true;
        IsExtended = true;
    }

    public void RotateTentacle()
    {
        StopCoroutine(nameof(RotateRoutine));
        StartCoroutine(RotateRoutine());
    }

    private IEnumerator RotateRoutine()
    {
        float targetAngle = Random.Range(minRotation, maxRotation);

        Quaternion targetRotation =
            Quaternion.Euler(0f, 0f, targetAngle);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.2f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime);

            yield return null;
        }

        transform.rotation = targetRotation;
    }

    public void Retract()
    {
        StopAllCoroutines();
        StartCoroutine(RetractRoutine());
    }

    private IEnumerator RetractRoutine()
    {
        Collider2D.enabled = false;

        cameraFollow.StartShake();

        while (Vector3.Distance(transform.position, hiddenPosition) > 0.02f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                hiddenPosition,
                retractSpeed * Time.deltaTime);

            yield return null;
        }

        cameraFollow.StopShake();

        transform.position = hiddenPosition;
        transform.rotation = Quaternion.identity;
        
        GameUIManager ui =
            FindFirstObjectByType<GameUIManager>();

        if (ui != null)
            ui.AddThreatScore(10);

        IsExtended = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Collider2D.enabled)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        SubmarineHealth health =
            collision.gameObject.GetComponent<SubmarineHealth>();

        if (health != null)
        {
            health.TakeDamage(800);
        }

        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.PlayTentacleHit();
        }
    }
}
