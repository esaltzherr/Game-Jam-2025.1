using UnityEngine;

public class FlyingFollowMovement : MonoBehaviour
{
    public GameObject Target;
    private float Dis;

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
        if (collision.gameObject.tag == "Player")
        {
            // Slight bounce effect when hitting character
            Debug.Log("Hit!");
            Vector2 bounceDirection = collision.contacts[0].normal * bounceForce; 
            rb.AddForce(bounceDirection, ForceMode2D.Impulse);
        }
    }
}
