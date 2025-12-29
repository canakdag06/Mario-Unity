using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [Header("Connection Settings")]
    public Transform connection;
    public Vector3 enterDirection = Vector3.down;
    public Vector3 exitDirection = Vector3.up;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (connection != null && collision.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();

            if ((playerMovement.Input.x != 0f && playerMovement.Input.x == enterDirection.x)
                || (playerMovement.Crouching && enterDirection == Vector3.down))
            {

                StartCoroutine(EnterPipeAnimation(collision.transform));
            }
        }
    }

    private IEnumerator EnterPipeAnimation(Transform player)
    {
        player.GetComponent<PlayerMovement>().enabled = false;

        Vector3 enteredPosition = transform.position + enterDirection;
        yield return Move(player, enteredPosition);
    }

    private IEnumerator Move(Transform player, Vector3 endPosition)
    {
        float elapsed = 0f;
        float duration = 1f;

        Vector3 startPosition = player.position;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            player.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPosition;
    }

}
