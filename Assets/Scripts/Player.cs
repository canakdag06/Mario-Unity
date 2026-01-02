using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerSpriteRenderer smallRenderer;
    public PlayerSpriteRenderer bigRenderer;

    public bool IsSmall => smallRenderer.enabled;
    public bool IsBig => bigRenderer.enabled;
    public bool IsDead => deathAnimation.enabled;
    public bool IsStarPowered { get; private set; }

    private DeathAnimation deathAnimation;
    private CapsuleCollider2D capsuleCollider;
    private PlayerSpriteRenderer activeRenderer;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        playerMovement.EnableCouchCollider += OnEnableCrouchCollider;
        playerMovement.DisableCouchCollider += OnDisableCrouchCollider;
    }

    private void OnDisable()
    {
        playerMovement.EnableCouchCollider -= OnEnableCrouchCollider;
        playerMovement.DisableCouchCollider -= OnDisableCrouchCollider;
    }

    public void Hit()
    {
        if (!IsStarPowered && !IsDead)
        {
            if (IsBig)
            {
                Shrink();
            }
            else
            {
                Death();
            }
        }
    }

    public void Grow()
    {
        if (IsBig) return;

        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        AdjustCollider(0f, 0.4f, 0.8f, 1.8f);

        StartCoroutine(ScaleAnimation());
    }

    public void StarPower()
    {
        StartCoroutine(StarPowerAnimation());
    }

    private void Shrink()
    {
        smallRenderer.enabled = true;
        bigRenderer.enabled = false;
        activeRenderer = smallRenderer;

        AdjustCollider(0f, 0f, 0.7f, 1f);

        StartCoroutine(ScaleAnimation());
    }

    private IEnumerator ScaleAnimation()
    {
        float duration = 1f;
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            smallRenderer.enabled = !smallRenderer.enabled;
            bigRenderer.enabled = !bigRenderer.enabled;

            yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
        }

        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        activeRenderer.enabled = true;
    }

    private IEnumerator StarPowerAnimation()
    {
        IsStarPowered = true;

        float duration = 10f;
        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            activeRenderer.spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.1f);
        }

        IsStarPowered = false;
        activeRenderer.spriteRenderer.color = Color.white;
    }

    private void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }

    private void OnEnableCrouchCollider()
    {
        if (activeRenderer == bigRenderer)
        {
            AdjustCollider(0f, 0.15f, 0.8f, 1.3f);
        }
        else if (activeRenderer == smallRenderer)
        {
            AdjustCollider(0f, 0f, 0.7f, 1f);
        }
    }

    private void OnDisableCrouchCollider()
    {
        if (activeRenderer == bigRenderer)
        {
            AdjustCollider(0f, 0.4f, 0.8f, 1.8f);
        }
        else if (activeRenderer == smallRenderer)
        {
            AdjustCollider(0f, 0f, 0.7f, 1f);
        }
    }

    private void AdjustCollider(float offsetX, float offsetY, float sizeX, float sizeY)
    {
        capsuleCollider.offset = new Vector2(offsetX, offsetY);
        capsuleCollider.size = new Vector2(sizeX, sizeY);
    }
}
