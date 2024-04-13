using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Reward : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private TextMeshProUGUI textButton;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private Image imageIcon;
    [SerializeField] private Image imageBackground;
    [SerializeField] private Button buttonSelect;
    [SerializeField] private Color colorNew;
    [SerializeField] private Color colorUpgrade;

    private IReward m_reward;
    private UI_RewardManager m_rewardManager;

    private void OnEnable()
    {
        buttonSelect.onClick.AddListener(ButtonSelect);
    }

    private void OnDisable()
    {
        buttonSelect.onClick.RemoveListener(ButtonSelect);
    }

    public void ShowReward(IReward reward, UI_RewardManager rewardManager)
    {
        int level = reward.GetLevel();

        if (level == 0)
            imageBackground.color = colorNew;
        else
            imageBackground.color = colorUpgrade;

        m_reward = reward;
        m_rewardManager = rewardManager;

        textTitle.text = reward.GetName();
        textButton.text = (level == 0) ? "Select" : "Upgrade";
        textDescription.text = reward.GetDescription(level);

        imageIcon.sprite = reward.GetIcon();
    }

    private void ButtonSelect()
    {
        m_rewardManager.SelectReward(m_reward);
    }
}
