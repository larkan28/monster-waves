using UnityEngine;
using UnityEngine.UI;

public class UI_Experience : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Image expBar;

    private void OnEnable()
    {
        gameEvent.OnUpdateExperience += UpdateExperience;
    }

    private void OnDisable()
    {
        gameEvent.OnUpdateExperience -= UpdateExperience;
    }

    private void UpdateExperience(float expRatio)
    {
        expBar.fillAmount = expRatio;
    }
}
