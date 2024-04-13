using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private Menu[] menus;

    public readonly Stack<Menu> OpenedMenus = new();

    private GameManager m_gameManager;

    private void OnEnable()
    {
        gameEvent.OnGameStatusChanged += OnGameStatusChanged;
    }

    private void OnDisable()
    {
        gameEvent.OnGameStatusChanged -= OnGameStatusChanged;
    }

    private void Awake()
    {
        m_gameManager = GameManager.Instance;

        foreach (var menu in menus)
            menu.Init(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Back();
    }

    public void Show(string tag)
    {
        foreach (var menu in menus)
        {
            if (menu != null && menu.Tag == tag)
            {
                ShowMenu(menu);
                break;
            }
        }
    }

    public void Show(Menu menu)
    {
        ShowMenu(menu);
    }

    public void Back()
    {
        if (!OpenedMenus.TryPeek(out Menu currMenu) || !currMenu.CanClose())
            return;

        OpenedMenus.Pop();
        currMenu.Hide();

        if (OpenedMenus.TryPeek(out Menu lastMenu))
            lastMenu.Show();
    }

    public void CloseAll()
    {
        foreach (var item in OpenedMenus)
            item.Hide();

        OpenedMenus.Clear();
    }

    private void ShowMenu(Menu menuToShow)
    {
        if (menuToShow != null)
        {
            if (OpenedMenus.TryPeek(out Menu currMenu))
                currMenu.Hide();

            OpenedMenus.Push(menuToShow);
            menuToShow.Show();
        }
    }

    private void OnGameStatusChanged(GameManager.Status status)
    {
        bool isPaused = (status == GameManager.Status.Paused && !m_gameManager.IsPausedForced);

        if (isPaused || status == GameManager.Status.GameOver)
            Show("pause");
        else if (status == GameManager.Status.Playing)
            CloseAll();
    }
}
