using UnityEngine;

public class SubmarineHide : MonoBehaviour
{
    public string hideLayer = "HideZone";

    [SerializeField] bool hidden;

    int zones;
    int layer;

    public bool IsHidden => hidden;

    void Awake()
    {
        layer = LayerMask.NameToLayer(hideLayer);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != layer)
            return;

        zones++;
        UpdateHide();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != layer)
            return;

        zones--;

        if (zones < 0)
            zones = 0;

        UpdateHide();
    }

    void UpdateHide()
    {
        hidden = zones > 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = hidden ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
