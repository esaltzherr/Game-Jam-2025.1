using UnityEngine;

public class WoodCollectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the correct tag
        {
            CollectibleCounter counter = FindObjectOfType<CollectibleCounter>();
            if (counter != null)
            {
                counter.IncrementCollectibles();
            }
            Destroy(gameObject); // Remove the wood collectible from the scene
        }
    }
}
