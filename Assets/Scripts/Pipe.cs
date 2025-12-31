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

            if ((playerMovement.Input.x != 0f && playerMovement.Input.x * enterDirection.x > 0)
                || (playerMovement.Crouching && enterDirection == Vector3.down))
            {
                StartCoroutine(EnterPipeAnimation(collision.transform));
            }
        }
    }

    private IEnumerator EnterPipeAnimation(Transform player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.enabled = false;

        Vector3 enteredPosition = player.transform.position + enterDirection;
        yield return Move(player, enteredPosition);
        yield return new WaitForSeconds(1f);

        bool isUnderground = connection.position.y < 0f;
        Camera.main.GetComponent<SideScroll>().SetUnderGround(isUnderground);

        if (exitDirection != Vector3.zero)
        {
            player.position = connection.position - exitDirection;
            yield return Move(player, connection.position + exitDirection);
        }
        else
        {
            player.position = connection.position;
        }

        playerMovement.enabled = true;
        playerMovement.HandleCrouchCancelled();
    }

    private IEnumerator Move(Transform player, Vector3 endPosition)
    {
        float elapsed = 0f;
        float duration = 1f;

        Vector3 startPosition = player.position;

        while (elapsed < duration)
        {
            Debug.Log("Player: " + player.transform.position);
            float t = elapsed / duration;

            player.position = Vector3.Lerp(startPosition, endPosition, t);
            elapsed += Time.deltaTime;

            yield return null;
        }

        player.position = endPosition;
    }

}
