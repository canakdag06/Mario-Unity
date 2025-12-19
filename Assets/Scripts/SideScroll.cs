using UnityEngine;

public class SideScroll : MonoBehaviour
{
    private Transform cameraTarget;

    private void Start()
    {
        cameraTarget = GameManager.Instance.Player.transform;
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = Mathf.Max(cameraPosition.x, cameraTarget.position.x);    // camera does not go left in original game
        transform.position = cameraPosition;
    }
}
