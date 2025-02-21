using UnityEngine;
using System.Collections.Generic;
using TMPro; // Import TextMeshPro

public class CompassDirection : MonoBehaviour
{
    public Transform homeTarget; // Always the home location
    public List<Transform> targetList = new List<Transform>(); // List of available targets
    public TextMeshProUGUI targetText; // UI Text to update with the current target's name

    private Transform currentTarget; // The target the compass is actively pointing at
    private int targetIndex = -1; // Index to track which target is currently active
    private bool isPointingHome = true; // Whether we're pointing to home or a target
    private Transform playerTransform;

    void Start()
    {
        // Find the Player object
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player object not found! Make sure it has the correct tag.");
        }

        // Start pointing at home
        currentTarget = homeTarget;
        UpdateTargetText();
    }

    void Update()
    {
        if (currentTarget == null)
        {
            // Reset targetIndex if the current target was removed
            currentTarget = homeTarget;
            isPointingHome = true;
            // Clean up the list by removing any null objects
            targetList.RemoveAll(t => t == null);

            UpdateTargetText(); // Update the text when a target is removed
            return;
        }
        // Calculate direction from Player to currentTarget
        Vector3 direction = currentTarget.position - playerTransform.position;
        direction.z = 0; // Ensure it's only in 2D space

        // Calculate the angle (convert direction to rotation)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Apply rotation to the UI Image (assuming it's on a Canvas)
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Swap target if the player presses Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CycleTarget();
        }
    }

    // Cycles between Home and the next valid target in the list
    private void CycleTarget()
    {
        if (isPointingHome)
        {
            // Move to the next available target
            if (targetList.Count > 0)
            {
                targetIndex = GetNextValidTargetIndex(targetIndex);
                if (targetIndex != -1)
                {
                    currentTarget = targetList[targetIndex];
                }
                else
                {
                    currentTarget = homeTarget; // No valid target found, return home
                }
            }
            else
            {
                currentTarget = homeTarget; // No targets available
            }
        }
        else
        {
            // Go back to home
            currentTarget = homeTarget;
        }

        // Toggle state
        isPointingHome = !isPointingHome;
        UpdateTargetText(); // Update the text when switching targets
    }

    // Gets the next valid (non-null) target index
    private int GetNextValidTargetIndex(int startIndex)
    {
        int count = targetList.Count;
        if (count == 0) return -1; // No valid targets

        int newIndex = (startIndex + 1) % count;

        // Loop through the list until a valid target is found
        for (int i = 0; i < count; i++)
        {
            if (targetList[newIndex] != null) return newIndex;
            newIndex = (newIndex + 1) % count;
        }

        return -1; // No valid target found
    }

    // Adds a new target to the list (objects can call this when collected/found)
    public void AddTarget(Transform newTarget)
    {
        if (newTarget != null && !targetList.Contains(newTarget))
        {
            targetList.Add(newTarget);
        }
    }

    // Removes a target from the list and prevents errors when it gets destroyed
    public void RemoveTarget(Transform target)
    {
        if (targetList.Contains(target))
        {
            targetList.Remove(target);

            // Reset targetIndex if the current target was removed
            if (currentTarget == target)
            {
                currentTarget = homeTarget;
                isPointingHome = true;
            }
        }

        // Clean up the list by removing any null objects
        targetList.RemoveAll(t => t == null);

        UpdateTargetText(); // Update the text when a target is removed
    }

    // Updates the UI text with the current target's name
    private void UpdateTargetText()
    {
        if (targetText != null)
        {
            targetText.text = isPointingHome ? "Home" : currentTarget.name;
        }
    }
}
