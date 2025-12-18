using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public SpriteAnimation run;

    private SpriteRenderer spriteRenderer;
    private PlayerMovement movement;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<PlayerMovement>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        run.enabled = movement.Running;

        if (movement.Jumping)
        {
            spriteRenderer.sprite = jump;
        }
        else if (movement.Sliding)
        {
            spriteRenderer.sprite = slide;
        }
        else if (!movement.Running)
        {
            spriteRenderer.sprite = idle;
        }
    }
}
