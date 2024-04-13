using System.Collections.Generic;
using UnityEngine;

public class AbilityFireShield : Ability
{
    [SerializeField] private Projectile projectilePrefab;

    private readonly List<Transform> m_fireBalls = new();
    private float m_angleZ;

    public override void Init(Actor owner)
    {
        base.Init(owner);

        m_fireBalls.Clear();
        m_angleZ = 0f;
    }

    public override void Think()
    {
        RotateShield();
        
        if (m_cooldownTime > 0f)
        {
            m_cooldownTime -= Time.deltaTime;
            return;
        }

        CreateShield();
    }

    private void RotateShield()
    {
        if (m_durationTime > 0f)
        {
            m_durationTime -= Time.deltaTime;

            if (m_durationTime <= 0f)
            {
                DestroyShield();
                return;
            }

            int count = m_fireBalls.Count;

            if (m_fireBalls.Count > 0)
            {
                m_angleZ += ProjectileSpeed * Time.deltaTime;

                if (m_angleZ > 360f)
                    m_angleZ = 0f;

                for (int i = 0; i < count; i++)
                {
                    float degress = (i * (360f / count)) + m_angleZ;

                    m_fireBalls[i].transform.localPosition = Util.PointByRadius(Range, degress);
                    m_fireBalls[i].transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
                }
            }
        }
    }

    private void DestroyShield()
    {
        foreach (var fireBall in m_fireBalls)
        {
            if (fireBall != null)
                Destroy(fireBall.gameObject);
        }

        m_fireBalls.Clear();
    }

    private void CreateShield()
    {
        DestroyShield();

        for (int i = 0; i < ProjectileCount; i++)
            CreateFireBall(i);

        m_durationTime = Duration;
        m_cooldownTime = Cooldown;
    }

    private void CreateFireBall(int id)
    {
        float degress = id * (360f / ProjectileCount);
        Vector3 position = Util.PointByRadius(Range, degress) + transform.position;
        Projectile projectile = Instantiate(projectilePrefab, position, Quaternion.identity, transform);

        if (projectile != null)
        {
            projectile.Owner = m_owner;
            projectile.Damage = Damage;
            projectile.CriticalChance = CriticalChance;
            projectile.CriticalDamage = CriticalDamage;

            m_fireBalls.Add(projectile.transform);
        }
    }
}
