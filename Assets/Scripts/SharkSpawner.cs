using UnityEngine;

public class SharkSpawner : MonoBehaviour
{
    [SerializeField] private GameObject sharkPrefab;

    [SerializeField] private float spawnX = 18f;

    [SerializeField] private float openOceanMinY = -2f;
    [SerializeField] private float openOceanMaxY = 2f;

    [SerializeField] private float spawnInterval = 7f;

    [SerializeField] private int maxSharks = 2;
    [SerializeField] private float firstSpawnMin = 1f;
    [SerializeField] private float firstSpawnMax = 3f;

    [SerializeField] private KrakenEvent krakenEvent;
    [SerializeField] private SeaMineEvent seaMineEvent;

    private void Start()
    {
        timer = Random.Range(firstSpawnMin, firstSpawnMax);
    }

    private float timer;

    private void Awake()
    {
        krakenEvent = FindFirstObjectByType<KrakenEvent>();
        Debug.Log(krakenEvent);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < spawnInterval)
            return;

        timer = Random.Range(-2f, 1f);

        int sharkCount =
            FindObjectsByType<SharkEnemy>(
                FindObjectsSortMode.None
            ).Length;

        if (sharkCount >= maxSharks)
            return;

        Spawn();
    }

    private void Spawn()
    {
        if ((krakenEvent != null &&
            krakenEvent.IsKrakenActive) ||
            (seaMineEvent != null &&
            seaMineEvent.IsMineEventActive))
        {
            return;
        }
        
        float y =
            Random.Range(
                openOceanMinY,
                openOceanMaxY
            );
        if (FindObjectsByType<SharkEnemy>(FindObjectsSortMode.None).Length >= 6)
            return;

        Instantiate(
            sharkPrefab,
            new Vector3(spawnX, y, 0),
            Quaternion.identity
        );
    }
}

