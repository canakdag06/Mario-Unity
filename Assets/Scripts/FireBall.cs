using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float speed = 10f;
    public float range = 1.5f;
    public float waveSpeed = 5f;

    private Vector3 startPosition;
    private Vector3 direction;
    private float timer;


    private ObjectPool pool;

    private void Awake()
    {
        pool = PoolManager.Instance.fireballPool;
    }
    private void OnEnable()
    {
        timer = 0f;
        startPosition = transform.position;
        Debug.Log("startPosition: " + startPosition);
    }

    private void Update()
    {
        timer += Time.deltaTime * waveSpeed;
        Vector3 forwardMove = direction * speed * (timer / waveSpeed);
        float yOffset = Mathf.PingPong(timer, range) - (range / 2f);
        transform.position = startPosition + forwardMove + new Vector3(0, yOffset, 0);
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
