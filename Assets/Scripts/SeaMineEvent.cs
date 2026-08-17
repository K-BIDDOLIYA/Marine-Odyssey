using System.Collections;
using UnityEngine;

public class SeaMineEvent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SeaMineSpawner seaMineSpawner;
    [SerializeField] private KrakenEvent krakenEvent;

    [Header("Timing")]
    [SerializeField] private float minCooldown = 25f;
    [SerializeField] private float maxCooldown = 35f;

    [SerializeField] private float warningDuration = 2f;

    [SerializeField] private float eventDuration = 12f;

    public bool IsMineEventActive { get; private set; }

    private void Awake()
    {
        if (seaMineSpawner == null)
            seaMineSpawner = FindFirstObjectByType<SeaMineSpawner>();

        if (krakenEvent == null)
            krakenEvent = FindFirstObjectByType<KrakenEvent>();
    }

    private void Start()
    {
        StartCoroutine(EventLoop());
    }

    private IEnumerator EventLoop()
    {
        while (true)
        {
            float cooldown =
                Random.Range(minCooldown, maxCooldown);

            yield return new WaitForSeconds(cooldown);

            while (krakenEvent != null &&
                   krakenEvent.IsKrakenActive)
            {
                yield return null;
            }

            yield return StartCoroutine(StartMineField());
        }
    }

    private IEnumerator StartMineField()
    {
        if (krakenEvent != null &&
            krakenEvent.IsKrakenActive)
        {
            yield break;
        }

        IsMineEventActive = true;

        FindFirstObjectByType<GameUIManager>()
            .ShowWarning("⚠ SEA MINES AHEAD");

        yield return new WaitForSeconds(warningDuration);

        // Kraken may have started during the warning.
        if (krakenEvent != null &&
            krakenEvent.IsKrakenActive)
        {
            IsMineEventActive = false;
            yield break;
        }

        seaMineSpawner.BeginMineField();

        yield return new WaitForSeconds(eventDuration);

        seaMineSpawner.EndMineField();

        IsMineEventActive = false;
    }

    public void CancelMineField()
    {
        IsMineEventActive = false;

        if (seaMineSpawner != null)
            seaMineSpawner.EndMineField();
    }

}
