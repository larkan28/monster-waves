using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Ability")]
public class AbilityData : ScriptableObject
{
    [Header("Data")]
    public string Tag;
    public string Name;
    public string Description;
    public Sprite Icon;
    public AudioClip Sound;
    public Ability Prefab;

    [Header("Stats")]
    public float Damage;
    public float Duration;
    public float Cooldown;
    public float Range;
    public int ProjectileCount;
    public int ProjectileHits;
    public float ProjectileSpeed;

    [Header("Upgrades")]
    public AbilityLevel[] Levels;

    public override string ToString()
    {
        return Tag;
    }

    public override bool Equals(object other)
    {
        if (other != null && other is AbilityData obj)
            return Tag.Equals(obj.Tag);

        return false;
    }

    public override int GetHashCode()
    {
        return Tag.GetHashCode();
    }
}

[System.Serializable]
public struct AbilityLevel
{
    public AttributeData Attribute;
    public float Bonus;

    public readonly string BonusText
    {
        get
        {
            if (Attribute.IsPercent)
                return Util.BonusToStr(Bonus * 100f) + "%";

            return Util.BonusToStr(Bonus);
        }
    }
}