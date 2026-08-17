using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    [Header("Health Boost")]
    public int healthAmount = 300;

    [Header("Movement")]
    public float speed = 3f;

    private void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x <= -20f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SubmarineHealth submarineHealth = collision.GetComponent<SubmarineHealth>();

            if (submarineHealth != null)
            {
                submarineHealth.Heal(healthAmount);

                if (GameAudioManager.Instance != null)
                {
                    GameAudioManager.Instance.PlayHeal();
                }
            }

            Destroy(gameObject);
        }
    }
}
