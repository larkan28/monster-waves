using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public float MoveSpeed;
    public Transform Target;

    private void Update()
    {
        if (Target != null)
        {
            Vector3 direction = (Target.position - transform.position).normalized;
            transform.position += MoveSpeed * Time.deltaTime * direction;
        }
    }
}
