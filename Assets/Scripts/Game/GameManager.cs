using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum Status
    {
        Init = 0,
        NotPlaying,
        Playing,
        Paused,
        GameOver
    };

    public float GameTimer => m_gameTimer;
    public Status CurrStatus => m_gameStatus;
    public bool IsPausedForced => m_forcedPause;

    private bool m_forcedPause;
    private float m_gameTimer;
    private Status m_gameStatus;
    private GameEvent m_gameEvent;

    private void Update()
    {
        if (m_gameStatus == Status.GameOver)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (m_gameStatus == Status.Paused)
                Unpause();
            else
                Pause();
        }

        if (m_gameStatus == Status.Playing)
            m_gameTimer += Time.deltaTime;
    }

    public void Init(GameEvent gameEvent)
    {
        m_gameEvent = gameEvent;
        SetStatus(Status.Init);

        Play();
    }

    public void Play()
    {
        m_gameTimer = 0;
        SpawnManager.Instance.SpawnPlayer();

        SetStatus(Status.Playing);
    }

    public void Retry()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void GameOver()
    {
        if (m_gameStatus == Status.GameOver)
            return;

        SetStatus(Status.GameOver);
    }

    public void Pause(bool forced = false)
    {
        if (m_gameStatus == Status.Paused)
            return;

        m_forcedPause = forced;
        SetStatus(Status.Paused);
        Time.timeScale = 0f;
    }

    public void Unpause(bool forced = false)
    {
        if (m_gameStatus != Status.Paused)
            return;

        if (m_forcedPause && !forced)
            return;

        m_forcedPause = false;
        SetStatus(Status.Playing);
        Time.timeScale = 1f;
    }

    private void SetStatus(Status status)
    {
        m_gameStatus = status;
        m_gameEvent.GameStatusChanged(status);
    }
}
