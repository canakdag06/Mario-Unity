using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private new Collider2D collider;
    [SerializeField] private EntityMovement entityMovement;
    [SerializeField] private SpriteAnimation spriteAnimation;
    [SerializeField] private DeathAnimation deathAnimation;

    private LayerMask deadEnemyMask;
    private int slidingShellLayer;
    private void Awake()
    {
        deadEnemyMask = LayerMask.NameToLayer("DeadEnemy");
        slidingShellLayer = LayerMask.NameToLayer("SlidingShell");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (transform.DotTest(collision.transform, Vector2.up))
            {
                Flatten();
            }
            else if (player.IsStarPowered)
            {
                GetHit();
            }
            else
            {
                player.Hit();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == slidingShellLayer)
        {
            GetHit();
        }
    }


    private void Flatten()
    {
        entityMovement.enabled = false;
        spriteAnimation.enabled = false;
        spriteRenderer.sprite = flatSprite;
        gameObject.layer = deadEnemyMask;

        Destroy(gameObject, 1f);
    }

    private void GetHit()
    {
        collider.enabled = false;
        entityMovement.enabled = false;
        deathAnimation.enabled = true;

        Destroy(gameObject, 3f);
    }
}
