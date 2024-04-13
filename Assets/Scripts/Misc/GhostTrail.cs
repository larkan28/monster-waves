using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GhostTrail : MonoBehaviour
{
    [SerializeField] private float followSpeed;
    [SerializeField] private bool updateSpriteFromParent;
    [SerializeField] private bool updateFlipFromParent;

    private Vector3 m_offset;
    private Transform m_target;
    private SpriteRenderer m_parentSprite;
    private SpriteRenderer m_spriteRenderer;

    void Start()
    {
        m_parentSprite = transform.parent.GetComponent<SpriteRenderer>();

        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.sprite = m_parentSprite.sprite;

        m_offset = transform.localPosition;
        m_target = transform.parent;

        transform.parent = null;
    }

    void Update()
    {
        if (m_target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (updateSpriteFromParent && m_parentSprite.sprite != m_spriteRenderer.sprite)
            m_spriteRenderer.sprite = m_parentSprite.sprite;

        if (updateFlipFromParent && m_parentSprite.flipX != m_spriteRenderer.flipX)
            m_spriteRenderer.flipX = m_parentSprite.flipX;

        Vector3 target = m_target.position + m_offset;
        Vector3 smooth = Vector3.Slerp(transform.position, target, followSpeed * Time.deltaTime);

        transform.SetPositionAndRotation(smooth, m_target.rotation);
    }
}
