using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float jump;
    [SerializeField] private float bounceForce;       // Bounce force for enemies
    [SerializeField] private float hazardBounceForce; // Stronger/weaker bounce force for hazards
    [SerializeField] private float shrinkScaleY;
    [SerializeField] private float normalScaleY;
    [SerializeField] private float slideSpeedMultiplier = 2f;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float healCooldown = 5f;

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

    public bool TimerOn = false;
    private int health;
    private float healTimer;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
        originalScale = transform.localScale;

        originalSpeed = speed;
        crouchSpeed = speed / 2f;
        health = maxHealth;
        healTimer = healCooldown;
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

        //Heal factor
        TimerOn = false;

        if(health < 3)
        {
            TimerOn = true;
            if(TimerOn)
            {
                if(healTimer > 0)
                {
                    healTimer -= Time.deltaTime;
                }
                else
                {
                    health ++;
                    healTimer = healCooldown;
                    Debug.Log("Healing! Health: " + health);
                    TimerOn = false;
                }
            }
        }

        // Sprinting logic
        if (grounded && Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            running = true;
            moveSpeed *= sprintMultiplier;
        }
        else if (!grounded && running && Input.GetKey(KeyCode.LeftShift))
        {
            running = true;
            moveSpeed *= sprintMultiplier;
        }
        else
        {
            running = false;
        }

        // Sliding
        if (running && Input.GetKeyDown(KeyCode.S))
        {
            StartSlide();
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            if (slideTimer <= 0)
            {
                EndSlide();
            }
            return;
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
        Debug.Log("Jumping");
        body.linearVelocity = new Vector2(body.linearVelocity.x, jump);
        grounded = false;
    }

    private void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        Debug.Log("Sliding!");

        body.linearVelocity = new Vector2(body.linearVelocity.x * slideSpeedMultiplier, body.linearVelocity.y);
        transform.localScale = new Vector3(originalScale.x, shrinkScaleY, originalScale.z);
    }

    private void EndSlide()
    {
        isSliding = false;
        transform.localScale = new Vector3(originalScale.x, normalScaleY, originalScale.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(collision, bounceForce);
        }
        else if (collision.gameObject.CompareTag("Hazard"))
        {
            TakeDamage(collision, hazardBounceForce);
        }
    }

    private void TakeDamage(Collision2D collision, float force)
    {
        health--;
        Debug.Log("Health: " + health);
        if (health <= 0)
        {
            GameOver();
            return;
        }

        // Reset heal timer
        healTimer = healCooldown;

        // Knockback effect
        BounceAway(collision, force);
    }


    private void GameOver()
    {
        Debug.Log("Game Over!");
    }

    private void BounceAway(Collision2D collision, float force)
    {
        Vector2 bounceDirection = (body.position - (Vector2)collision.transform.position).normalized;

        // Ensure the bounce is mostly upwards
        if (bounceDirection.y < 0.3f)
        { 
        
            bounceDirection.y = 0.75f; // Ensure a reasonable upwards push
        }
        bounceDirection.Normalize();

        body.linearVelocity = bounceDirection * force; // Directly set velocity instead of using AddForce

        isBouncing = true;
        bounceTimer = bounceDuration;
    }
}
