using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{
    public Actor Owner;
    public float Damage;
    public float CriticalDamage;
    public float CriticalChance;

    public int MaxHits
    {
        get => m_maxHits;
        set
        {
            m_maxHits = value;
            m_currHits = 0;
        }
    }
    public float Size
    {
        set => transform.localScale = transform.localScale * value;
    }
    public float Duration
    {
        set => m_destroyCooldown = value;
    }

    public Rigidbody2D Rigidbody2D => m_rigidbody2D;

    protected int m_maxHits;
    protected int m_currHits;
    protected bool m_hasRemoved;
    protected float m_destroyCooldown;
    protected Rigidbody2D m_rigidbody2D;

    private void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        OnInit();
    }

    private void Update()
    {
        if (m_destroyCooldown > 0)
        {
            m_destroyCooldown -= Time.deltaTime;

            if (m_destroyCooldown <= 0f)
            {
                OnRemove();
                return;
            }
        }

        OnThink();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_hasRemoved || (m_maxHits > 0 && m_currHits >= m_maxHits))
            return;

        Health health = collider.GetComponent<Health>();

        if (health == null)
            return;

        int maxChance = Mathf.RoundToInt(CriticalChance * 100);
        int randomChance = Random.Range(0, 100);

        float finalDamage = Damage;

        if (randomChance <= maxChance)
            finalDamage *= CriticalDamage;
        
        if (!health.TakeDamage(finalDamage, Owner, transform))
            return;

        if (m_maxHits > 0 && (++m_currHits) >= m_maxHits)
            OnRemove();

        PlayerUI.Instance.ShowDamage(health.transform.position, finalDamage, finalDamage != Damage);
    }

    public void Remove()
    {
        m_hasRemoved = true;
        Destroy(gameObject);
    }

    protected virtual void OnRemove()
    {
        Remove();
    }

    protected abstract void OnInit();
    protected abstract void OnThink();
}
