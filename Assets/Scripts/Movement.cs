using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float sprintMultiplier; 
    [SerializeField] private float jump; 
    [SerializeField] private float bounceForce;
    [SerializeField] private float shrinkScaleY; // New Y scale when shrinking
    [SerializeField] private float normalScaleY; // Normal Y scale

    private Rigidbody2D body;
    private bool grounded;
    private float bounceDuration = 0.2f; // Prevents instant movement override
    private float bounceTimer = 0f;
    private bool isBouncing = false;
    private bool running = false;

    private Vector3 originalScale;
    private bool isShrinking = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.isKinematic = false; 
        body.freezeRotation = true;

        originalScale = transform.localScale;
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
            return; // Ignore movement input while bouncing
        }

        if (Input.GetKey(KeyCode.S) && grounded)
        {
            isShrinking = true;
        }
        else
        {
            isShrinking = false;
        }

        // Crouch
        if (isShrinking)
        {
            transform.localScale = new Vector3(originalScale.x, shrinkScaleY, originalScale.z);
        }
        else
        {
            transform.localScale = new Vector3(originalScale.x, normalScaleY, originalScale.z);
        }

        float moveSpeed = speed;

        //Sprinting
        if (grounded && Input.GetKey(KeyCode.LeftShift))
        {
            Debug.Log("Running!");
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
            Debug.Log("Walking");
            running = false;
        }

        //Moving left and moving right
        float moveInput = Input.GetAxisRaw("Horizontal"); 
        body.linearVelocity = new Vector2(moveInput * moveSpeed, body.linearVelocity.y);
        
        //Jump
        if((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && grounded && !isShrinking)
        {
            Jump();
        }
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jump);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
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
