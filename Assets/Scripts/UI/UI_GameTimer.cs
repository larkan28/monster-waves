using TMPro;
using UnityEngine;

public class UI_GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTimer;

    private GameManager m_gameManager;

    private void Awake()
    {
        m_gameManager = GameManager.Instance;
    }

    private void Update()
    {
        float time = m_gameManager.GameTimer;

        int secs = Mathf.RoundToInt(time % 60f);
        int mins = Mathf.RoundToInt(Mathf.Floor(time / 60f));

        textTimer.text = mins.ToString("00") + ":" + secs.ToString("00");
    }
}
