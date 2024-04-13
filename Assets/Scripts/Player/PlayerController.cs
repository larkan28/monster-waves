using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameEvent gameEvent;

    public Vector2 Direction
    {
        get
        {
            Vector2 dir;

            if (m_moveInput == Vector2.zero)
                dir = Util.FacingDirection(m_spriteRenderer);
            else
                dir = m_moveInput;

            return dir;
        }
    }
    public Vector2 MoveInput => m_moveInput;
    public SpriteRenderer SpriteRenderer => m_spriteRenderer;

    private float m_moveSpeed;
    private Vector2 m_moveInput;

    private Animator m_animator;
    private BodyController m_bodyController;
    private SpriteRenderer m_spriteRenderer;

    private void OnEnable()
    {
        gameEvent.OnAttributeChanged += RefreshSpeed;
    }

    private void OnDisable()
    {
        gameEvent.OnAttributeChanged -= RefreshSpeed;
    }

    internal void Init(PlayerActor playerActor)
    {
        m_bodyController = GetComponent<BodyController>();
        m_bodyController.Init();
        m_animator = GetComponentInChildren<Animator>();
        m_spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_moveSpeed = moveSpeed;
    }

    internal void Think()
    {
        m_animator.SetFloat("Speed", Mathf.Clamp01(m_bodyController.Velocity.magnitude));
        m_moveInput.x = Input.GetAxisRaw("Horizontal");
        m_moveInput.y = Input.GetAxisRaw("Vertical");
        m_bodyController.Think();

        Rotate();
    }

    internal void ThinkFixed()
    {
        Move();
    }

    private void Move()
    {
        m_bodyController.Velocity = m_moveInput.normalized * m_moveSpeed;
    }

    private void Rotate()
    {
        float speedX = m_moveInput.x;

        if (speedX != 0f)
            m_spriteRenderer.flipX = speedX < 0f;
    }

    private void RefreshSpeed(PlayerStats playerStats)
    {
        m_moveSpeed = moveSpeed * playerStats.GetBonus("speed");
    }
}
