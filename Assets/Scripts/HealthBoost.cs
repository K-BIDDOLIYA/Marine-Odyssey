using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    public int health = 300;
    public float speed = 3f;

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        if (transform.position.x <= -20f)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player"))
            return;

        SubmarineHealth player = col.GetComponent<SubmarineHealth>();

        if (player != null)
        {
            player.Heal(health);

            if (GameAudioManager.Instance != null)
                GameAudioManager.Instance.PlayHeal();
        }

        Destroy(gameObject);
    }
}
