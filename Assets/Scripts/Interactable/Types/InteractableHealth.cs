using UnityEngine;

public class InteractableHealth : Interactable
{
    [SerializeField] private float healingAmount;
    [SerializeField] private AudioClip soundPickup;

    private Actor m_actorInteracted;
    private FollowTarget m_followTarget;

    protected override void Init()
    {
        m_followTarget = GetComponent<FollowTarget>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_hasCollided)
            return;

        if (collider.TryGetComponent(out Health health) && health.Owner == m_actorInteracted)
        {
            health.AddHealth(healingAmount);

            m_hasCollided = true;
            Destroy(gameObject);
        }
    }

    public override void Interact(Actor actor)
    {
        if (actor == null || m_hasInteracted)
            return;

        m_hasInteracted = true;
        m_actorInteracted = actor;
        m_followTarget.Target = actor.transform;

        if (actor.TryGetComponent(out AudioSource audioSource))
            audioSource.PlayOneShot(soundPickup);
    }
}
