using UnityEngine;
using UnityEngine.UI;

public class TorchBar : MonoBehaviour
{
    public Slider torchSlider; // Drag & drop the UI Slider in Inspector
    public float maxTorchTime = 250f; // Maximum torch duration in seconds
    private float currentTorchTime;
    private bool filling = false;
    public Animator playerAnimator;
    public GameObject jeremy;
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
        playerAnimator.SetBool("Lit", false);
    }

    void Update()
    {
        if(filling && currentTorchTime < maxTorchTime){
            currentTorchTime += Time.deltaTime * 20;

        }
        else if(!filling && currentTorchTime > 0)
        {
            currentTorchTime -= Time.deltaTime; 
        }
        torchSlider.value = currentTorchTime;
        if (currentTorchTime > 0){
            playerAnimator.SetBool("Lit", true);
            jeremy.SetActive(false);
        }
        else{
            playerAnimator.SetBool("Lit", false);
            jeremy.SetActive(true);
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
    public float GetTorchNum()
    {
        return currentTorchTime;
    }
}
