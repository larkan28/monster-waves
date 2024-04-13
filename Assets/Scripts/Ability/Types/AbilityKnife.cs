using System.Collections;
using UnityEngine;

public class AbilityKnife : Ability
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float throwCooldown;
    [SerializeField] private float minHeight;
    [SerializeField] private float maxHeight;

    private Vector2 m_lastDirection;
    private PlayerController m_playerController;

    public override void Init(Actor owner)
    {
        base.Init(owner);
        m_playerController = owner.GetComponent<PlayerController>();
        m_lastDirection = m_playerController.Direction;
    }

    public override void Think()
    {
        Vector2 direction = m_playerController.MoveInput;

        if (direction != Vector2.zero)
            m_lastDirection = direction;

        if (m_cooldownTime > 0)
        {
            m_cooldownTime -= Time.deltaTime;
            return;
        }

        StopAllCoroutines();
        StartCoroutine(CR_CreateKnifes());
        
        m_cooldownTime = Cooldown;
    }

    private IEnumerator CR_CreateKnifes()
    {
        for (int i = 0; i < ProjectileCount; i++)
        {
            Vector3 offset = new(Random.Range(minHeight, maxHeight), Random.Range(minHeight, maxHeight), 0f);
            Vector2 position = transform.position + offset;

            Projectile knife = Instantiate(projectilePrefab, position, Util.RotationFromDirection(m_lastDirection));

            if (knife != null)
            {
                knife.Owner = m_owner;
                knife.Damage = Damage;
                knife.Duration = Duration;
                knife.MaxHits = ProjectileHits;
                knife.Rigidbody2D.velocity = m_lastDirection * ProjectileSpeed;
                knife.Size = Range;
                knife.CriticalChance = CriticalChance;
                knife.CriticalDamage = CriticalDamage;

                PlaySound();
            }

            yield return new WaitForSeconds(throwCooldown);
        }
    }
}
