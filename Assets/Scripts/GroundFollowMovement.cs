using UnityEngine;
using System.Collections;

public class FollowMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    
    public GameObject Target;
    private float Dis;
    private bool isBouncing = false;
    private float bounceDuration = 0.2f; // Prevents instant movement override
    private float bounceTimer = 0f;

    [SerializeField] private float speed; 
    [SerializeField] private float distance;
    [SerializeField] private float heightTolerance = 1f;
    [SerializeField] private float bounceForce;
    [SerializeField] private float patrolSpeed; 
    [SerializeField] private float patrolDistance = 3f;
    [SerializeField] private float waitPatrol = 2f;

    private Vector2 patrolStart;
    private Vector2 patrolEnd;
    private bool movingRight = true;
    private bool isWaiting = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false; 
        rb.freezeRotation = true;// Prevents physics forces from affecting it

        patrolStart = transform.position;
        patrolEnd = new Vector2(patrolStart.x + patrolDistance, patrolStart.y);
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
            return; // Stop movement while bouncing
        }

        if (Target.transform.position.y - transform.position.y <= heightTolerance)
        {
            FollowPlayer();
        }
        
        else if(!isWaiting)
        {
            Patrol();
        }
    }

    void FollowPlayer()
    {
        Dis = Vector2.Distance(transform.position, Target.transform.position);

        float moveSpeed = (Dis >= distance) ? speed : 1f;

        Vector2 direction = (Target.transform.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
    }

    void Patrol()
    {
        rb.linearVelocity = Vector2.zero;
        if (movingRight)
        {

            rb.linearVelocity = new Vector2(patrolSpeed, rb.linearVelocity.y);
            if (transform.position.x >= patrolEnd.x && !isWaiting)
            {
                StartCoroutine(ChangeDirectionAfterWait());
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(-patrolSpeed, rb.linearVelocity.y);
            if (transform.position.x <= patrolStart.x && !isWaiting)
            {
                StartCoroutine(ChangeDirectionAfterWait());
            }
        }
    }

    IEnumerator Wait()
    {
        isWaiting = true;
        rb.linearVelocity = Vector2.zero; // Stop movement while waiting
        yield return new WaitForSeconds(waitPatrol);
        isWaiting = false;
    }

    IEnumerator ChangeDirectionAfterWait()
    {
        isWaiting = true;
        rb.linearVelocity = Vector2.zero; // Stop movement while waiting
        yield return new WaitForSeconds(waitPatrol);
        movingRight = !movingRight; // Switch direction
        isWaiting = false;
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
