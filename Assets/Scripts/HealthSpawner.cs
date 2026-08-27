using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    public GameObject healthPrefab;

    public float spawnTime = 5f;

    public float minX = 10f;
    public float maxX = 20f;

    public float minY = -3f;
    public float maxY = 3f;

    public float minSpeed = 2f;
    public float maxSpeed = 5f;

    void Start()
    {
        InvokeRepeating("Spawn", 1f, spawnTime);
    }

    void Spawn()
    {
        if (healthPrefab == null)
            return;

        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);

        GameObject obj = Instantiate(
            healthPrefab,
            new Vector3(x, y, 0),
            Quaternion.identity
        );

        HealthBoost health = obj.GetComponent<HealthBoost>();

        if (health != null)
            health.speed = Random.Range(minSpeed, maxSpeed);
    }
}
