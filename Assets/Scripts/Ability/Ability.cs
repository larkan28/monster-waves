using UnityEngine;

public abstract class Ability : MonoBehaviour, IReward
{
    [SerializeField] protected AbilityData data;

    public AbilityData Data => data;
    public int CurrLevel => m_level;
    public bool IsLevelMax => m_level >= data.Levels.Length;

    protected int m_level;
    protected float m_cooldownTime;
    protected float m_durationTime;
    protected PlayerStats m_playerStats;
    protected ActorManager m_actorManager;
    protected AudioSource m_audioSource;
    protected Transform m_target;
    protected Actor m_owner;

    public float Damage => data.Damage * GetBonus("damage");
    public float Duration => data.Duration * GetBonus("duration");
    public float Cooldown => data.Cooldown * GetBonus("cooldown");
    public float Range => data.Range * GetBonus("area");
    public float CriticalDamage => GetBonus("crit_damage");
    public float CriticalChance => GetBonus("crit_chance");
    public int ProjectileCount => (int) (data.ProjectileCount + GetBonus("pro_count"));
    public int ProjectileHits => (int) (data.ProjectileHits + GetBonus("pro_hits"));
    public float ProjectileSpeed => data.ProjectileSpeed * GetBonus("pro_speed");

    public virtual void Init(Actor owner)
    {
        m_owner = owner;
        m_level = 1;
        m_audioSource = GetComponent<AudioSource>();
        m_playerStats = m_owner.GetComponent<PlayerStats>();
        m_actorManager = ActorManager.Instance;
        m_cooldownTime = 0f;
        m_durationTime = 0f;
    }

    public abstract void Think();

    public void LevelUp()
    {
        if (!IsLevelMax)
            m_level++;
    }

    public float GetBonus(string attributeTag)
    {
        float bonus = m_playerStats.GetBonus(attributeTag);
        int maxLoop = Mathf.Min((m_level - 1), data.Levels.Length);

        for (int i = 0; i < maxLoop; i++)
        {
            AbilityLevel level = data.Levels[i];

            if (level.Attribute.Tag.Equals(attributeTag))
                bonus += level.Bonus;
        }

        return bonus;
    }

    public override string ToString()
    {
        return Data.Tag;
    }

    public override bool Equals(object other)
    {
        if (other is not Ability item)
            return false;

        return item.Data.Equals(Data);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    protected void PlaySound()
    {
        m_audioSource.PlayOneShot(data.Sound);
    }

    #region INTERFACE
    public string GetName()
    {
        return data.Name;
    }

    public string GetDescription(int level)
    {
        if (level == 0)
            return data.Description;

        int nextLevel = level + 1;
        AbilityLevel levelBonus = data.Levels[level];

        return "Level " + level + " > " + nextLevel + "\n" + levelBonus.Attribute.Name + ": <color=blue>" + levelBonus.BonusText + "</color>";
    }

    public Sprite GetIcon()
    {
        return data.Icon;
    }

    public int GetLevel()
    {
        return m_level;
    }
    #endregion
}