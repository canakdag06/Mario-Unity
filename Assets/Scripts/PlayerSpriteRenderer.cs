using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public Sprite crouch;
    public Sprite climb;
    public SpriteAnimation run;

    public SpriteRenderer spriteRenderer { get; private set; }
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
        run.enabled = false;
        spriteRenderer.enabled = false;
    }

    private void LateUpdate()
    {
        run.enabled = movement.Running;
        float horizontalSpeed = Mathf.Abs(movement.Velocity.x); // Adjust animation playback speed based on character speed
        float minFPS = 8f;
        float speedFactor = 1.5f;
        run.FPS = minFPS + (horizontalSpeed * speedFactor);

        if (movement.Climbing)      // The order matters here. Do not change it.
        {
            spriteRenderer.sprite = climb;
        }
        else if (movement.Jumping)
        {
            spriteRenderer.sprite = jump;
        }
        else if (movement.Crouching)
        {
            spriteRenderer.sprite = crouch;
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
