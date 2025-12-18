using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private InputReader inputReader;



    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float maxJumpHeight = 5f;
    [SerializeField] private float maxJumpTime = 1f;
    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2); // d = Vi*t + (1/2)*a*(t^2)


    private Vector2 velocity;
    private float inputAxis;


    public bool Grounded { get; private set; }
    public bool Jumping { get; private set; }

    private new Rigidbody2D rigidbody;
    private new Camera camera;
    private float playerWidth;
    private bool jumpButtonStarted;
    private bool jumpButtonPerformed;

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
            inputReader.JumpStartedEvent += HandleJumpStarted;
            inputReader.JumpPerformedEvent += HandleJumpPerformed;
            inputReader.JumpCanceledEvent += HandleJumpCanceled;
        }

        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        velocity = Vector2.zero;
        Jumping = false;
    }

    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.MoveEvent -= HandleMovement;
            inputReader.JumpStartedEvent -= HandleJumpStarted;
            inputReader.JumpPerformedEvent -= HandleJumpPerformed;
            inputReader.JumpCanceledEvent -= HandleJumpCanceled;
        }

        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        velocity = Vector2.zero;
        inputAxis = 0f;
        Jumping = false;
    }

    private void Update()
    {
        HorizontalMovement();

        Grounded = rigidbody.Raycast(Vector2.down);

        if (Grounded)
        {
            GroundedMovement();
        }

        ApplyGravity();
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody.position;
        position += velocity * Time.fixedDeltaTime;

        // Clamp
        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        rigidbody.MovePosition(position);
    }

    private void HandleMovement(Vector2 direction)
    {
        inputAxis = direction.x;
    }

    private void HorizontalMovement()
    {
        velocity.x = Mathf.MoveTowards(velocity.x, inputAxis * moveSpeed, moveSpeed * Time.deltaTime);

        //if (rigidbody.Raycast(Vector2.right * velocity.x))
        //{
        //    velocity.x = 0f;
        //}

        //if (velocity.x > 0f)
        //{
        //    transform.eulerAngles = Vector3.zero;
        //}
        //else if (velocity.x < 0f)
        //{
        //    transform.eulerAngles = new Vector3(0f, 180f, 0f);
        //}

    }

    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        Jumping = velocity.y > 0f;

        if (jumpButtonStarted)
        {
            velocity.y = jumpForce;
            Jumping = true;
            jumpButtonStarted = false;
        }
    }

    private void HandleJumpStarted()
    {
        jumpButtonStarted = true;
    }

    private void HandleJumpPerformed()
    {
        //jumpButtonStarted = false;
        jumpButtonPerformed = true;
    }

    private void HandleJumpCanceled()
    {
        jumpButtonStarted = false;
        jumpButtonPerformed = false;
        if (rigidbody.linearVelocity.y > 0)
        {
            rigidbody.linearVelocity = new Vector2(rigidbody.linearVelocity.x, rigidbody.linearVelocity.y * 0.5f);
        }
    }


    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !jumpButtonPerformed;
        float multiplier = falling ? 2f : 1f;
        Debug.Log("falling: " + falling);

        velocity.y += gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, gravity / 2f);
    }
}
