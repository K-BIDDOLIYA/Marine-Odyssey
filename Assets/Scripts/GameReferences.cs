using UnityEngine;

public class GameReferences : MonoBehaviour
{
    public static GameReferences Instance { get; private set; }

    [Header("Player")]
    public Transform submarine;

    public SubmarineHide submarineHide;

    [Header("World")]
    public OceanScroller oceanScroller;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
