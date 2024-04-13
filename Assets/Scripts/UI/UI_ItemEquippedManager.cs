using UnityEngine;

public class UI_ItemEquippedManager : MonoBehaviour
{
    [SerializeField] private UI_ItemEquipped itemPrefab;
    [SerializeField] private Transform rootPassives;
    [SerializeField] private Transform rootAbilities;
    [SerializeField] private GameEvent gameEvent;

    private UI_ItemEquipped[] m_passives;
    private UI_ItemEquipped[] m_abilities;

    private void Awake()
    {
        m_passives = new UI_ItemEquipped[5];

        for (int i = 0; i < m_passives.Length; i++)
            m_passives[i] = Instantiate(itemPrefab, rootPassives);

        m_abilities = new UI_ItemEquipped[5];

        for (int i = 0; i < m_abilities.Length; i++)
            m_abilities[i] = Instantiate(itemPrefab, rootAbilities);
    }

    private void OnEnable()
    {
        gameEvent.OnAttributeChanged += OnAttributeChanged;
        gameEvent.OnAbilityChanged += OnAbilityChanged;
    }

    private void OnDisable()
    {
        gameEvent.OnAttributeChanged -= OnAttributeChanged;
        gameEvent.OnAbilityChanged -= OnAbilityChanged;
    }

    private void OnAttributeChanged(PlayerStats playerStats)
    {
        int id = 0;

        foreach (var attribute in playerStats.Attributes)
        {
            if (attribute.GetLevel() < 1)
                continue;

            m_passives[id].Refresh(attribute.Data.Icon, attribute.GetLevel());

            if ((++id) >= m_passives.Length)
                break;
        }
    }

    private void OnAbilityChanged(PlayerAbility playerAbility)
    {
        int maxLoop = Mathf.Min(m_abilities.Length, playerAbility.Abilities.Count);

        for (int i = 0; i < maxLoop; i++)
        {
            Ability ability = playerAbility.Abilities[i];

            if (ability == null)
                m_abilities[i].Refresh(null);
            else
                m_abilities[i].Refresh(ability.Data.Icon, ability.GetLevel());
        }
    }
}
