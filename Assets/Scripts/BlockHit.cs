using System.Collections;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject blockCoinPrefab;
    [SerializeField] private Sprite emptyBlock;
    [SerializeField] private int maxHits = -1;

    private SpriteRenderer spriteRenderer;

    private bool isAnimating = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isAnimating && maxHits != 0 && collision.gameObject.CompareTag("Player"))
        {
            if (collision.transform.DotTest(transform, Vector2.up))
            {
                Hit();
            }
        }
    }

    private void Hit()
    {
        spriteRenderer.enabled = true; // if its a hidden block

        maxHits--;

        if (maxHits == 0)
        {
            spriteRenderer.sprite = emptyBlock;
        }

        if (itemPrefab == null)
        {
            if (blockCoinPrefab != null)
            {
                Instantiate(blockCoinPrefab, transform.position, Quaternion.identity);
            }
        }

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        isAnimating = true;

        Vector3 initialPosition = transform.localPosition;
        Vector3 secondPosition = initialPosition + Vector3.up * 0.5f;

        yield return Move(initialPosition, secondPosition);
        yield return Move(secondPosition, initialPosition);

        isAnimating = false;

        if (itemPrefab != null)
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
        GameObject mushroom = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        mushroom.GetComponent<BlockItem>().PopUp();
    }
}
