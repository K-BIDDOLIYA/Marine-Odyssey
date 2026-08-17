using UnityEngine;

public class SubmarineHealth : MonoBehaviour
{
    [Header("Health")]

    [SerializeField]
    private int maxHealth = 1000;

    [SerializeField]
    private int lowHealthThreshold = 400;

    [Header("Low Health")]

    [SerializeField]
    private float slowMultiplier = 0.7f;

    [Header("Coral Healing")]

    [SerializeField] private int coralHealthDrain = 40;

    [SerializeField] private float coralDamageInterval = 4f;

    private int currentHealth;

    private float healTimer;

    private bool isDead;

    private bool wasSlow;

    private GameUIManager ui;

    private SubmarineController controller;

    private SubmarineHide hide;

    public int CurrentHealth => currentHealth;

    public int MaxHealth => maxHealth;

    private void Awake()
    {
        ui = FindFirstObjectByType<GameUIManager>();

        controller = GetComponent<SubmarineController>();

        hide = GetComponent<SubmarineHide>();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        ui.UpdateHealthUI(currentHealth, maxHealth);
    }

    private void Update()
    {
        if (isDead)
            return;

        HandleCoralHealthDrain();
        HandleLowHealth();
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0);

        ui.UpdateHealthUI(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead)
            return;

        currentHealth += amount;

        currentHealth = Mathf.Min(currentHealth, maxHealth);

        ui.UpdateHealthUI(currentHealth, maxHealth);
    }

    private void HandleCoralHealthDrain()
    {
        if (!hide.IsHidden)
        {
            healTimer = 0f;
            return;
        }

        healTimer += Time.deltaTime;

        if (healTimer >= coralDamageInterval)
        {
            healTimer = 0f;

            TakeDamage(coralHealthDrain);;
        }
    }

    private void HandleLowHealth()
    {
        bool lowHealth =
            currentHealth < lowHealthThreshold;

        if (lowHealth == wasSlow)
            return;

        wasSlow = lowHealth;

        if (lowHealth)
            controller.SpeedMultiplier = slowMultiplier;
        else
            controller.SpeedMultiplier = 1f;
    }

    private void Die()
    {
        isDead = true;

        ui.PlayerDied();
    }
}
