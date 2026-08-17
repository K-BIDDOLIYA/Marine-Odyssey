using UnityEngine;

public class StarfishSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject starfishPrefab;

    [SerializeField] private KrakenEvent krakenEvent;

    [Header("Spawn Area")]
    [SerializeField] private float spawnX = 18f;
    [SerializeField] private float minY = -3f;
    [SerializeField] private float maxY = 3f;

    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnInterval = 1.5f;
    [SerializeField] private float maxSpawnInterval = 4f;

    [Header("Spawn Chance")]
    [Range(0, 1)]
    [SerializeField] private float doubleSpawnChance = 0.3f;
    [SerializeField] private float minMoveSpeed = 10f;
    [SerializeField] private float maxMoveSpeed = 16f;

    private float timer;
    private float nextSpawn;
    private float moveSpeed;

    private void Awake()
    {
        krakenEvent = FindFirstObjectByType<KrakenEvent>();
    }

    private void Start()
    {
        nextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < nextSpawn)
            return;

        timer = 0f;
        nextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);

        if (krakenEvent != null && krakenEvent.IsKrakenActive)
            return;

        SpawnStarfish();
    }

    private void SpawnStarfish()
    {
        SpawnSingleStarfish();

        if (krakenEvent != null &&
            krakenEvent.IsKrakenActive)
        {
            return;
        }
    }

    private void SpawnSingleStarfish()
    {
        float y = Random.Range(minY, maxY);
        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);

        Instantiate(
            starfishPrefab,
            new Vector3(spawnX, y, 0),
            Quaternion.identity
        );
    }
}
