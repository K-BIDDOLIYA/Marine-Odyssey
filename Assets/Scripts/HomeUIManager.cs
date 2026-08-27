using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeUIManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject settingsButton;

    public Toggle controllerToggle;

    public Slider musicSlider;
    public Slider sfxSlider;

    const string musicKey = "MusicVolume";
    const string sfxKey = "SFXVolume";
    const string controllerKey = "ControllerMode";

    void Start()
    {
        settingsPanel.SetActive(false);

        float music = PlayerPrefs.GetFloat(musicKey, 1f);
        float sfx = PlayerPrefs.GetFloat(sfxKey, 1f);
        int controller = PlayerPrefs.GetInt(controllerKey, 0);

        musicSlider.value = music;
        sfxSlider.value = sfx;
        controllerToggle.isOn = controller == 1;

        musicSlider.onValueChanged.AddListener(SetMusic);
        sfxSlider.onValueChanged.AddListener(SetSFX);
        controllerToggle.onValueChanged.AddListener(SetController);

        if (GameAudioManager.Instance != null)
        {
            GameAudioManager.Instance.SetMusicVolume(music);
            GameAudioManager.Instance.SetSFXVolume(sfx);
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlayStartGame();

        SceneManager.LoadScene("Game");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        settingsButton.SetActive(false);

        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlaySettingsOpen();
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        settingsButton.SetActive(true);

        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlaySettingsClose();
    }

    public void ButtonSound()
    {
        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.PlayButtonClick();
    }

    public void SetMusic(float value)
    {
        PlayerPrefs.SetFloat(musicKey, value);
        PlayerPrefs.Save();

        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.SetMusicVolume(value);
    }

    public void SetSFX(float value)
    {
        PlayerPrefs.SetFloat(sfxKey, value);
        PlayerPrefs.Save();

        if (GameAudioManager.Instance != null)
            GameAudioManager.Instance.SetSFXVolume(value);
    }

    public void SetController(bool value)
    {
        PlayerPrefs.SetInt(controllerKey, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    void OnDestroy()
    {
        musicSlider.onValueChanged.RemoveListener(SetMusic);
        sfxSlider.onValueChanged.RemoveListener(SetSFX);
        controllerToggle.onValueChanged.RemoveListener(SetController);
    }
}

