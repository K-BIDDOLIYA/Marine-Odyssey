using UnityEngine;

public class VerticalCameraFollow : MonoBehaviour
{
    public Transform target;

    public float smooth = 0.18f;
    public float minY = -3f;
    public float maxY = 3f;
    public float shake = 0.08f;

    int shakes;
    float vel;

    bool bgMode;
    float bgY;

    public void StartShake()
    {
        shakes++;
    }

    public void StopShake()
    {
        shakes--;

        if (shakes < 0)
            shakes = 0;
    }

    public void EnableBackgroundMode()
    {
        bgMode = true;
        bgY = transform.position.y;
        vel = 0f;
    }

    public void DisableBackgroundMode()
    {
        bgMode = false;
        vel = 0f;
    }

    void LateUpdate()
    {
        if (bgMode)
        {
            Vector3 pos = transform.position;
            pos.y = bgY;

            if (shakes > 0)
            {
                Vector2 offset = Random.insideUnitCircle * shake;
                pos += new Vector3(offset.x, offset.y, 0f);
            }

            transform.position = pos;
            return;
        }

        if (target == null)
            return;

        float y = Mathf.Clamp(target.position.y, minY, maxY);

        y = Mathf.SmoothDamp(
            transform.position.y,
            y,
            ref vel,
            smooth
        );

        Vector3 newPos = transform.position;
        newPos.y = y;

        if (shakes > 0)
        {
            Vector2 offset = Random.insideUnitCircle * shake;
            newPos += new Vector3(offset.x, offset.y, 0f);
        }

        transform.position = newPos;
    }
}

