using System.Collections;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject flowerPrefab;
    [SerializeField] private GameObject blockCoinPrefab;
    [SerializeField] private Sprite emptyBlock;
    [SerializeField] private int maxHits = -1;
    [SerializeField] private bool isBreakable = false;
    [SerializeField] GameObject brickPieces;

    private SpriteRenderer spriteRenderer;

    private bool isAnimating = false;

    private GameObject currentItemToSpawn;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentItemToSpawn = itemPrefab;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAnimating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(transform, Vector2.up))
            {
                Player player = collision.gameObject.GetComponent<Player>();

                if (player.IsBig && isBreakable)
                {
                    Break();
                    return;
                }

                Hit(player);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(transform, Vector2.up))
            {
                Collider2D collider = GetComponent<Collider2D>();
                collider.isTrigger = false;
                Player player = collision.gameObject.GetComponent<Player>();
                Hit(player);
            }
        }
    }

    private void Hit(Player player)
    {
        spriteRenderer.enabled = true; // if its a hidden block

        maxHits--;

        if (maxHits == 0 && !isBreakable)
        {
            spriteRenderer.sprite = emptyBlock;
        }

        currentItemToSpawn = itemPrefab;

        if (itemPrefab == null)
        {
            if (blockCoinPrefab != null)
            {
                Instantiate(blockCoinPrefab, transform.position, Quaternion.identity);
            }
        }
        else
        {
            PowerUp powerUp = itemPrefab.GetComponent<PowerUp>();

            if (powerUp != null && powerUp.type == PowerUp.Type.MagicMushroom && player != null && player.IsBig)
            {
                currentItemToSpawn = flowerPrefab;
            }
        }

        StartCoroutine(Animate());
    }

    private void Break()
    {
        CheckForEnemyOnTop();
        ParticlePool.Instance.GetFromPool(transform.position);
        Destroy(gameObject);
    }

    private IEnumerator Animate()
    {
        isAnimating = true;

        Vector3 initialPosition = transform.localPosition;
        Vector3 secondPosition = initialPosition + Vector3.up * 0.5f;

        yield return Move(initialPosition, secondPosition);
        yield return Move(secondPosition, initialPosition);

        isAnimating = false;

        if (currentItemToSpawn != null)
        {
            SpawnItem();
        }
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0;
        float duration = 0.125f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = to;
    }

    private void SpawnItem()
    {
        GameObject item = Instantiate(currentItemToSpawn, transform.position, Quaternion.identity);
        item.GetComponent<BlockItem>().PopUp();
    }

    private void CheckForEnemyOnTop()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position + (Vector3.up * 0.5f), new Vector2(0.8f, 0.2f), 0f, LayerMask.GetMask("Enemy"));

        if (hit != null)
        {
            if (hit.TryGetComponent(out Goomba goomba)) goomba.GetHit();
            else if (hit.TryGetComponent(out Koopa koopa)) koopa.GetHit();
        }
    }
}
