using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected BodyController m_bodyController;
    protected SpriteRenderer m_spriteRenderer;

    internal virtual void Init(EnemyActor enemyActor)
    {
        m_bodyController = GetComponent<BodyController>();
        m_bodyController.Init();

        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    internal virtual void Think()
    {
        m_bodyController.Think();
    }

    internal virtual void ThinkFixed() { }
}
