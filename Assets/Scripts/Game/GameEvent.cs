using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    public float MaxGameTime;

    [SerializeField] private Ability[] abilities;
    [SerializeField] private AttributeData[] attributes;

    public Ability[] Abilities => abilities;
    public AttributeData[] Attributes => attributes;

    #region EVENTS
    public event Action<float> OnUpdateHealth;
    public void UpdateHealth(float hpRatio) { OnUpdateHealth?.Invoke(hpRatio); }

    public event Action<float> OnUpdateExperience;
    public void UpdateExperience(float expRatio) { OnUpdateExperience?.Invoke(expRatio); }

    public event Action<GameManager.Status> OnGameStatusChanged;
    public void GameStatusChanged(GameManager.Status status) { OnGameStatusChanged?.Invoke(status); }

    public event Action<IReward[]> OnLevelUp;
    public void LevelUp(IReward[] rewards) { OnLevelUp?.Invoke(rewards); }

    public event Action<IReward> OnRewardSelected;
    public void SelectReward(IReward reward) { OnRewardSelected?.Invoke(reward); }

    public event Action<PlayerStats> OnAttributeChanged;
    public void UpdateAttribute(PlayerStats playerStats) { OnAttributeChanged?.Invoke(playerStats); }

    public event Action<PlayerAbility> OnAbilityChanged;
    public void UpdateAbility(PlayerAbility playerAbility) { OnAbilityChanged?.Invoke(playerAbility); }

    public event Action<int> OnPlayerLevelChanged;
    public void PlayerLevelChanged(int level) { OnPlayerLevelChanged?.Invoke(level); }
    #endregion
}
