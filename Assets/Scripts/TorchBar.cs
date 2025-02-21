using UnityEngine;
using UnityEngine.UI;

public class TorchBar : MonoBehaviour
{
    public Slider torchSlider; // Drag & drop the UI Slider in Inspector
    public float maxTorchTime = 60f; // Maximum torch duration in seconds
    private float currentTorchTime;
    private bool filling = false;
    void Start()
    {
        currentTorchTime = maxTorchTime;
        if (torchSlider != null)
        {
            torchSlider.maxValue = maxTorchTime;
            torchSlider.value = maxTorchTime;
        }
        else
        {
            Debug.LogError("TorchBar: No Slider assigned!");
        }
    }

    void Update()
    {
        if(filling && currentTorchTime < maxTorchTime){
            currentTorchTime += Time.deltaTime * 5;

        }
        else if(!filling && currentTorchTime > 0)
        {
            currentTorchTime -= Time.deltaTime; 
        }
        torchSlider.value = currentTorchTime;
    }

    public void enableFilling(){
        filling = true;
        
    }
    public void disableFilling(){
        filling = false;
    }
    // Get the remaining percentage of the torch
    public float GetTorchPercentage()
    {
        return (currentTorchTime / maxTorchTime) * 100f;
    }
}
