using UnityEngine;

public class Home : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger Enter Detected: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered home trigger!");
            TorchBar torchBar = other.GetComponentInChildren<TorchBar>();
            if (torchBar != null)
            {
                torchBar.enableFilling();
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Exits");
        if (other.CompareTag("Player"))
        {

            // Search for TorchBar in the Player's children
            TorchBar torchBar = other.GetComponentInChildren<TorchBar>();
            if (torchBar != null)
            {
                torchBar.disableFilling();
            }
            else
            {
                Debug.Log("NO FIND");
            }
        }
    }
}
