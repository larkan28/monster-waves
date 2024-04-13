using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private bool spawnEnemies;
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Transform playerPrefab;
    [SerializeField] private Health[] enemyPrefabs;
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private float enemyMaxHp;
    [SerializeField] private float enemySpawnCooldown;

    private float m_spawnCooldown;
    private Transform m_playerActor;
    private GameManager m_gameManager;

    private void Awake()
    {
        m_gameManager = GameManager.Instance;
    }

    private void Update()
    {
        if (m_gameManager.CurrStatus != GameManager.Status.Playing || m_playerActor == null)
            return;

        m_spawnCooldown -= Time.deltaTime;

        if (m_spawnCooldown <= 0f)
            SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (!spawnEnemies)
            return;

        float difficulty = (m_gameManager.GameTimer / 60f);
        float health = 0.5f + (difficulty * 0.5f);

        int maxEnemies = Mathf.RoundToInt(Random.Range(1, 3) * Mathf.Max(difficulty, 1f));

        for (int i = 0; i < maxEnemies; i++)
        {
            Vector2 origin = RandomSpawnPoint();
            CreateEnemy(origin, health, Mathf.RoundToInt(difficulty));
        }

        float cooldown = enemySpawnCooldown - (difficulty * 0.1f);
        m_spawnCooldown = Mathf.Clamp(cooldown, 0.1f, enemySpawnCooldown);
    }

    private void CreateEnemy(Vector2 origin, float hp, int level)
    {
        Health enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], origin, Quaternion.identity);

        if (enemy != null)
        {
            EnemyActor enemyActor = enemy.Owner as EnemyActor;

            enemyActor.Level = level;
            enemy.SetHealth(Mathf.Clamp(enemy.DefaultHp * hp, 1f, enemyMaxHp));
        }
    }

    private Vector2 RandomSpawnPoint()
    {
        float radius = Random.Range(minRadius, maxRadius);
        float degress = Random.Range(0f, 360f);

        return m_playerActor.position + Util.PointByRadius(radius, degress);
    }

    public void SpawnPlayer()
    {
        if (m_playerActor == null)
            m_playerActor = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }
}
