using UnityEngine;

public class EnemyHurt : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private float hurtForce;
    [SerializeField] private float hurtDuration;
    [SerializeField] private float destroyDuration;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private GameObject hpPrefab;
    [SerializeField] private GameObject[] expPrefab;
    [SerializeField] private int dropHpChance;

    private Animator m_animator;
    private EnemyActor m_enemyActor;
    private AudioSource m_audioSource;
    private BodyController m_bodyController;

    private readonly static int k_animHurt = Animator.StringToHash("Hurt");
    private readonly static int k_animIsDead = Animator.StringToHash("IsDead");

    private void OnEnable()
    {
        health.OnTakeDamage.AddListener(OnTakeDamage);
        health.OnDeath.AddListener(OnDeath);
    }

    private void OnDisable()
    {
        health.OnTakeDamage.RemoveListener(OnTakeDamage);
        health.OnDeath.RemoveListener(OnDeath);
    }

    internal void Init(EnemyActor enemyActor)
    {
        m_animator = GetComponent<Animator>();
        m_enemyActor = enemyActor;
        m_audioSource = GetComponent<AudioSource>();
        m_bodyController = GetComponent<BodyController>();
    }

    private void OnDeath(Actor attacker, Transform inflictor)
    {
        ActorManager.Instance.Remove(m_enemyActor);

        foreach (var collider in GetComponentsInChildren<Collider2D>())
            collider.enabled = false;

        if (Random.Range(0, dropHpChance) == 0)
            DropHealth();
        else
            DropExp();

        m_bodyController.Stop();
        m_animator.SetBool(k_animIsDead, true);

        Destroy(gameObject, destroyDuration);
    }

    private void OnTakeDamage(Actor attacker, Transform inflictor, float damage)
    {
        m_animator.SetTrigger(k_animHurt);

        m_audioSource.pitch = Random.Range(0.9f, 1.1f);
        m_audioSource.PlayOneShot(hurtSound);

        Vector3 direction = (transform.position - attacker.transform.position).normalized;
        m_bodyController.AddForce(direction * hurtForce, hurtDuration);
    }

    private void DropExp()
    {
        int randomExp = Random.Range(0, 5);
        int enemyLevel = m_enemyActor.Level;

        GameObject prefabExpSelected;

        if (randomExp != 0 || enemyLevel <= 3)
            prefabExpSelected = expPrefab[0];
        else
            prefabExpSelected = expPrefab[enemyLevel > 4 ? 2 : 1];

        Instantiate(prefabExpSelected, transform.position, Quaternion.identity);
    }

    private void DropHealth()
    {
        Instantiate(hpPrefab, transform.position, Quaternion.identity);
    }
}
