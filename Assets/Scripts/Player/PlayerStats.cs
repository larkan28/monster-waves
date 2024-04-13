using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;

    public readonly List<Attribute> Attributes = new();
    public bool CanAddAttribute
    {
        get
        {
            int count = 0;

            foreach (var attribute in Attributes)
            {
                if (attribute.Level > 0)
                    count++;
            }

            return count < 5;
        }
    }

    internal void Init(PlayerActor playerActor)
    {
        foreach (var attribute in gameEvent.Attributes)
            Attributes.Add(new Attribute(attribute));
    }

    public void Upgrade(Attribute attribute)
    {
        Attribute existingAttribute = Attributes.Find(x => x.Equals(attribute));

        if (existingAttribute != null)
            existingAttribute.LevelUp();

        gameEvent.UpdateAttribute(this);
    }

    public float GetBonus(string attributeTag)
    {
        foreach (var attribute in Attributes)
        {
            if (attribute.Data.Tag.Equals(attributeTag))
                return attribute.GetBonus();
        }

        return 0f;
    }
}
