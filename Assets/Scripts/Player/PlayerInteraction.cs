using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameEvent gameEvent;
    [SerializeField] private LayerMask layerInteractable;
    [SerializeField] private float collectRadius;

    private float m_collectedRadius;
    private PlayerActor m_playerActor;
    private readonly Collider2D[] m_colliders = new Collider2D[16];

    private void OnEnable()
    {
        gameEvent.OnAttributeChanged += OnAttributeChanged;
    }

    private void OnDisable()
    {
        gameEvent.OnAttributeChanged -= OnAttributeChanged;
    }

    internal void Init(PlayerActor playerActor)
    {
        m_playerActor = playerActor;
        m_collectedRadius = collectRadius;
    }

    internal void ThinkFixed()
    {
        CollectExp();
    }

    private void CollectExp()
    {
        int maxColliders = Physics2D.OverlapCircleNonAlloc(transform.position, m_collectedRadius, m_colliders, layerInteractable);

        for (int i = 0; i < maxColliders; i++)
        {
            if (m_colliders[i].TryGetComponent(out Interactable interactable))
                interactable.Interact(m_playerActor);
        }
    }

    private void OnAttributeChanged(PlayerStats stats)
    {
        m_collectedRadius = collectRadius * stats.GetBonus("collect_radius");
    }
}
