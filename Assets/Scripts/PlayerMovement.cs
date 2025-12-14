using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private InputReader inputReader;


    [SerializeField] private float maxSpeed = 8f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 20f;

    private new Rigidbody2D rigidbody;
    private new Camera camera;
    private Vector2 moveInput;
    private float playerWidth;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;

        playerWidth = GetComponent<CapsuleCollider2D>().bounds.extents.x;
    }

    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.MoveEvent += HandleMovement;
        }
    }

    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.MoveEvent -= HandleMovement;
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ClampPosition();
    }

    private void HandleMovement(Vector2 direction)
    {
        moveInput = direction;
    }

    private void ApplyMovement()
    {
        float targetSpeed = moveInput.x * maxSpeed;
        float speedDif = targetSpeed - rigidbody.linearVelocity.x;
        float changeRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float movement = Mathf.MoveTowards(rigidbody.linearVelocity.x, targetSpeed, changeRate * Time.fixedDeltaTime);
        rigidbody.linearVelocity = new Vector2(movement, rigidbody.linearVelocity.y);
    }

    private void ClampPosition()
    {
        Vector2 leftEdge = camera.ViewportToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ViewportToWorldPoint(Vector2.one);
        Vector3 currentPos = transform.position;

        currentPos.x = Mathf.Clamp(currentPos.x, leftEdge.x + playerWidth, rightEdge.x - playerWidth);

        transform.position = currentPos;
    }

}
