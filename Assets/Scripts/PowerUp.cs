using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum Type
    {
        Coin,
        ExtraLife,
        MagicMushroom,
        Star,
        FireFlower
    }

    public Type type;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ApplyPowerUp(collision.gameObject);
        }
    }

    private void ApplyPowerUp(GameObject player)
    {
        switch (type)
        {
            case Type.Coin:
                GameManager.Instance.AddCoin(); break;
            case Type.ExtraLife:
                GameManager.Instance.AddLife(); break;
            case Type.MagicMushroom:
                player.GetComponent<Player>().Grow();
                break;
            case Type.Star:
                player.GetComponent<Player>().StarPower();
                break;
            case Type.FireFlower:
                player.GetComponent<Player>().EnableFireMario();
                Debug.Log("Fire Flower Alýndý!");
                break;
        }

        Destroy(gameObject);
    }
}
