using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            GameManager.Instance.ResetLevel(3f);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
