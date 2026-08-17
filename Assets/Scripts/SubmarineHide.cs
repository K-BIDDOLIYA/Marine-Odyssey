using UnityEngine;

public class SubmarineHide : MonoBehaviour
{
    [Header("Hide Zone")]
    [SerializeField] private string hideZoneLayerName = "HideZone";

    [Header("Debug (Read Only)")]
    [SerializeField] private bool isHidden;

    private int hideZoneCount;
    private int hideZoneLayer;

    public bool IsHidden => isHidden;

    private void Awake()
    {
        hideZoneLayer = LayerMask.NameToLayer(hideZoneLayerName);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != hideZoneLayer)
            return;

        hideZoneCount++;

        UpdateHiddenState();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != hideZoneLayer)
            return;

        hideZoneCount = Mathf.Max(0, hideZoneCount - 1);

        UpdateHiddenState();
    }

    private void UpdateHiddenState()
    {
        isHidden = hideZoneCount > 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isHidden ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
