using System.Collections;
using UnityEngine;

public class AbilitySpear : Ability
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float findRadius;
    [SerializeField] private int findTeamId;

    public override void Think()
    {
        if (m_cooldownTime > 0f)
        {
            m_cooldownTime -= Time.deltaTime;
            return;
        }

        StopAllCoroutines();
        StartCoroutine(CR_CreateSpears());

        m_cooldownTime = Cooldown;
    }

    private IEnumerator CR_CreateSpears()
    {
        for (int i = 0; i < ProjectileCount; i++)
        {
            CreateSpear();
            yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));
        }
    }

    private void CreateSpear()
    {
        Vector3 position;
        Actor victim = m_actorManager.Find(transform.position, findRadius, findTeamId, true);

        if (victim != null)
            position = victim.transform.position;
        else
            position = Util.RandomPointInCamera(Camera.main);

        Projectile spear = Instantiate(projectilePrefab, position, Quaternion.identity);

        if (spear != null)
        {
            spear.Owner = m_owner;
            spear.Damage = Damage;
            spear.Duration = Duration;
            spear.MaxHits = ProjectileHits;
            spear.Size = Range;
            spear.CriticalChance = CriticalChance;
            spear.CriticalDamage = CriticalDamage;

            PlaySound();
        }
    }
}
