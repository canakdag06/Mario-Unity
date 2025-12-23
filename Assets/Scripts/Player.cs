using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;

    public bool IsSmall => smallRenderer.enabled;
    public bool IsBig => bigRenderer.enabled;
    public bool IsDead => deathAnimation.enabled;

    private DeathAnimation deathAnimation;


    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
    }

    public void Hit()
    {
        if(IsBig)
        {
            Shrink();
        }
        else
        {
            Death();
        }
    }

    private void Shrink()
    {
        // TODO
    }

    private void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }
}
