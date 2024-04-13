using System.Collections;
using UnityEngine;

public class AbilityWhip : Ability
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float launchCooldown;
    [SerializeField] private float offsetWidth;
    [SerializeField] private float offsetHeight;

    private SpriteRenderer m_spriteRenderer;

    public override void Init(Actor owner)
    {
        base.Init(owner);
        m_spriteRenderer = owner.GetComponent<PlayerController>().SpriteRenderer;
    }

    public override void Think()
    {
        if (m_cooldownTime > 0)
        {
            m_cooldownTime -= Time.deltaTime;
            return;
        }

        StopAllCoroutines();
        StartCoroutine(CR_CreateWhip());

        m_cooldownTime = Cooldown;
    }

    private IEnumerator CR_CreateWhip()
    {
        float facingDir = Util.BoolToFloat101(m_spriteRenderer.flipX);

        for (int i = 0; i < ProjectileCount; i++)
        {
            float side = (i % 2) == 0 ? -facingDir : facingDir;

            float x = offsetWidth * side;
            float y = (i / 2) * offsetHeight;

            Vector3 offset = new(x, y, 0f);
            Vector3 position = transform.position + offset;

            Projectile whip = Instantiate(projectilePrefab, position, Quaternion.identity);

            if (whip != null)
            {
                whip.Owner = m_owner;
                whip.Damage = Damage;
                whip.Duration = Duration;
                whip.MaxHits = ProjectileHits;
                whip.Size = Range;
                whip.CriticalChance = CriticalChance;
                whip.CriticalDamage = CriticalDamage;

                if (whip.TryGetComponent(out SpriteRenderer spriteRenderer))
                    spriteRenderer.flipX = Util.FloatToBool(side);

                PlaySound();
            }

            yield return new WaitForSeconds(launchCooldown);
        }
    }
}
