using UnityEngine;

public class InteractableExp : Interactable
{
    [SerializeField] private int expAmount;
    [SerializeField] private AudioClip soundPickup;

    private FollowTarget m_followTarget;

    protected override void Init()
    {
        m_followTarget = GetComponent<FollowTarget>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_hasCollided)
            return;

        if (collider.TryGetComponent(out PlayerLevel playerLevel))
        {
            playerLevel.AddExp(expAmount);

            m_hasCollided = true;
            Destroy(gameObject);
        }
    }

    public override void Interact(Actor actor)
    {
        if (actor == null || m_hasInteracted)
            return;

        m_hasInteracted = true;
        m_followTarget.Target = actor.transform;

        if (actor.TryGetComponent(out AudioSource audioSource))
            audioSource.PlayOneShot(soundPickup);
    }
}
