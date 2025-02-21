using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform; // The layer object
        public float parallaxMultiplier; // How much this layer moves relative to player movement
    }

    public ParallaxLayer[] layers; // Array of parallax layers
    public Transform player; // The player to track movement

    private Vector3 previousPlayerPosition;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform not assigned! Assign the player in the Inspector.");
            return;
        }

        previousPlayerPosition = player.position;
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 deltaMovement = player.position - previousPlayerPosition;

        foreach (ParallaxLayer layer in layers)
        {
            if (layer.layerTransform != null)
            {
                Vector3 newPosition = layer.layerTransform.position;
                newPosition.x += deltaMovement.x * layer.parallaxMultiplier;
                newPosition.y += deltaMovement.y * layer.parallaxMultiplier;
                layer.layerTransform.position = newPosition;
            }
        }

        previousPlayerPosition = player.position; // Update player position
    }
}
