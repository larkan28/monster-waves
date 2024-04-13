using UnityEngine;

public abstract class Actor : MonoBehaviour
{
    [SerializeField] private string actorName;
    [SerializeField] private int actorTeam;

    public Health Health => m_health;
    public string Name => actorName;
    public int Team => actorTeam;

    protected Health m_health;

    private void Awake()
    {
        ActorManager.Instance.Add(this);

        if (TryGetComponent(out Health health))
        {
            m_health = health;
            m_health.Init(this);
        }

        OnSpawn();
    }

    public abstract void OnSpawn();

    public override bool Equals(object other)
    {
        if (other != null && other is Actor otherActor)
            return actorName.Equals(otherActor.actorName);

        return false;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return actorName;
    }
}
