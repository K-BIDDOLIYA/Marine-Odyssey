using System.Collections.Generic;
using UnityEngine;

public class SeaMineSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject seaMinePrefab;

    [SerializeField] private Transform submarine;

    [Header("Spawn Area")]
    [SerializeField] private float minSpawnX = 15f;
    [SerializeField] private float maxSpawnX = 30f;

    [SerializeField] private float minSpawnY = -3.5f;
    [SerializeField] private float maxSpawnY = 3.5f;

    [Header("Minefield")]
    [SerializeField] private int mineCount = 12;

    [SerializeField] private float minimumSpacing = 2.5f;

    private readonly List<GameObject> activeMines =
        new List<GameObject>();

    private void Awake()
    {
        if (submarine == null)
            submarine = GameReferences.Instance.submarine;
    }

    public void BeginMineField()
    {
        ClearMineField();

        int spawned = 0;
        int attempts = 0;

        while (spawned < mineCount && attempts < 500)
        {
            attempts++;

            Vector3 position =
                new Vector3(
                    submarine.position.x +
                    Random.Range(minSpawnX, maxSpawnX),

                    Random.Range(
                        minSpawnY,
                        maxSpawnY),

                    0);

            bool valid = true;

            foreach (GameObject existingMine in activeMines)
            {
                if (Vector2.Distance(
                        existingMine.transform.position,
                        position)
                    < minimumSpacing)
                {
                    valid = false;
                    break;
                }
            }

            if (!valid)
                continue;

            GameObject newMine =    
                Instantiate(
                    seaMinePrefab,
                    position,
                    Quaternion.identity);

            activeMines.Add(newMine);

            spawned++;
        }
    }

    public void EndMineField()
    {
        ClearMineField();
    }

    private void ClearMineField()
    {
        foreach (GameObject mine in activeMines)
        {
            if (mine != null)
                Destroy(mine);
        }

        activeMines.Clear();
    }
}
