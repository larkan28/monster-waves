using System.Collections;
using UnityEngine;

public class AbilityWater : Ability
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float findRadius;
    [SerializeField] private int findTeamId;

    public override void Think()
    {
        if (m_cooldownTime > 0)
        {
            m_cooldownTime -= Time.deltaTime;
            return;
        }

        StopAllCoroutines();
        StartCoroutine(CR_CreateWater());
        
        m_cooldownTime = Cooldown;
    }

    private IEnumerator CR_CreateWater()
    {
        for (int i = 0; i < ProjectileCount; i++)
        {
            CreateWaterBall();
            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
    }

    private void CreateWaterBall()
    {
        Vector3 origin = transform.position;
        Actor victim = ActorManager.Instance.Find(origin, findRadius, findTeamId, true);

        if (victim == null)
            return;

        Projectile water = Instantiate(projectilePrefab, origin, Quaternion.identity);

        if (water != null)
        {
            water.Owner = m_owner;
            water.Damage = Damage;
            water.Duration = Duration;
            water.MaxHits = ProjectileHits;
            water.Size = Range;
            water.CriticalChance = CriticalChance;
            water.CriticalDamage = CriticalDamage;

            Vector3 direction = victim.transform.position - origin;

            water.Rigidbody2D.velocity = direction.normalized * ProjectileSpeed;
            water.transform.rotation = Util.RotationFromDirection(direction);

            PlaySound();
        }
    }
}
