using UnityEngine;

public class AbilityEarthSpike : Ability
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Vector3 createOffset;

    private BodyController m_bodyController;

    public override void Think()
    {
        if (m_cooldownTime > 0)
        {
            m_cooldownTime -= Time.deltaTime;
            return;
        }

        if (m_bodyController.IsMoving)
            CreateSpike();

        m_cooldownTime = Cooldown;
    }

    public override void Init(Actor owner)
    {
        base.Init(owner);
        m_bodyController = owner.GetComponent<BodyController>();
    }

    private void CreateSpike()
    {
        Projectile spike = Instantiate(projectilePrefab, transform.position - createOffset, Quaternion.identity);

        if (spike != null)
        {
            spike.Owner = m_owner;
            spike.Damage = Damage;
            spike.Duration = Duration;
            spike.MaxHits = ProjectileHits;
            spike.Size = Range;
            spike.CriticalChance = CriticalChance;
            spike.CriticalDamage = CriticalDamage;
        }

        PlaySound();
    }
}
