using System.Collections;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] new private CircleCollider2D collider;

    private void Start()
    {
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
        collider.enabled = false;
    }

    public void PopUp()
    {
        StartCoroutine(PopUpAnimation());
    }

    private IEnumerator PopUpAnimation()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        Vector3 startPos = transform.localPosition;
        Vector3 endPos = transform.localPosition + Vector3.up;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        collider.enabled = true;
    }
}
