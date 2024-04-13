using UnityEngine;

public class EnemyActor : Actor
{
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private EnemyHurt enemyHurt;

    public int Level;

    public override void OnSpawn()
    {
        enemyController.Init(this);
        enemyHurt.Init(this);
    }

    private void Update()
    {
        if (!m_health.IsAlive)
            return;

        enemyController.Think();
    }

    private void FixedUpdate()
    {
        if (!m_health.IsAlive)
            return;

        enemyController.ThinkFixed();
    }
}
