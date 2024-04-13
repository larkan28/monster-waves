using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Attribute")]
public class AttributeData : ScriptableObject
{
    public string Tag;
    public string Name;
    public string Description;
    public Sprite Icon;
    public float StartBonus;
    public float BonusPerLevel;
    public int MaxLevel;
    public bool IsPercent;
    public bool IsUpgradeable;

    public override string ToString()
    {
        return Tag;
    }

    public override bool Equals(object other)
    {
        if (other != null && other is AttributeData obj)
            return Tag.Equals(obj.Tag);

        return false;
    }

    public override int GetHashCode()
    {
        return Tag.GetHashCode();
    }
}
