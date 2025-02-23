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
    public AudioClip[] flapSounds;
    public float cooldownDuration = 0.6f; // Cooldown duration in seconds

    private float nextSoundTime = 0f;   // Tracks the next allowed time to play a sound
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

        Vector3 scale = transform.localScale;
        if (rb.linearVelocity.x < 0)
        {
            // Facing right, ensure x scale is positive.
            scale.x = Mathf.Abs(scale.x);
        }
        else if (rb.linearVelocity.x > 0)
        {
            // Facing left, flip the x scale.
            scale.x = -Mathf.Abs(scale.x);
        }
        transform.localScale = scale;



        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            // Only play sound if the cooldown period has passed
            if (Time.time >= nextSoundTime)
            {
                if (flapSounds.Length > 0)
                {
                    // Select a random sound from the array
                    AudioClip clip = flapSounds[Random.Range(0, flapSounds.Length)];
                    AudioManager.Instance.PlaySFX(clip);

                    // Set the next allowed time to play a sound
                    nextSoundTime = Time.time + cooldownDuration;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Target == null) return;

        // Cast a ray from the enemy to the target
        Vector2 directionToTarget = (Target.transform.position - transform.position).normalized;

        // Perform a raycast that hits multiple objects
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionToTarget, raycastRange);

        foreach (RaycastHit2D hit in hits)
        {
            // Ignore itself
            if (hit.collider.gameObject == gameObject) continue;

            // Debug.Log("Ray hit: " + hit.collider.gameObject.name + " | Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));

            if (hit.collider.gameObject == Target)  // Ensure it's the target
            {
                hasLineOfSight = true;
                Debug.DrawRay(transform.position, directionToTarget * raycastRange, Color.green);
                return; // Stop checking after hitting the target
            }
            else
            {
                hasLineOfSight = false;
                Debug.DrawRay(transform.position, directionToTarget * raycastRange, Color.red);
                return; // Stop checking after hitting an obstacle
            }
        }

        // If nothing was hit, no line of sight
        hasLineOfSight = false;
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

    public void Scared(Transform player)
    {
        Debug.Log("GETTIN SPOOKED");
        Vector2 bounceDirection = (rb.position - (Vector2)player.transform.position).normalized;
        rb.linearVelocity = bounceDirection * bounceForce;

        isBouncing = true;
        bounceTimer = bounceDuration;
    }



}
