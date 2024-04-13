using UnityEngine;

public abstract class Menu : MonoBehaviour
{
    [SerializeField] protected string menuTag;
    [SerializeField] protected GameObject body;

    public string Tag => menuTag;

    protected MenuManager m_manager;

    public void Init(MenuManager manager)
    {
        m_manager = manager;
        Hide();
    }

    public virtual void Show()
    {
        body.SetActive(true);
    }

    public virtual void Hide()
    {
        body.SetActive(false);
    }

    public virtual bool CanClose()
    {
        return true;
    }
}
