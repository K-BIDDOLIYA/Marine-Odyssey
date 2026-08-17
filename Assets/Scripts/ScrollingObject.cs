using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    [SerializeField] private OceanScroller oceanScroller;

    private void Update()
    {
        if (oceanScroller == null)
            return;

        transform.position += oceanScroller.GetMovementThisFrame();
    }
}
