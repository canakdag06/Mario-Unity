using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    public float speed = 1f;
    public Vector2 direction = Vector2.left;

    private new Rigidbody2D rigidbody;
    private Vector2 velocity;
    private LayerMask colliderMask;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        colliderMask = LayerMask.GetMask("Default", "Enemy");
        enabled = false;
    }

    private void OnEnable()
    {
        rigidbody.WakeUp();
    }

    private void OnDisable()
    {
        rigidbody.linearVelocity = Vector2.zero;
        rigidbody.Sleep();
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }

    private void FixedUpdate()
    {
        velocity.x = direction.x * speed;
        velocity.y += Physics2D.gravity.y * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);

        if (rigidbody.Raycast(direction, colliderMask))
        {
            direction = -direction;
        }

        if(rigidbody.Raycast(Vector2.down, colliderMask))
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere((Vector2)transform.position + direction.normalized * 0.375f, 0.25f);
    //}
}
