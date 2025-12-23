using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private new Collider2D collider;
    [SerializeField] private EntityMovement entityMovement;
    [SerializeField] private SpriteAnimation spriteAnimation;

    [SerializeField] private float slidingSpeed = 12f;

    private LayerMask deadEnemyMask;
    private bool isShelled;
    private bool isSliding;

    private void Awake()
    {
        deadEnemyMask = LayerMask.NameToLayer("DeadEnemy");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (!isShelled)
            {
                if (transform.DotTest(collision.transform, Vector2.up))
                {
                    EnterShell();
                }
                else
                {
                    player.Hit();
                }
            }
            else
            {
                if (!isSliding)
                {
                    Vector2 direction = new Vector2(transform.position.x - collision.gameObject.transform.position.x, transform.position.y);
                    Slide(direction);
                }
                else
                {
                    player.Hit();
                }
            }
        }
    }


    private void EnterShell()
    {
        isShelled = true;

        entityMovement.enabled = false;
        spriteAnimation.StopAnimation();
        spriteRenderer.sprite = shellSprite;
    }

    private void Slide(Vector2 direction)
    {
        isSliding = true;

        entityMovement.direction = direction.normalized;
        entityMovement.speed = slidingSpeed;
        entityMovement.enabled = true;
    }
}
