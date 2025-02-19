using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Assign the Player in the Inspector
    public Vector3 offset = new Vector3(0, 1, -10); // Default offset
    public float smoothSpeed = 5f; // Adjust for smoother/faster movement

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
