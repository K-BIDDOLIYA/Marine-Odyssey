using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KrakenEvent : MonoBehaviour
{
    [Header("Cooldown")]
    [SerializeField] private float minimumCooldown = 90f;
    [SerializeField] private float maximumCooldown = 150f;

    [Header("Tentacles")]
    [SerializeField] private KrakenTentacle[] tentacles;

    [Header("Safe Gap")]
    [SerializeField] private float minimumSafeGap = 2.5f;
    [SerializeField] private float maximumSafeGap = 3.5f;

    [Header("Timing")]
    [SerializeField] private float warningDuration = 1.2f;
    [SerializeField] private float rotateDelay = 1f;
    [SerializeField] private float eventDuration = 5f;

    [Header("UI")]
    [SerializeField] private TMP_Text krakenWarning;
    [SerializeField] private Image oceanDarkener;
    [SerializeField] private float darknessAlpha = 0.45f;

    [Header("Attack Height")]
    [SerializeField] private float minimumTargetY = -0.5f;
    [SerializeField] private float maximumTargetY = 2.5f;

    public bool IsKrakenActive { get; private set; }
    private Vector3 originalCameraPosition;

    private float cooldownTimer;
    private float currentCooldown;
    private Camera mainCamera;
    private VerticalCameraFollow cameraFollow;
    private SeaMineEvent seaMineEvent;
    private bool oceanDarkened = false;

    private void Start()
    {
        mainCamera = Camera.main;
        cameraFollow = mainCamera.GetComponent<VerticalCameraFollow>();
        SetNextCooldown();
    }

    private void Awake()
    {
        seaMineEvent = FindFirstObjectByType<SeaMineEvent>();
    }

    private void Update()
    {
        if (IsKrakenActive)
            return;

        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= currentCooldown)
        {
            StartCoroutine(KrakenRoutine());
        }
    }

    private IEnumerator KrakenRoutine()
    {
        IsKrakenActive = true;
        ClearOtherThreats();

        oceanDarkened = true;

        FindFirstObjectByType<GameUIManager>()
            .ShowWarning("⚠ TITAN HAND");

        yield return new WaitForSeconds(warningDuration);

    krakenWarning.gameObject.SetActive(false);


        //--------------------------------------------------
        // Attack all tentacles together
        //--------------------------------------------------

        float topOfScreen =
            mainCamera.transform.position.y +
            mainCamera.orthographicSize;

        int[] attackOrder = GetRandomTentacleOrder();

        foreach (int index in attackOrder)
        {
            KrakenTentacle tentacle = tentacles[index];

            float distanceBelowTop =
                Random.Range(minimumTargetY, maximumTargetY);

            float targetY = Random.Range(minimumTargetY, maximumTargetY);

            float height = targetY - tentacle.transform.position.y;

            tentacle.Attack(height);

            yield return new WaitForSeconds(2.5f);
        }

        //--------------------------------------------------
        // Wait until every tentacle has finished extending
        //--------------------------------------------------

        yield return new WaitUntil(AllTentaclesExtended);

        yield return new WaitForSeconds(1f);

        foreach (KrakenTentacle tentacle in tentacles)
        {
            tentacle.RotateTentacle();
}

        //--------------------------------------------------
        // Wait before rotating
        //--------------------------------------------------

        yield return new WaitForSeconds(rotateDelay);

        //--------------------------------------------------
        // Rotate all together
        //--------------------------------------------------

        foreach (KrakenTentacle tentacle in tentacles)
        {
            tentacle.RotateTentacle();
        }

        //--------------------------------------------------
        // Let player navigate between them
        //--------------------------------------------------

        yield return new WaitForSeconds(eventDuration);

        //--------------------------------------------------
        // Retract together
        //--------------------------------------------------

        foreach (KrakenTentacle tentacle in tentacles)
        {
            tentacle.Retract();
        }

        yield return new WaitUntil(AllTentaclesHidden);

        RestoreOcean();

        oceanDarkened = false;

        IsKrakenActive = false;

        SetNextCooldown();
    }

    private bool AllTentaclesExtended()
    {
        foreach (KrakenTentacle tentacle in tentacles)
        {
            if (!tentacle.IsExtended)
                return false;
        }

        return true;
    }

    private bool AllTentaclesHidden()
    {
        foreach (KrakenTentacle tentacle in tentacles)
        {
            if (tentacle.IsExtended)
                return false;
        }

        return true;
    }

    private void darkenOcean()
    {
        
    }

    private void RestoreOcean()
    {
        Color c = oceanDarkener.color;
        c.a = 0f;
        oceanDarkener.color = c;
    }

    private void SetNextCooldown()
    {
        cooldownTimer = 0f;

        currentCooldown =
            Random.Range(minimumCooldown, maximumCooldown);
    }

    private int[] GetRandomTentacleOrder()
    {
        int[] order = new int[tentacles.Length];

        for (int i = 0; i < order.Length; i++)
            order[i] = i;

        for (int i = 0; i < order.Length; i++)
        {
            int randomIndex = Random.Range(i, order.Length);

            (order[i], order[randomIndex]) =
                (order[randomIndex], order[i]);
        }

        return order;
    }

    private void ClearOtherThreats()
    {
        if (seaMineEvent != null)
        {
            seaMineEvent.CancelMineField();
        }

        SharkEnemy[] sharks =
            FindObjectsByType<SharkEnemy>(
                FindObjectsSortMode.None);

        foreach (SharkEnemy shark in sharks)
        {
            if (shark != null)
                Destroy(shark.gameObject);
        }

        StarfishProjectile[] starfishes =
            FindObjectsByType<StarfishProjectile>(
                FindObjectsSortMode.None);

        foreach (StarfishProjectile starfish in starfishes)
        {
            if (starfish != null)
                Destroy(starfish.gameObject);
        }

        SeaMine[] mines =
            FindObjectsByType<SeaMine>(
                FindObjectsSortMode.None);

        foreach (SeaMine mine in mines)
        {
            if (mine != null)
                Destroy(mine.gameObject);
        }
    }
}
