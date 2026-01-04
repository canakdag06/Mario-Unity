using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector2 direction;
    ObjectPool pool;

    private void Awake()
    {
        pool = PoolManager.Instance.fireballPool;
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Goomba goomba)) goomba.GetHit();
        else if (collision.gameObject.TryGetComponent(out Koopa koopa)) koopa.GetHit();

        pool.ReturnToPool(gameObject);
    }

    private void OnBecameInvisible()
    {
        pool.ReturnToPool(gameObject);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }
}
