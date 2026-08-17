using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeBackgroundLoader : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Game";

    public static HomeUIManager Instance { get; private set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        StartCoroutine(LoadGameBackground());
    }

    private IEnumerator LoadGameBackground()
    {
        if (SceneManager.GetSceneByName(gameSceneName).isLoaded)
            yield break;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(
                gameSceneName,
                LoadSceneMode.Additive
            );

        while (!operation.isDone)
            yield return null;

        Scene gameScene =
            SceneManager.GetSceneByName(gameSceneName);

        if (!gameScene.IsValid())
            yield break;

        GameUIManager ui =
            FindFirstObjectByType<GameUIManager>();

        if (ui != null)
        {
            ui.EnableBackgroundMode();
        }

        HomeHideGamePlayer();
    }

    private void HomeHideGamePlayer()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning(
                "HomeBackgroundLoader: No object with Player tag found."
            );

            return;
        }

        BackgroundPlayerMode backgroundPlayer =
            player.GetComponent<BackgroundPlayerMode>();

        if (backgroundPlayer == null)
        {
            Debug.LogWarning(
                "HomeBackgroundLoader: Player does not have BackgroundPlayerMode."
            );

            return;
        }

        backgroundPlayer.EnableBackgroundMode();
    }
}
