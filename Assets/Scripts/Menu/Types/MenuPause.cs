using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPause : Menu
{
    [SerializeField] private Menu menuOption;
    [SerializeField] private Button buttonContinue;
    [SerializeField] private Button buttonOptions;
    [SerializeField] private Button buttonRetry;
    [SerializeField] private Button buttonExit;
    [SerializeField] private TextMeshProUGUI textTitle;

    private GameManager m_gameManager;

    private void OnEnable()
    {
        buttonContinue.onClick.AddListener(ButtonContinue);
        buttonOptions.onClick.AddListener(ButtonOptions);
        buttonRetry.onClick.AddListener(ButtonRetry);
        buttonExit.onClick.AddListener(ButtonExit);
    }

    private void OnDisable()
    {
        buttonContinue.onClick.RemoveListener(ButtonContinue);
        buttonOptions.onClick.RemoveListener(ButtonOptions);
        buttonRetry.onClick.RemoveListener(ButtonRetry);
        buttonExit.onClick.RemoveListener(ButtonExit);
    }

    private void Awake()
    {
        m_gameManager = GameManager.Instance;
    }

    public void ButtonContinue()
    {
        m_manager.Back();
    }

    public void ButtonRetry()
    {
        GameManager.Instance.Retry();
    }

    public void ButtonOptions()
    {
        m_manager.Show(menuOption);
    }

    public void ButtonExit()
    {
        Application.Quit();
    }

    public override void Show()
    {
        base.Show();

        if (m_gameManager.CurrStatus == GameManager.Status.GameOver)
        {
            textTitle.text = "Game Over";
            buttonContinue.gameObject.SetActive(false);
        }
        else
        {
            textTitle.text = "Paused";
            buttonContinue.gameObject.SetActive(true);
        }
    }

    public override void Hide()
    {
        base.Hide();
        bool isPaused = m_gameManager != null && m_gameManager.CurrStatus == GameManager.Status.Paused;

        if (m_manager.OpenedMenus.Count < 1 && isPaused)
            m_gameManager.Unpause();
    }

    public override bool CanClose()
    {
        return m_gameManager.CurrStatus != GameManager.Status.GameOver;
    }
}
