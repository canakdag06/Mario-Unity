using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private new Collider2D collider;
    [SerializeField] private EntityMovement entityMovement;
    [SerializeField] private SpriteAnimation spriteAnimation;
    [SerializeField] private DeathAnimation deathAnimation;

    [SerializeField] private float slidingSpeed = 12f;

    private int slidingShellLayer;
    private bool isShelled;
    private bool isSliding;

    private void Awake()
    {
        slidingShellLayer = LayerMask.NameToLayer("SlidingShell");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player.IsStarPowered)
            {
                GetHit();
                return;
            }

            if (!isShelled)
            {
                if (transform.DotTest(collision.transform, Vector2.up))
                {
                    EnterShell();
                }
                else
                {
                    player.Hit();
                    entityMovement.direction.x *= -1f;
                }
            }
        }
        else if (!isShelled && collision.gameObject.layer == slidingShellLayer)
        {
            GetHit();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isShelled && collision.CompareTag("Player"))
        {
            if (!isSliding)
            {
                Vector2 direction = new Vector2(transform.position.x - collision.transform.position.x, 0f);
                Slide(direction);
            }
            else
            {
                Player player = collision.GetComponent<Player>();
                player.Hit();
                entityMovement.direction.x *= -1f;
            }
        }
    }


    private void EnterShell()
    {
        isShelled = true;

        entityMovement.enabled = false;
        spriteAnimation.enabled = false;
        spriteRenderer.sprite = shellSprite;
    }

    private void Slide(Vector2 direction)
    {
        isSliding = true;
        gameObject.layer = slidingShellLayer;

        entityMovement.direction = direction.normalized;
        entityMovement.speed = slidingSpeed;
        entityMovement.enabled = true;

        gameObject.layer = slidingShellLayer;
    }

    private void GetHit()
    {
        entityMovement.enabled = false;
        deathAnimation.enabled = true;

        Destroy(gameObject, 3f);
    }

    private void OnBecameInvisible()
    {
        if (isSliding)
        {
            Destroy(gameObject);
        }
    }
}
