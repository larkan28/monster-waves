using UnityEngine;

public class ProjectileTornado : Projectile
{
    private Animator m_animator;

    protected override void OnInit()
    {
        m_animator = GetComponent<Animator>();
    }

    protected override void OnThink()
    {

    }

    protected override void OnRemove()
    {
        m_animator.SetBool("Destroy", true);
    }

    public void EventRemoveTornado()
    {
        Remove();
    }
}
