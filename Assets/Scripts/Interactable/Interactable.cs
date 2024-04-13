using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] private float destroyCooldown;

    protected bool m_hasCollided;
    protected bool m_hasInteracted;

    private void Awake()
    {
        if (destroyCooldown > 0f)
            Destroy(gameObject, destroyCooldown);

        Init();
    }

    protected virtual void Init() { }
    public abstract void Interact(Actor actor);
}
