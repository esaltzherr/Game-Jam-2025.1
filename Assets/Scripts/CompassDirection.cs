using UnityEngine;

public class CompassDirection : MonoBehaviour
{
    public string homeTag = "Home"; // Tag for the home location
    public string playerTag = "Player"; // Tag for the player
    private Transform homeTransform;
    private Transform playerTransform;
    
    void Start()
    {
        // Find the GameObjects with the given tags
        GameObject homeObject = GameObject.FindGameObjectWithTag(homeTag);
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);

        if (homeObject != null && playerObject != null)
        {
            homeTransform = homeObject.transform;
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Home or Player object not found! Make sure they have the correct tags.");
        }
    }

    void Update()
    {
        if (homeTransform == null || playerTransform == null)
            return;

        // Calculate direction from Player to Home
        Vector3 direction = homeTransform.position - playerTransform.position;
        direction.z = 0; // Ensure it's only in 2D space

        // Calculate the angle (convert direction to rotation)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation to the UI Image (assuming it's on a Canvas)
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
