using UnityEngine;

public class FlyingFollowMovement : MonoBehaviour
{
    public GameObject Target;
    private float Dis;
    private bool isBouncing = false;
    private float bounceDuration = 0.2f; // Prevents instant movement override
    private float bounceTimer = 0f;

    [SerializeField] private float speed; 
    [SerializeField] private float distance;
    [SerializeField] private float slowSpeed;
    [SerializeField] private float bounceForce;

    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false; 
        rb.gravityScale = 0;    // No gravity for flying
        rb.freezeRotation = true;// Prevents physics forces from affecting it
    }

    // Update is called once per frame
    void Update()
    {
        if (isBouncing)
        {
            bounceTimer -= Time.deltaTime;
            if (bounceTimer <= 0)
            {
                isBouncing = false;
            }
            return; // Stop movement while bouncing
        }
        Vector2 targetPosition = Target.transform.position;
        Dis = Vector2.Distance(rb.position, Target.transform.position);

        float currentSpeed = (Dis >= distance) ? speed : slowSpeed;

        Vector2 direction = (targetPosition- rb.position).normalized;

        rb.linearVelocity = direction * currentSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Slight bounce effect when hitting ground
            rb.linearVelocity *= 0.5f; 
        }
        if (collision.gameObject.CompareTag("Player"))
        {
         

            // Calculate bounce direction away from player
            Vector2 bounceDirection = (rb.position - (Vector2)collision.transform.position).normalized;
            rb.linearVelocity = bounceDirection * bounceForce;

            isBouncing = true;
            bounceTimer = bounceDuration;
        }
    }
}
