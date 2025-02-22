using UnityEngine;

public class Home : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            TorchBar torchBar = other.GetComponentInChildren<TorchBar>();
            if (torchBar != null)
            {
                torchBar.enableFilling();
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            // Search for TorchBar in the Player's children
            TorchBar torchBar = other.GetComponentInChildren<TorchBar>();
            if (torchBar != null)
            {
                torchBar.disableFilling();
            }
         
        }
    }
}
