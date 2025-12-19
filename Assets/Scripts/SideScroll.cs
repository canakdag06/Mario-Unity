using UnityEngine;

public class SideScroll : MonoBehaviour
{
    public Transform cameraTarget;

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = Mathf.Max(cameraPosition.x, cameraTarget.position.x);    // camera does not go left in original game
        transform.position = cameraPosition;
    }
}
