using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private Actor owner;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Health health))
            health.TakeDamage(damage, owner, transform);
    }
}
