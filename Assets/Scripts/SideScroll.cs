using UnityEngine;

public class SideScroll : MonoBehaviour
{
    public Transform cameraTarget;

    [SerializeField] private float defaultYPos = 6.5f;
    [SerializeField] private float undergroundYPos = -14.5f;
    private void Start()
    {
        defaultYPos = transform.position.y;
    }

    private void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = Mathf.Max(cameraPosition.x, cameraTarget.position.x);    // camera does not go left in original game
        transform.position = cameraPosition;
    }

    public void SetUnderGround(bool isUnderground)
    {
        Vector3 cameraPos = transform.position;
        cameraPos.y = isUnderground ? undergroundYPos : defaultYPos;
        transform.position = cameraPos;
    }
}
