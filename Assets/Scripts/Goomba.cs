using UnityEngine;

public class Goomba : MonoBehaviour
{
    public Sprite flatSprite;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private new Collider2D collider;
    [SerializeField] private EntityMovement entityMovement;
    [SerializeField] private SpriteAnimation spriteAnimation;

    private LayerMask deadEnemyMask;
    private void Awake()
    {
        deadEnemyMask = LayerMask.NameToLayer("DeadEnemy");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                Flatten();
            }
        }
    }


    private void Flatten()
    {
        entityMovement.enabled = false;
        spriteAnimation.StopAnimation();
        spriteRenderer.sprite = flatSprite;
        gameObject.layer = deadEnemyMask;

        Destroy(gameObject, 1f);
    }
}
