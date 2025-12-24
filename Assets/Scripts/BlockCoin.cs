using System.Collections;
using UnityEngine;

public class BlockCoin : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.AddCoin();
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        Vector3 initialPosition = transform.localPosition;
        Vector3 secondPosition = initialPosition + Vector3.up * 3f;

        yield return Move(initialPosition, secondPosition);
        yield return Move(secondPosition, initialPosition);

        Destroy(gameObject);
    }

    private IEnumerator Move(Vector3 from, Vector3 to)
    {
        float elapsed = 0;
        float duration = 0.25f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            transform.localPosition = Vector3.Lerp(from, to, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = to;
    }
}
