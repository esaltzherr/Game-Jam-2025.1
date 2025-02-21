using UnityEngine;
using TMPro;

public class CollectibleCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI collectibleText;
    [SerializeField] private int maxCollectibles = 3;
    private int currentCollectibles = 0;

    private void Start()
    {
        UpdateText();
    }

    public void IncrementCollectibles()
    {
        if (currentCollectibles < maxCollectibles)
        {
            currentCollectibles++;
            UpdateText();
        }
    }

    private void UpdateText()
    {
        if (collectibleText != null)
        {
            collectibleText.text = $"{currentCollectibles} / {maxCollectibles}";
        }
    }
}
