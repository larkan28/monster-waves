using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Image healthBar;

    private void OnEnable()
    {
        gameEvent.OnUpdateHealth += UpdateHealth;
    }

    private void OnDisable()
    {
        gameEvent.OnUpdateHealth -= UpdateHealth;
    }

    private void UpdateHealth(float hpRatio)
    {
        healthBar.fillAmount = hpRatio;
    }
}
