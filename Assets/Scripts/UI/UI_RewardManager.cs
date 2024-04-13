using System.Collections.Generic;
using UnityEngine;

public class UI_RewardManager : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private UI_Reward rewardPrefab;
    [SerializeField] private Transform rewardsParent;
    [SerializeField] private GameObject rootContainer;

    private readonly List<UI_Reward> m_rewards = new();

    private void OnEnable()
    {
        gameEvent.OnLevelUp += ShowOptions;
    }

    private void OnDisable()
    {
        gameEvent.OnLevelUp -= ShowOptions;
    }

    private void Awake()
    {
        Close();
    }

    private void ShowOptions(IReward[] rewards)
    {
        ClearOptions();

        for (int i = 0; i < rewards.Length; i++)
        {
            if (rewards[i] == null)
                continue;

            UI_Reward rewardUI = Instantiate(rewardPrefab, rewardsParent);

            if (rewardUI != null)
            {
                rewardUI.ShowReward(rewards[i], this);
                m_rewards.Add(rewardUI);
            }
        }

        rootContainer.SetActive(true);
    }

    public void ClearOptions()
    {
        foreach (var reward in m_rewards)
            Destroy(reward.gameObject);

        m_rewards.Clear();
    }

    public void Close()
    {
        rootContainer.SetActive(false);
    }

    public void SelectReward(IReward reward)
    {
        gameEvent.SelectReward(reward);
        Close();
    }
}
