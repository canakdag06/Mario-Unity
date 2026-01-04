using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    private Vector2 direction;

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * direction);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ReturnToPool();
    }

    private void OnBecameInvisible()
    {
        ReturnToPool();
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
