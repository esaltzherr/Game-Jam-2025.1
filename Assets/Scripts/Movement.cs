using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float jump;
    [SerializeField] private float bounceForce;
    [SerializeField] private float shrinkScaleY;
    [SerializeField] private float normalScaleY;
    [SerializeField] private float slideSpeedMultiplier = 2f; // Increased slide speed
    [SerializeField] private float slideDuration = 0.5f;

    private Rigidbody2D body;
    private bool grounded;
    private float bounceDuration = 0.2f;
    private float bounceTimer = 0f;
    private bool isBouncing = false;
    private bool running = false;

    private Vector3 originalScale;
    private bool isCrouching = false;
    private float originalSpeed;
    private float crouchSpeed;
    private bool isSliding = false;
    private float slideTimer = 0f;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        originalScale = transform.localScale;

        originalSpeed = speed;
        crouchSpeed = speed / 2f;
    }

    private void Update()
    {
        if (isBouncing)
        {
            bounceTimer -= Time.deltaTime;
            if (bounceTimer <= 0)
            {
                isBouncing = false;
            }
            return;
        }

        float moveSpeed = speed;

        // Sprinting logic
        if (grounded && Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            running = true;
            moveSpeed *= sprintMultiplier;
        }
        //Sprinting and Jumping
        else if (!grounded && running && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Running and Jumping");
            running = true;
            moveSpeed *= sprintMultiplier;
        }
        //Walking
        else
        {
            running = false;
        }

        // Sliding
        if (running && Input.GetKeyDown(KeyCode.S))
        {
            StartSlide();
        }

        // Handle sliding duration
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                EndSlide();
            }
            return; // Prevents normal movement logic while sliding
        }

        // Crouching logic (only if not sliding)
        if (Input.GetKey(KeyCode.S) && grounded)
        {
            if (!isCrouching)
            {
                isCrouching = true;
                speed = crouchSpeed;
                transform.localScale = new Vector3(originalScale.x, shrinkScaleY, originalScale.z);
            }
        }
        else
        {
            isCrouching = false;
            speed = originalSpeed;
            transform.localScale = new Vector3(originalScale.x, normalScaleY, originalScale.z);
        }

        // Moving left/right
        float moveInput = Input.GetAxisRaw("Horizontal");
        body.linearVelocity = new Vector2(moveInput * moveSpeed, body.linearVelocity.y);

        // Jumping
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && grounded && !isCrouching)
        {
            Jump();
        }
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jump);
        grounded = false;
    }

    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        Debug.Log("Sliding!");

        // Apply sliding velocity
        body.linearVelocity = new Vector2(body.linearVelocity.x * slideSpeedMultiplier, body.linearVelocity.y);
        transform.localScale = new Vector3(originalScale.x, shrinkScaleY, originalScale.z);
    }

    private void EndSlide()
    {
        isSliding = false;
        transform.localScale = new Vector3(originalScale.x, normalScaleY, originalScale.z);
        Debug.Log("Slide Ended");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector2 bounceDirection = (body.position - (Vector2)collision.transform.position).normalized;
            body.linearVelocity = bounceDirection * bounceForce;

            isBouncing = true;
            bounceTimer = bounceDuration;
        }
    }
}
