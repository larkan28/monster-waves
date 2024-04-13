using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BodyController : MonoBehaviour
{
    public bool BlockVelocityX;
    public bool BlockVelocityY;

    private float m_blockDuration;
    private Rigidbody2D m_rigibody2D;

    public Vector2 Velocity
    {
        get => m_rigibody2D.velocity;
        set
        {
            Vector2 velocity = value;

            if (BlockVelocityX) velocity.x = m_rigibody2D.velocity.x;
            if (BlockVelocityY) velocity.y = m_rigibody2D.velocity.y;

            m_rigibody2D.velocity = velocity;
        }
    }
    public bool IsMoving => m_rigibody2D.velocity != Vector2.zero;

    public void Init()
    {
        m_rigibody2D = GetComponent<Rigidbody2D>();
    }

    public void Think()
    {
        if (m_blockDuration > 0f)
        {
            m_blockDuration -= Time.deltaTime;

            if (m_blockDuration <= 0f)
            {
                BlockVelocityX = false;
                BlockVelocityY = false;
            }
        }
    }

    public void AddForce(Vector2 force, float duration = 0f, bool removePreviousVelocity = false)
    {
        if (removePreviousVelocity)
            m_rigibody2D.velocity = Vector2.zero;

        if (duration > 0f)
            BlockVelocity(true, true, duration);

        m_rigibody2D.AddForce(force, ForceMode2D.Impulse);
    }

    public void BlockVelocity(bool axisX, bool axisY, float duration)
    {
        BlockVelocityX = axisX;
        BlockVelocityY = axisY;

        m_blockDuration = duration;
    }

    public void Stop()
    {
        m_rigibody2D.velocity = Vector2.zero;

        BlockVelocityX = true;
        BlockVelocityY = true;

        m_blockDuration = 0f;
    }
}
