using UnityEngine;

public class FlyingFollowMovement : MonoBehaviour
{
    public GameObject Target;
    private float Dis;
    private bool isBouncing = false;
    private float bounceDuration = 0.2f; 
    private float bounceTimer = 0f;

    [SerializeField] private float speed; 
    [SerializeField] private float distance;  // Distance before slowing down
    [SerializeField] private float slowSpeed;
    [SerializeField] private float bounceForce;
    [SerializeField] private float maxChaseDistance = 15f; // Max range to chase
    [SerializeField] private float raycastRange = 20f; // Distance for sight check

    private bool hasLineOfSight = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false; 
        rb.gravityScale = 0;    
        rb.freezeRotation = true; 
    }

    void Update()
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

        if (Target == null) return;

        // Calculate distance to target
        Vector2 targetPosition = Target.transform.position;
        Dis = Vector2.Distance(rb.position, targetPosition);

        // **NEW:** Check for both conditions before stopping movement
        if (!hasLineOfSight || Dis > maxChaseDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Move towards target
        float currentSpeed = (Dis > distance) ? speed : slowSpeed;
        Vector2 direction = (targetPosition - rb.position).normalized;
        rb.linearVelocity = direction * currentSpeed;
    }

    private void FixedUpdate()
    {
        if (Target == null) return;

        // **NEW:** Cast a ray from the enemy to the target
        Vector2 directionToTarget = (Target.transform.position - transform.position).normalized;
        RaycastHit2D ray = Physics2D.Raycast(transform.position, directionToTarget, raycastRange);

        if (ray.collider != null)
        {
            if (ray.collider.gameObject == Target)  // Ensures it's the target, not a wall
            {
                hasLineOfSight = true;
                Debug.DrawRay(transform.position, directionToTarget * raycastRange, Color.green);
            }
            else
            {
                hasLineOfSight = false;
                Debug.DrawRay(transform.position, directionToTarget * raycastRange, Color.red);
            }
        }
        else
        {
            hasLineOfSight = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.linearVelocity *= 0.5f; 
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 bounceDirection = (rb.position - (Vector2)collision.transform.position).normalized;
            rb.linearVelocity = bounceDirection * bounceForce;

            isBouncing = true;
            bounceTimer = bounceDuration;
        }
    }
}
