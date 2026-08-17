using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class GameUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject deathPanel;

    [Header("Settings")]
    [SerializeField] private UnityEngine.UI.Slider musicVolumeSlider;
    [SerializeField] private UnityEngine.UI.Slider sfxVolumeSlider;
    [SerializeField] private UnityEngine.UI.Toggle controllerModeToggle;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string CONTROLLER_MODE_KEY = "ControllerMode";

    [Header("Buttons")]
    [SerializeField] private GameObject pauseButton;

    [Header("Death Panel")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text liveScoreText;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private UnityEngine.UI.Image healthFill;

    [SerializeField] private float scorePerSecond = 10f;

    [SerializeField] private TMP_Text warningText;

    private Coroutine warningRoutine;

    public bool IsPaused { get; private set; }
    public bool PlayerDead { get; private set; }
    public int CurrentScore { get; private set; }

    public bool IsBackgroundMode { get; private set; }

    private float scoreTimer;

    private void Start()
    {
        Time.timeScale = 1f;

        IsPaused = false;
        PlayerDead = false;
        IsBackgroundMode = false;

        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        deathPanel.SetActive(false);

        warningText.gameObject.SetActive(false);

        ApplyControllerCursor();

        float musicVolume =
            PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);

        float sfxVolume =
            PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.SetValueWithoutNotify(musicVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.SetValueWithoutNotify(sfxVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (controllerModeToggle != null)
        {
            controllerModeToggle.SetIsOnWithoutNotify(
                PlayerPrefs.GetInt(CONTROLLER_MODE_KEY, 0) == 1
            );

            controllerModeToggle.onValueChanged.AddListener(SetControllerMode);
        }
    }

    private void Update()
    {
        if (IsBackgroundMode)
            return;

        if (PlayerDead)
            return;

        UpdateScore();

        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (IsPaused)
                ResumeGame();
            else
                PauseGame();
        }

        KeepCursorVisible();
    }

    public void EnableBackgroundMode()
    {
        IsBackgroundMode = true;

        IsPaused = false;
        PlayerDead = false;

        Time.timeScale = 1f;

        // Hide all gameplay UI
        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);
        deathPanel.SetActive(false);
        pauseButton.SetActive(false);
        warningText.gameObject.SetActive(false);

        // Hide gameplay HUD
        if (healthFill != null)
            healthFill.gameObject.SetActive(false);

        if (liveScoreText != null)
            liveScoreText.gameObject.SetActive(false);

        // Home menu needs the cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void DisableBackgroundMode()
    {
        IsBackgroundMode = false;

        IsPaused = false;
        PlayerDead = false;

        Time.timeScale = 1f;

        pauseButton.SetActive(true);

        if (healthFill != null)
            healthFill.gameObject.SetActive(true);

        if (liveScoreText != null)
            liveScoreText.gameObject.SetActive(true);

        ApplyControllerCursor();
    }

    #region Pause

    public void PauseGame()
    {
        if (IsBackgroundMode)
            return;

        if (PlayerDead || IsPaused)
            return;

        IsPaused = true;

        pausePanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (IsBackgroundMode)
            return;

        if (PlayerDead)
            return;

        IsPaused = false;

        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        ApplyControllerCursor();

        Time.timeScale = 1f;
    }

    #endregion

    #region Settings

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);

        if (!PlayerDead)
            pausePanel.SetActive(true);
        else
            deathPanel.SetActive(true);
    }

    #endregion

    #region Death

    public void PlayerDied()
    {
        if (IsBackgroundMode)
            return;

        if (PlayerDead)
            return;

        PlayerDead = true;

        IsPaused = false;

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        pauseButton.SetActive(false);

        pausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        deathPanel.SetActive(true);

        scoreText.text = "Score : " + CurrentScore;

        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (CurrentScore > highScore)
        {
            highScore = CurrentScore;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        highScoreText.text = "High Score : " + highScore;
    }

    #endregion

    #region Score

    public void SetScore(int score)
    {
        CurrentScore = score;
    }

    private void UpdateScore()
    {
        if (IsBackgroundMode)
            return;

        if (PlayerDead || IsPaused)
            return;

        scoreTimer += Time.deltaTime;

        if (scoreTimer >= 1f)
        {
            scoreTimer -= 1f;

            CurrentScore += 1;

            liveScoreText.text = CurrentScore.ToString();
        }
    }

    #endregion

    #region Scene Buttons

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Home");
    }

    #endregion

    public void ShowWarning(string message)
    {
        if (warningRoutine != null)
            StopCoroutine(warningRoutine);

        warningRoutine =
            StartCoroutine(WarningRoutine(message));
    }

    private IEnumerator WarningRoutine(string message)
    {
        warningText.gameObject.SetActive(true);

        warningText.text = message;

        yield return new WaitForSecondsRealtime(2f);

        warningText.gameObject.SetActive(false);

        warningRoutine = null;
    }

    public void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        healthFill.fillAmount = currentHealth / maxHealth;

        if (healthText != null)
        {
            healthText.text = Mathf.RoundToInt(currentHealth) + " / " + Mathf.RoundToInt(maxHealth);
        }
    }

    public void AddThreatScore(int amount)
    {
        if (PlayerDead)
            return;

        CurrentScore += amount;

        liveScoreText.text = CurrentScore.ToString();
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat(
            MUSIC_VOLUME_KEY,
            value
        );

        PlayerPrefs.Save();

        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.SetMusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat(
            SFX_VOLUME_KEY,
            value
        );

        PlayerPrefs.Save();

        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.SetSFXVolume(value);
    }

    public void SetControllerMode(bool cursorMode)
    {
        PlayerPrefs.SetInt(
            CONTROLLER_MODE_KEY,
            cursorMode ? 1 : 0
        );

        PlayerPrefs.Save();

        ApplyControllerCursor();
    }

    private void ApplyControllerCursor()
    {
        bool cursorMode =
            PlayerPrefs.GetInt(
                CONTROLLER_MODE_KEY,
                0
            ) == 1;

        Cursor.visible = cursorMode;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void KeepCursorVisible()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
