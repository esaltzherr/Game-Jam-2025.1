using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cave_Checkpoint : MonoBehaviour
{
    private Vector2 checkpointPos;
    private Movement player;
    private int branchCount; // Track branches collected

    public GameObject loseMessage; // UI text for death message
    public GameObject loseMessage2; // UI text for death message
    public Image fadeScreen; // UI Image for fade to black effect
    public GameObject winMessage; // Win message (Text/Image)

    private bool gameWon = false;
    bool diedToJermy = false;

    void Start()
    {
        checkpointPos = transform.position; // Initial spawn point
        player = FindObjectOfType<Movement>();

        if (loseMessage != null)
            loseMessage.SetActive(false);

        if (loseMessage2 != null)
            loseMessage2.SetActive(false);


        if (fadeScreen != null)
            fadeScreen.color = new Color(0, 0, 0, 0); // Transparent at start

        if (winMessage != null)
            winMessage.SetActive(false); // Hide win message initially
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkpointPos = collision.transform.position;
            checkpointPos.y += 3; // Move checkpoint up

            if (player.branchCount >= 3) // Win condition
            {
                StartCoroutine(WinGame());
            }
        }
    }
    public void Setjermy(bool deadTo){
        diedToJermy = deadTo;
    }

    IEnumerator Respawn(float duration, bool diedToJermy)
    {
        if (diedToJermy)
        {
            loseMessage2.SetActive(true);
        }
        else
        {
            if (loseMessage != null)
                loseMessage.SetActive(true); // Show text when player dies
        }


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
        player.ResetGame(); // Restore branches

        if (loseMessage != null)
            loseMessage.SetActive(false); // Hide text after respawn
    }

    void Update()
    {
        if (player != null && player.health <= 0)
        {
            StartCoroutine(Respawn(2f, diedToJermy)); // Respawn after 2 seconds
        }

    }

    IEnumerator WinGame()
    {
        if (gameWon) yield break; // Prevent multiple triggers
        gameWon = true;

        Debug.Log("Game Won!");

        float fadeDuration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            fadeScreen.color = new Color(0, 0, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadeScreen.color = new Color(0, 0, 0, 1); // Fully black

        if (winMessage != null)
            winMessage.SetActive(true); // Show win message

        yield return new WaitForSeconds(3f);
        Debug.Log("Game Ended!");
    }
}
