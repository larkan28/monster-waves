using System.Collections;
using UnityEngine;

public class AbilityAxe : Ability
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float minForceX;
    [SerializeField] private float maxForceX;
    [SerializeField] private float minTorque;
    [SerializeField] private float maxTorque;

    public override void Think()
    {
        if (m_cooldownTime > 0)
        {
            m_cooldownTime -= Time.deltaTime;
            return;
        }

        StopAllCoroutines();
        StartCoroutine(CR_ThrowAxe());

        m_cooldownTime = Cooldown;
    }

    IEnumerator CR_ThrowAxe()
    {
        for (int i = 0; i < ProjectileCount; i++)
        {
            ThrowAxe();
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
    }

    private void ThrowAxe()
    {
        Projectile axe = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        if (axe != null)
        {
            axe.Owner = m_owner;
            axe.Damage = Damage;
            axe.Duration = Duration;
            axe.MaxHits = ProjectileHits;
            axe.Size = Range;
            axe.CriticalChance = CriticalChance;
            axe.CriticalDamage = CriticalDamage;

            Vector2 direction = Vector2.up + new Vector2(Random.Range(minForceX, maxForceX), 0f);
            Vector2 velocity = direction * ProjectileSpeed;

            axe.Rigidbody2D.AddForce(velocity, ForceMode2D.Impulse);
            axe.Rigidbody2D.AddTorque(Random.Range(minTorque, maxTorque));
        }

        PlaySound();
    }
}
