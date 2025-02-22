using UnityEngine;
using UnityEngine.Rendering.Universal; // Required for Light2D

public class Home : MonoBehaviour
{
    public GameObject torch;             // The torch GameObject
    public GameObject torchCanvas;       // The canvas for the torch UI
    public GameObject globalLightObject; // The GameObject that contains the Light2D component

    private Light2D globalLight;         // Reference to the Light2D component on the global light object

    // Boolean flag to ensure torch activation and light change only happen once
    private bool torchActivated = false;

    private void Awake()
    {
        if (globalLightObject != null)
        {
            globalLight = globalLightObject.GetComponent<Light2D>(); // Corrected to Light2D
            if (globalLight == null)
            {
                Debug.LogWarning("globalLightObject does not have a Light2D component attached!");
            }
        }
        else
        {
            Debug.LogWarning("globalLightObject is not assigned in the inspector.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Always enable TorchBar filling
            TorchBar torchBar = other.GetComponentInChildren<TorchBar>();
            if (torchBar != null)
            {
                torchBar.enableFilling();
            }

            // Activate the torch, canvas, and change global light intensity only once
            if (!torchActivated)
            {
                torch.SetActive(true);
                torchCanvas.SetActive(true);

                if (globalLight != null)
                {
                    globalLight.intensity = 1f;
                }
                torchActivated = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Always disable TorchBar filling when the player exits
            TorchBar torchBar = other.GetComponentInChildren<TorchBar>();
            if (torchBar != null)
            {
                torchBar.disableFilling();
            }
        }
    }
}
