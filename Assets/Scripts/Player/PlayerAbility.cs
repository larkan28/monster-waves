using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    [SerializeField] private Ability[] prefabs;
    [SerializeField] private GameEvent gameEvent;

    public readonly List<Ability> Abilities = new();
    public bool CanAddAbilities => Abilities.Count < 5;

    private PlayerActor m_playerActor;

    internal void Init(PlayerActor playerActor)
    {
        m_playerActor = playerActor;

        foreach (var ability in prefabs)
            Add(ability);
    }

    internal void Think()
    {
        foreach (var ability in Abilities)
            ability.Think();
    }

    public void Add(Ability abilityPrefab)
    {
        if (abilityPrefab == null)
            return;

        Ability ability = Abilities.Find(x => x.Equals(abilityPrefab));

        if (ability != null)
            ability.LevelUp();
        else
        {
            ability = Instantiate(abilityPrefab, transform);

            if (ability != null)
            {
                ability.Init(m_playerActor);
                Abilities.Add(ability);
            }
        }

        gameEvent.UpdateAbility(this);
    }
}
