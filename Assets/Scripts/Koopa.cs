using UnityEngine;

public class Koopa : MonoBehaviour
{
    public Sprite shellSprite;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private new Collider2D collider;
    [SerializeField] private EntityMovement entityMovement;
    [SerializeField] private SpriteAnimation spriteAnimation;

    private LayerMask deadEnemyMask;
    private bool isShelled;
    private bool isShellMoving;

    private void Awake()
    {
        deadEnemyMask = LayerMask.NameToLayer("DeadEnemy");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isShelled && collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (transform.DotTest(collision.transform, Vector2.up))
            {
                EnterShell();
            }
            else
            {
                player.Hit();
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
}
