using TMPro;
using UnityEngine;

public class UI_PlayerLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textLevel;
    [SerializeField] private GameEvent gameEvent;

    private void OnEnable()
    {
        gameEvent.OnPlayerLevelChanged += OnPlayerLevelChanged;
    }

    private void OnDisable()
    {
        gameEvent.OnPlayerLevelChanged -= OnPlayerLevelChanged;
    }

    private void OnPlayerLevelChanged(int level)
    {
        textLevel.text = level.ToString();
    }
}
