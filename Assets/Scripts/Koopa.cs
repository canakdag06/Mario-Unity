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
                    Vector2 direction = new(transform.position.x - collision.gameObject.transform.position.x, 0f);
                    Slide(direction);
                }
                else
                {
                    player.Hit();
                }
            }
        }
        else if (!isShelled && collision.gameObject.layer == slidingShellLayer)
        {
            GetHit();
        }
    }


    private void EnterShell()
    {
        isShelled = true;

        entityMovement.enabled = false;
        spriteAnimation.enabled = false;
        spriteRenderer.sprite = shellSprite;

        gameObject.layer = slidingShellLayer;
    }

    private void Slide(Vector2 direction)
    {
        isSliding = true;
        gameObject.layer = slidingShellLayer;

        entityMovement.direction = direction.normalized;
        entityMovement.speed = slidingSpeed;
        entityMovement.enabled = true;
    }

    private void GetHit()
    {
        entityMovement.enabled = false;
        deathAnimation.enabled = true;

        Destroy(gameObject, 3f);
    }

    private void OnBecameInvisible()
    {
        if(isSliding)
        {
            Destroy(gameObject);
        }
    }
}
