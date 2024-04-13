using System;
using UnityEngine;

public class PlayerHurt : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Health health;
    [SerializeField] private float hurtForce;
    [SerializeField] private float hurtDamage;
    [SerializeField] private float hurtDuration;
    [SerializeField] private AudioClip hurtSound;

    private float m_regenHp;
    private float m_regenTime;
    private float m_damageReduction;
    private Animator m_animator;
    private PlayerActor m_playerActor;
    private AudioSource m_audioSource;

    internal void Init(PlayerActor playerActor)
    {
        m_regenHp = m_damageReduction = 0f;
        m_animator = GetComponentInChildren<Animator>();
        m_playerActor = playerActor;
        m_audioSource = GetComponent<AudioSource>();
    }

    internal void Think()
    {
        if (m_regenHp > 0f && health.Hp < health.MaxHp)
            RegenerateHp();
    }

    private void RegenerateHp()
    {
        m_regenTime += Time.deltaTime;

        if (m_regenTime > 1f)
        {
            health.AddHealth(m_regenHp);
            m_regenTime = 0f;

            gameEvent.UpdateHealth(health.HpRatio);
        }
    }

    private void OnEnable()
    {
        health.OnHealthChanged.AddListener(OnHealthChanged);
        health.OnTakeDamagePre.AddListener(OnTakeDamagePre);
        health.OnTakeDamage.AddListener(OnTakeDamage);
        health.OnDeath.AddListener(OnDeath);

        gameEvent.OnAttributeChanged += OnAttributeChanged;
    }

    private void OnDisable()
    {
        health.OnHealthChanged.RemoveListener(OnHealthChanged);
        health.OnTakeDamagePre.RemoveListener(OnTakeDamagePre);
        health.OnTakeDamage.RemoveListener(OnTakeDamage);
        health.OnDeath.RemoveListener(OnDeath);

        gameEvent.OnAttributeChanged -= OnAttributeChanged;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Health attacker))
            health.TakeDamage(hurtDamage, attacker.Owner, attacker.transform);
    }

    private void OnHealthChanged()
    {
        gameEvent.UpdateHealth(health.HpRatio);
    }

    private void OnTakeDamagePre(HealthEvent args)
    {
        if (m_damageReduction > 0f)
        {
            args.Damage *= Mathf.Clamp(1f - m_damageReduction, 0.1f, 1f);
            args.ReturnCode = Health.C_CONTINUE;
        }
    }

    private void OnTakeDamage(Actor attacker, Transform inflictor, float damage)
    {
        if (inflictor.TryGetComponent(out BodyController bodyController))
        {
            Vector2 force = (inflictor.position - transform.position).normalized;
            bodyController.AddForce(force * hurtForce, hurtDuration);
        }

        m_animator.SetTrigger("Hurt");
        m_audioSource.PlayOneShot(hurtSound);

        gameEvent.UpdateHealth(health.HpRatio);
    }

    private void OnDeath(Actor attacker, Transform inflictor)
    {
        m_animator.SetBool("IsDead", true);
        gameEvent.UpdateHealth(health.HpRatio);

        if (TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        if (TryGetComponent(out Collider2D collider))
            collider.enabled = false;

        ActorManager.Instance.Remove(m_playerActor);
        GameManager.Instance.GameOver();
    }

    private void OnAttributeChanged(PlayerStats stats)
    {
        m_regenHp = stats.GetBonus("hp_recovery");
        m_damageReduction = stats.GetBonus("armor");

        health.MaxHp = health.DefaultHp + stats.GetBonus("hp");
        gameEvent.UpdateHealth(health.HpRatio);
    }
}
