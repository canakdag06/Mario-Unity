using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private InputReader inputReader;

    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float maxJumpHeight = 5f;
    [SerializeField] private float maxJumpTime = 1f;
    public float JumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public float Gravity => (-2f * maxJumpHeight) / Mathf.Pow((maxJumpTime / 2f), 2); // d = Vi*t + (1/2)*a*(t^2)

    public bool Grounded { get; private set; }
    public bool Jumping { get; private set; }
    public bool Running => Mathf.Abs(velocity.x) > 0.25f;
    public bool Sliding => inputAxis * velocity.x < 0f; // is inputAxis and velocity.x are opposite signs
    public bool Crouching { get; private set; }

    public Vector2 Input { get; private set; }

    private Vector2 velocity;
    private float inputAxis;

    private new Rigidbody2D rigidbody;
    private new Camera camera;
    private bool jumpButtonStarted;
    private bool jumpButtonPerformed;


    public Action EnableCouchCollider;
    public Action DisableCouchCollider;
    private bool isTryingCrouch = false;


    private LayerMask colliderMask;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    private void OnEnable()
    {
        if (inputReader != null)
        {
            inputReader.MoveEvent += HandleMovement;

            inputReader.JumpStartedEvent += HandleJumpStarted;
            inputReader.JumpPerformedEvent += HandleJumpPerformed;
            inputReader.JumpCanceledEvent += HandleJumpCanceled;

            inputReader.CrouchStartedEvent += HandleCrouchStarted;
            inputReader.CrouchCancelledEvent += HandleCrouchCancelled;
        }

        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        velocity = Vector2.zero;
        Jumping = false;
    }

    private void Start()
    {
        colliderMask = LayerMask.GetMask("Default");
    }

    private void OnDisable()
    {
        if (inputReader != null)
        {
            inputReader.MoveEvent -= HandleMovement;
            inputReader.JumpStartedEvent -= HandleJumpStarted;
            inputReader.JumpPerformedEvent -= HandleJumpPerformed;
            inputReader.JumpCanceledEvent -= HandleJumpCanceled;

            inputReader.CrouchStartedEvent -= HandleCrouchStarted;
            inputReader.CrouchCancelledEvent -= HandleCrouchCancelled;
        }

        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        velocity = Vector2.zero;
        inputAxis = 0f;
        Jumping = false;
    }

    private void Update()
    {
        HorizontalMovement();

        Grounded = rigidbody.Raycast(Vector2.down, colliderMask);

        if (Grounded)
        {
            if (isTryingCrouch)
            {
                Crouching = true;
                EnableCouchCollider?.Invoke();
            }

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
        this.Input = direction;
        Debug.Log("Input: " + Input);
        inputAxis = direction.x;
    }

    private void HorizontalMovement()
    {
        float targetSpeed = Crouching ? 0f : inputAxis * moveSpeed;
        velocity.x = Mathf.MoveTowards(velocity.x, targetSpeed, moveSpeed * Time.deltaTime);

        if (rigidbody.Raycast(Vector2.right * velocity.x, colliderMask))
        {
            velocity.x = 0;
        }

        if (velocity.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    private void GroundedMovement()
    {
        velocity.y = Mathf.Max(velocity.y, 0f);
        Jumping = velocity.y > 0f;

        if (jumpButtonStarted)
        {
            velocity.y = JumpForce;
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

    private void HandleCrouchStarted()
    {
        isTryingCrouch = true;
    }

    private void HandleCrouchCancelled()
    {
        isTryingCrouch = false;
        Crouching = false;
        DisableCouchCollider?.Invoke();
        //Crouching = false;
        //Debug.Log("Crouch: " + Crouching);
    }

    private void ApplyGravity()
    {
        bool falling = velocity.y < 0f || !jumpButtonPerformed;
        float multiplier = falling ? 2f : 1f;

        velocity.y += Gravity * multiplier * Time.deltaTime;
        velocity.y = Mathf.Max(velocity.y, Gravity / 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = JumpForce / 2f;
                Jumping = true;
            }
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp")
            && transform.DotTest(collision.transform, Vector2.up))
        {
            velocity.y = 0f;
        }
    }

}
