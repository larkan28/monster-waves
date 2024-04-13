using UnityEngine;

public class Attribute : IReward
{
    public int Level => m_level;
    public bool IsLevelMax => (m_level >= m_data.MaxLevel);
    public AttributeData Data => m_data;

    private readonly AttributeData m_data;
    private int m_level;

    public Attribute(AttributeData data)
    {
        m_data = data;
        m_level = 0;
    }

    public void LevelUp()
    {
        if (m_level < m_data.MaxLevel)
            m_level++;
    }

    public override bool Equals(object other)
    {
        if (other != null && other is Attribute obj)
            return m_data.Equals(obj.Data);

        return false;
    }

    public override int GetHashCode()
    {
        return m_data.GetHashCode();
    }

    public override string ToString()
    {
        return m_data.Tag;
    }

    public float GetBonus(int level)
    {
        return m_data.StartBonus + (m_data.BonusPerLevel * level);
    }

    public float GetBonus()
    {
        return m_data.StartBonus + (m_data.BonusPerLevel * m_level);
    }

    #region INTERFACE
    public string GetName()
    {
        return m_data.Name;
    }

    public string GetDescription(int level)
    {
        if (level == 0)
            return m_data.Description;

        int nextLevel = level + 1;
        float bonus = GetBonus(nextLevel) - GetBonus();

        if (m_data.IsPercent)
            bonus *= 100.0f;

        return "Level " + level + " > " + nextLevel + "\n" + m_data.Name + ": " + Util.BonusToStr(bonus) + (m_data.IsPercent ? "%" : "");
    }

    public Sprite GetIcon()
    {
        return m_data.Icon;
    }

    public int GetLevel()
    {
        return m_level;
    }
    #endregion
}
