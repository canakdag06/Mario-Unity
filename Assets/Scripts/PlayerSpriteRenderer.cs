using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public Sprite run;

    private SpriteRenderer spriteRenderer;
    private PlayerMovement movement;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponentInParent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        if (movement.Jumping)
        {
            spriteRenderer.sprite = jump;
        }
        else if (movement.Sliding)
        {
            spriteRenderer.sprite = slide;
        }
        else if (movement.Running)
        {
            spriteRenderer.sprite = run;
        }
        else
        {
            spriteRenderer.sprite = idle;
        }
    }
}
