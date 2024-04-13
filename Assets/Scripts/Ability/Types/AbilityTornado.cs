using System.Collections;
using UnityEngine;

public class AbilityTornado : Ability
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float radiusDirection;
    [SerializeField] private float launchCooldown;

    public override void Think()
    {
        if (m_cooldownTime > 0)
        {
            m_cooldownTime -= Time.deltaTime;
            return;
        }

        StopAllCoroutines();
        StartCoroutine(CR_CreateTornados());

        m_cooldownTime = Cooldown;
    }

    private IEnumerator CR_CreateTornados()
    {
        for (int i = 0; i < ProjectileCount; i++)
        {
            CreateTornado();
            yield return new WaitForSeconds(launchCooldown);
        }
    }

    private void CreateTornado()
    {
        Projectile tornado = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        if (tornado != null)
        {
            PlaySound();

            tornado.Owner = m_owner;
            tornado.Damage = Damage;
            tornado.Duration = Duration;
            tornado.MaxHits = ProjectileHits;
            tornado.Size = Range;
            tornado.CriticalChance = CriticalChance;
            tornado.CriticalDamage = CriticalDamage;

            float speed = ProjectileSpeed * Random.Range(1f, 2f);
            Vector3 direction = Util.PointByRadius(radiusDirection, Random.Range(0, 360f)) - transform.position;
            
            tornado.Rigidbody2D.velocity = direction.normalized * speed;
        }
    }
}
