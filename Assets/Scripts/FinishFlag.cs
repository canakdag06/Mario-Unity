using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishFlag : MonoBehaviour
{
    [SerializeField] private Transform flag;
    [SerializeField] private Transform end;
    [SerializeField] private Transform castle;
    [SerializeField] private float speed = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(MoveTo(flag, end.position));
            StartCoroutine(LevelCompleteSequence(collision.transform));
        }
    }

    private IEnumerator LevelCompleteSequence(Transform player)
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        playerMovement.Climbing = true;
        yield return MoveTo(player, end.position);
        playerMovement.Climbing = false;
        yield return MoveTo(player, player.position + Vector3.right);
        yield return MoveTo(player, player.position + Vector3.right + Vector3.down);
        yield return MoveTo(player, castle.position);

        player.gameObject.SetActive(false);
        SceneManager.LoadScene("MainMenu");

    }

    private IEnumerator MoveTo(Transform subject, Vector3 target)
    {
        while ((subject.position - target).sqrMagnitude > 0.05f)
        {
            subject.position = Vector3.MoveTowards(subject.position, target, speed * Time.deltaTime);
            yield return null;
        }

        subject.position = target;
    }
}
