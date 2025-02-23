using UnityEngine;

public class Jeremy : MonoBehaviour
{
    public GameObject Target;
    [SerializeField] private float speed = 3f; // Movement speed

    private Rigidbody2D rb;
    public AudioClip screech;
    public Transform startingPos; // Corrected type

    // Updated references to the correct script classes
    public Movement movement;
    public Cave_Checkpoint cavecheckpoint;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        AudioManager.Instance.PlaySFX(screech);
    }

    void Update()
    {
        if (Target == null) return;

        // Move towards the player continuously
        Vector2 direction = (Target.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched!");
            movement.health = 0;
            cavecheckpoint.Setjermy(true);
            // Move Jeremy back to its starting position
            transform.position = startingPos.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cavecheckpoint.Setjermy(false);
        }
    }
}
