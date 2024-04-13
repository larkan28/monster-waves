using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float health;

    public UnityEvent OnHealthChanged;
    public UnityEvent<Actor, Transform> OnDeath;
    public UnityEvent<Actor, Transform, float> OnTakeDamage;
    public UnityEvent<HealthEvent> OnTakeDamagePre;

    public readonly static int C_CONTINUE = 0;
    public readonly static int C_IGNORE = 1;

    public bool GodMode;
    public bool IsAlive => m_currHealth > 0f;
    public Actor Owner => m_owner;
    public float Hp => m_currHealth;
    public float MaxHp
    {
        get => m_maxHealth;
        set => m_maxHealth = value;
    }
    public float HpRatio => m_currHealth / m_maxHealth;
    public float DefaultHp => health;

    private float m_currHealth;
    private float m_maxHealth;
    private Actor m_owner;

    public void Init(Actor owner)
    {
        m_owner = owner;
        m_currHealth = m_maxHealth = health;
    }

    public bool TakeDamage(float damage, Actor attacker, Transform inflictor)
    {
        if (!IsAlive || GodMode || damage <= 0f)
            return false;

        if (m_owner.Team == attacker.Team || attacker == m_owner)
            return false;

        if (OnTakeDamagePre != null)
        {
            HealthEvent args = new(attacker, inflictor, damage);
            OnTakeDamagePre.Invoke(args);

            if (args.ReturnCode != C_CONTINUE)
                return false;

            damage = args.Damage;
        }

        m_currHealth -= damage;
        OnTakeDamage?.Invoke(attacker, inflictor, damage);

        if (m_currHealth <= 0f)
            Death(attacker, inflictor);

        return true;
    }

    public void Kill(Actor attacker, Transform inflictor)
    {
        if (IsAlive)
            Death(attacker, inflictor);
    }

    public void AddHealth(float amount)
    {
        if (!IsAlive)
            return;

        m_currHealth += amount;

        if (m_currHealth <= 0f)
        {
            Death(m_owner, transform);
            return;
        }

        if (m_currHealth > m_maxHealth)
            m_currHealth = m_maxHealth;

        OnHealthChanged?.Invoke();
    }

    public void SetHealth(float amount)
    {
        m_currHealth = amount;

        if (m_currHealth <= 0f)
        {
            Death(m_owner, transform);
            return;
        }

        if (m_currHealth > m_maxHealth)
            m_maxHealth = m_currHealth;

        OnHealthChanged?.Invoke();
    }

    private void Death(Actor attacker, Transform inflictor)
    {
        GodMode = false;
        m_currHealth = 0f;
        OnDeath?.Invoke(attacker, inflictor);
    }
}

public struct HealthEvent
{
    public int ReturnCode;
    public float Damage;
    public Actor Attacker;
    public Transform Inflictor;

    public HealthEvent(Actor attacker, Transform inflictor, float damage, int returnCode = 0)
    {
        ReturnCode = returnCode;
        Inflictor = inflictor;
        Attacker = attacker;
        Damage = damage;
    }
}
