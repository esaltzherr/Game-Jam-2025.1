using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Import UI library

public class Cave_Checkpoint : MonoBehaviour
{
    private Vector2 checkpointPos;
    private Movement player;
    public Text deathText; // Reference to UI text

    void Start()
    {
        checkpointPos = transform.position; // Initial spawn point
        player = FindObjectOfType<Movement>(); // Find the player

        if (deathText != null)
            deathText.gameObject.SetActive(false); // Hide text at start
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkpointPos = collision.transform.position;
            checkpointPos.y += 3; // Move checkpoint up
        }
    }

    IEnumerator Respawn(float duration)
    {
        if (deathText != null)
            deathText.gameObject.SetActive(true); // Show text when player dies

        Debug.Log("Respawning...");

        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        playerRb.linearVelocity = Vector2.zero;
        playerRb.simulated = false;

        player.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(duration);

        // Respawn at checkpoint
        player.transform.position = checkpointPos;
        player.transform.localScale = Vector3.one;
        playerRb.simulated = true;

        player.ResetHealth(); // Restore health

        if (deathText != null)
            deathText.gameObject.SetActive(false); // Hide text after respawn
    }

    void Update()
    {
        if (player != null && player.health <= 0)
        {
            StartCoroutine(Respawn(2f)); // Respawn after 2 seconds
        }
    }
}
