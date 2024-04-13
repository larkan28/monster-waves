using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private AudioClip soundLevelup;
    [SerializeField] private int costLevel;
    [SerializeField] private bool isTesting;

    public int Level => m_level;

    private GameManager m_gameManager;
    private PlayerStats m_playerStats;
    private AudioSource m_audioSource;
    private PlayerAbility m_playerAbility;
    private int m_level;
    private int m_exp;

    private void OnEnable()
    {
        gameEvent.OnRewardSelected += OnRewardSelected;
    }

    private void OnDisable()
    {
        gameEvent.OnRewardSelected -= OnRewardSelected;
    }

    internal void Init(PlayerActor playerActor)
    {
        m_level = m_exp = 0;

        m_gameManager = GameManager.Instance;
        m_audioSource = GetComponent<AudioSource>();
        m_playerStats = playerActor.GetComponent<PlayerStats>();
        m_playerAbility = playerActor.GetComponent<PlayerAbility>();

        ShowExp();
        ShowLvl();
    }

    internal void Think()
    {
        if (isTesting && Input.GetKeyDown(KeyCode.H))
            LevelUp();
    }

    private void ShowExp()
    {
        float exp = m_exp;
        float cost = CostLevel(m_level);

        gameEvent.UpdateExperience(exp / cost);
    }

    private void ShowLvl()
    {
        gameEvent.PlayerLevelChanged(m_level);
    }

    public void LevelUp()
    {
        m_level++;
        m_exp = 0;
        m_audioSource.PlayOneShot(soundLevelup);

        ShowLvl();
        ShowExp();

        GenerateRandomRewards();
    }

    private void GenerateRandomRewards()
    {
        List<IReward> options = new();

        foreach (var item in gameEvent.Abilities)
        {
            Ability ability = m_playerAbility.Abilities.Find(x => x.Equals(item));

            if (ability == null && m_playerAbility.CanAddAbilities)
                options.Add(item);
            else if (ability != null && !ability.IsLevelMax)
                options.Add(ability);
        }

        if (m_level > 1)
        {
            bool canAdd = m_playerStats.CanAddAttribute;

            foreach (var attribute in m_playerStats.Attributes)
            {
                if (!attribute.Data.IsUpgradeable)
                    continue;

                int level = attribute.Level;

                if (level > 0 && !attribute.IsLevelMax)
                    options.Add(attribute);
                else if (level == 0 && canAdd)
                    options.Add(attribute);
            }
        }

        if (options.Count < 1)
            return;

        int count = 0;
        int maxCount = Mathf.Min(3, options.Count);

        IReward[] rewards = new IReward[3];

        while (count < maxCount)
        {
            int randomId = Random.Range(0, options.Count);

            rewards[count] = options[randomId];
            options.RemoveAt(randomId);

            count++;
        }

        gameEvent.LevelUp(rewards);
        m_gameManager.Pause(true);
    }

    public void AddExp(int exp)
    {
        if (exp < 0)
            return;

        m_exp += Mathf.RoundToInt(exp * m_playerStats.GetBonus("exp_bonus"));

        if (m_exp >= CostLevel(m_level))
            LevelUp();

        ShowExp();
    }

    public int CostLevel(int level)
    {
        if (level < 0)
            return 0;

        return (level + 1) * costLevel;
    }

    private void OnRewardSelected(IReward reward)
    {
        if (reward is Ability ability)
            m_playerAbility.Add(ability);
        else if (reward is Attribute attribute)
            m_playerStats.Upgrade(attribute);

        m_gameManager.Unpause(true);
    }
}
