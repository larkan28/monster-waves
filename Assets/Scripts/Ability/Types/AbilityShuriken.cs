using System.Collections;
using UnityEngine;

public class AbilityShuriken : Ability
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float launchRadius;
    [SerializeField] private float launchCooldown;

    public override void Think()
    {
        if (m_cooldownTime > 0)
        {
            m_cooldownTime -= Time.deltaTime;
            return;
        }

        StopAllCoroutines();
        StartCoroutine(CR_ThrowShurikens());

        m_cooldownTime = Cooldown;
    }

    private IEnumerator CR_ThrowShurikens()
    {
        for (int i = 0; i < ProjectileCount; i++)
        {
            CreateShuriken();
            yield return new WaitForSeconds(launchCooldown);
        }
    }

    private void CreateShuriken()
    {
        Projectile shuriken = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        if (shuriken != null)
        {
            PlaySound();

            shuriken.Owner = m_owner;
            shuriken.Damage = Damage;
            shuriken.Duration = Duration;
            shuriken.MaxHits = ProjectileHits;
            shuriken.Size = Range;
            shuriken.CriticalChance = CriticalChance;
            shuriken.CriticalDamage = CriticalDamage;

            Vector3 target = Util.PointByRadius(launchRadius, Random.Range(0f, 360f));
            Vector3 direction = target - transform.position;

            shuriken.Rigidbody2D.velocity = direction.normalized * ProjectileSpeed;
        }
    }
}
