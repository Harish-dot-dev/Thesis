using UnityEngine;

public class Collectible : MonoBehaviour
{
    private bool isPlayerNearby = false; // Tracks if the player is within range
    public bool isPartOfSequence = true; // Mark if this collectible is valid for the sequence

    private SequenceManager sequenceManager;

    private void Start()
    {
        // Find the SequenceManager in the scene
        sequenceManager = FindObjectOfType<SequenceManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }

    private void OnMouseDown()
    {
        // Ensure the player is nearby before registering a click
        if (isPlayerNearby)
        {
            if (isPartOfSequence)
            {
                // Valid collectible
                sequenceManager.RecordCollection(gameObject); // Pass the GameObject itself
                gameObject.SetActive(false); // Hide valid collectible after collection
            }
            else
            {
                // Invalid collectible
                sequenceManager.RecordInvalidCollection(gameObject.name); // Pass the name of the GameObject

                // Keep invalid objects visible in Condition2
                if (sequenceManager.currentCondition == "Condition2")
                {
                    Debug.Log($"Invalid object clicked: {gameObject.name} (remains visible).");
                    // Do nothing, leave the object visible
                }
                else
                {
                    // Hide invalid object for other conditions
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
