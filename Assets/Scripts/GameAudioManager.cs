using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Music")]
    [SerializeField] private AudioClip backgroundMusic;

    [Header("Home UI Sounds")]
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip settingsOpenSound;
    [SerializeField] private AudioClip settingsCloseSound;
    [SerializeField] private AudioClip startGameSound;

    [Header("Player Sounds")]
    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private AudioClip healSound;

    [Header("Threat Sounds")]
    [SerializeField] private AudioClip sharkHitSound;
    [SerializeField] private AudioClip starfishHitSound;
    [SerializeField] private AudioClip seaMineExplosionSound;
    [SerializeField] private AudioClip tentacleHitSound;

    [Header("Event Sounds")]
    [SerializeField] private AudioClip krakenEventSound;
    [SerializeField] private AudioClip titanHandMovementSound;
    [SerializeField] private AudioClip titanHandRetractSound;

    [Header("Game Sounds")]
    [SerializeField] private AudioClip gameOverSound;

    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        float musicVolume =
            PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1f);

        float sfxVolume =
            PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);

        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        PlayMusic();
    }

    private void PlayMusic()
    {
        if (musicSource == null)
            return;

        if (backgroundMusic == null)
            return;

        musicSource.clip = backgroundMusic;
        musicSource.loop = true;

        if (!musicSource.isPlaying)
            musicSource.Play();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null)
            return;

        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, value);
        PlayerPrefs.Save();

        if (musicSource != null)
            musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, value);
        PlayerPrefs.Save();

        if (sfxSource != null)
            sfxSource.volume = value;
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClickSound);
    }

    public void PlaySettingsOpen()
    {
        PlaySFX(settingsOpenSound);
    }

    public void PlaySettingsClose()
    {
        PlaySFX(settingsCloseSound);
    }

    public void PlayStartGame()
    {
        PlaySFX(startGameSound);
    }

    public void PlayPlayerDeath()
    {
        PlaySFX(playerDeathSound);
    }

    public void PlaySharkHit()
    {
        PlaySFX(sharkHitSound);
    }

    public void PlayStarfishHit()
    {
        PlaySFX(starfishHitSound);
    }

    public void PlaySeaMineExplosion()
    {
        PlaySFX(seaMineExplosionSound);
    }

    public void PlayTentacleHit()
    {
        PlaySFX(tentacleHitSound);
    }

    public void PlayKrakenEvent()
    {
        PlaySFX(krakenEventSound);
    }

    public void PlayTitanHandMovement()
    {
        PlaySFX(titanHandMovementSound);
    }

    public void PlayTitanHandRetract()
    {
        PlaySFX(titanHandRetractSound);
    }

    public void PlayGameOver()
    {
        PlaySFX(gameOverSound);
    }

    public void PlayHeal()
    {
        if (healSound != null)
        {
            sfxSource.PlayOneShot(healSound);
        }
    }
}
