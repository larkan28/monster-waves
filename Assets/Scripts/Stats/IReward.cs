using UnityEngine;

public interface IReward
{
    Sprite GetIcon();
    string GetName();
    string GetDescription(int level);
    int GetLevel();
}
