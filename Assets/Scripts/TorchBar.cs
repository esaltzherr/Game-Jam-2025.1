using UnityEngine;
using UnityEngine.UI;

public class TorchBar : MonoBehaviour
{
    public Slider torchSlider; // Drag & drop the UI Slider in Inspector
    public float maxTorchTime = 60f; // Maximum torch duration in seconds
    private float currentTorchTime;
    private bool filling = false;
    public Animator playerAnimator;
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
        if (currentTorchTime > 0){
            playerAnimator.SetBool("Lit", true);

        }
        else{
            playerAnimator.SetBool("Lit", false);

        }
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
