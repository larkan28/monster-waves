using UnityEngine;

public class SimpleController : EnemyController
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;

    internal override void Init(EnemyActor enemyActor)
    {
        base.Init(enemyActor);

        if (target == null)
        {
            Actor player = ActorManager.Instance.Find("Player");

            if (player != null)
                target = player.transform;
        }
    }

    internal override void ThinkFixed()
    {
        if (target != null)
        {
            Move();
            Rotate();
        }
    }

    private void Move()
    {
        Vector2 velocity = (target.position - transform.position).normalized;
        m_bodyController.Velocity = velocity * moveSpeed;
    }

    private void Rotate()
    {
        float targetX = target.position.x;
        float enemyX = transform.position.x;

        m_spriteRenderer.flipX = (targetX < enemyX);
    }
}
