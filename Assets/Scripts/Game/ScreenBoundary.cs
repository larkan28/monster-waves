using UnityEngine;

public class ScreenBoundary : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boundLeft;
    [SerializeField] private BoxCollider2D boundRight;
    [SerializeField] private BoxCollider2D boundUp;
    [SerializeField] private BoxCollider2D boundDown;

    private Transform m_followTarget;

    private void Start() => SetBounds();

    private void Update()
    {
        if (m_followTarget != null)
            transform.position = m_followTarget.position;
    }

    public void SetBounds()
    {
        Bounds cameraBounds = Util.CameraBounds(Camera.main);

        float width = cameraBounds.size.x;
        float height = cameraBounds.size.y;

        float up = cameraBounds.extents.y;
        float left = cameraBounds.extents.x;

        boundLeft.offset = new Vector2(left + 0.5f, 0f);
        boundRight.offset = new Vector2(-left - 0.5f, 0f);
        boundUp.offset = new Vector2(0f, up + 0.5f);
        boundDown.offset = new Vector2(0f, -up - 0.5f);

        boundLeft.size = new Vector2(1f, height);
        boundRight.size = new Vector2(1f, height);
        boundUp.size = new Vector2(width, 1f);
        boundDown.size = new Vector2(width, 1f);

        Actor player = ActorManager.Instance.Find("Player");

        if (player != null)
            m_followTarget = Camera.main.transform;
    }
}
