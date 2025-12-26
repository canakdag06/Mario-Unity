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

    private void Awake()
    {
        deathAnimation = GetComponent<DeathAnimation>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeRenderer = smallRenderer;
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
        smallRenderer.enabled = false;
        bigRenderer.enabled = true;
        activeRenderer = bigRenderer;

        capsuleCollider.size = new Vector2(1f, 2f);
        capsuleCollider.offset = new Vector2(0f, 0.5f);

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

        capsuleCollider.size = new Vector2(1f, 1f);
        capsuleCollider.offset = new Vector2(0f, 0f);

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

        activeRenderer.spriteRenderer.color = Color.white;
        IsStarPowered = false;
    }

    private void Death()
    {
        smallRenderer.enabled = false;
        bigRenderer.enabled = false;
        deathAnimation.enabled = true;

        GameManager.Instance.ResetLevel(3f);
    }
}
