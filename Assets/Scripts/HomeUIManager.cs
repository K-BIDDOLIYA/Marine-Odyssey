using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeUIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;

    [Header("Buttons")]
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private Toggle controllerModeToggle;

    [Header("Volume Sliders")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";
    private const string CONTROLLER_MODE_KEY = "ControllerMode";

    private void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        if (settingsButton != null)
            settingsButton.SetActive(true);

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

        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.SetMusicVolume(musicVolume);
            GameAudioManager.Instance.SetSFXVolume(sfxVolume);
        }

        int controllerMode =
            PlayerPrefs.GetInt(CONTROLLER_MODE_KEY, 0);

        if (controllerModeToggle != null)
        {
            controllerModeToggle.SetIsOnWithoutNotify(
                controllerMode == 1
            );

            controllerModeToggle.onValueChanged.AddListener(
                SetControllerMode
            );
        }
    }

    public void OpenGameScene()
    {
        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlayStartGame();

        Time.timeScale = 1f;

        SceneManager.LoadScene("Game");
    }

    public void OpenSettings()
    {
        if (settingsPanel == null)
            return;

        settingsPanel.SetActive(true);

        if (settingsButton != null)
            settingsButton.SetActive(false);

        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlaySettingsOpen();
    }

    public void CloseSettings()
    {
        if (settingsPanel == null)
            return;

        settingsPanel.SetActive(false);

        if (settingsButton != null)
            settingsButton.SetActive(true);

        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlaySettingsClose();
    }

    public void PlayButtonSound()
    {
        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlayButtonClick();
    }

    public void SetMusicVolume(float value)
    {
        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.SetMusicVolume(value);

        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.SetSFXVolume(value);

        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, value);
        PlayerPrefs.Save();
    }

    private void OnDestroy()
    {
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.RemoveListener(SetSFXVolume);
    }

    public void SetControllerMode(bool cursorMode)
    {
        PlayerPrefs.SetInt(
            CONTROLLER_MODE_KEY,
            cursorMode ? 1 : 0
        );

        PlayerPrefs.Save();
    }
}

