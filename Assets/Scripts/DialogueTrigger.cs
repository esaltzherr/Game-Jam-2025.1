using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueText; // Drag & drop your UI text here

    private void Start()
    {
        if (dialogueText != null)
        {
            dialogueText.SetActive(false); // Ensure it's hidden at the start
        }
        else
        {
            Debug.LogError("DialogueTrigger: No dialogue text assigned!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (dialogueText != null)
            {
                dialogueText.SetActive(true); // Show the text
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (dialogueText != null)
            {
                dialogueText.SetActive(false); // Hide the text
            }
        }
    }
}
