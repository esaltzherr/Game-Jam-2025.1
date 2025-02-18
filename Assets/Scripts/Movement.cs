using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed; 
    [SerializeField] private float jump; 
    [SerializeField] private float bounceForce;

    private Rigidbody2D body;
    private bool grounded;
    private float bounceDuration = 0.2f; // Prevents instant movement override
    private float bounceTimer = 0f;
    private bool isBouncing = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.isKinematic = false; 
        body.freezeRotation = true;
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
        
        body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.linearVelocity.y);
        if(Input.GetKey(KeyCode.Space) && grounded)
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
