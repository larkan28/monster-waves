using System.Collections;
using UnityEngine;

public class AbilityGeyser : Ability
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float minCreateTime;
    [SerializeField] private float maxCreateTime;

    public override void Think()
    {
        if (m_cooldownTime > 0)
        {
            m_cooldownTime -= Time.deltaTime;
            return;
        }

        StopAllCoroutines();
        StartCoroutine(CR_CreateGeyser());

        m_cooldownTime = Cooldown;
    }

    private IEnumerator CR_CreateGeyser()
    {
        for (int i = 0; i < ProjectileCount; i++)
        {
            CreateGeyser();
            yield return new WaitForSeconds(Random.Range(minCreateTime, maxCreateTime));
        }
    }

    private void CreateGeyser()
    {
        Vector3 position = Util.RandomPointInCamera(Camera.main);
        Projectile geyser = Instantiate(projectilePrefab, position, Quaternion.identity);

        if (geyser != null)
        {
            geyser.Owner = m_owner;
            geyser.Damage = Damage;
            geyser.Duration = Duration;
            geyser.MaxHits = ProjectileHits;
            geyser.Size = Range;
            geyser.CriticalChance = CriticalChance;
            geyser.CriticalDamage = CriticalDamage;

            PlaySound();
        }
    }
}
