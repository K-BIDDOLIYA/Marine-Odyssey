using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    [Header("Health Boost")]
    public GameObject healthBoostPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 5f;

    [Header("X Position")]
    public float minX = 10f;
    public float maxX = 20f;

    [Header("Y Position")]
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Speed")]
    public float minSpeed = 2f;
    public float maxSpeed = 5f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnHealthBoost), 1f, spawnInterval);
    }

    void SpawnHealthBoost()
    {
        if (healthBoostPrefab == null)
            return;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        Vector3 spawnPosition = new Vector3(randomX, randomY, 0f);

        GameObject healthBoost = Instantiate(
            healthBoostPrefab,
            spawnPosition,
            Quaternion.identity
        );

        HealthBoost healthScript = healthBoost.GetComponent<HealthBoost>();

        if (healthScript != null)
        {
            healthScript.speed = Random.Range(minSpeed, maxSpeed);
        }
    }
}
