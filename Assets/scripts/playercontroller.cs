using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float stationaryThreshold = 3f; // Time in seconds before snapping back
    private float stationaryTimer = 0f;
    private Vector3 lastPosition;

    private SequenceManager sequenceManager;

    void Start()
    {
        sequenceManager = FindObjectOfType<SequenceManager>();
        lastPosition = transform.position; // Initialize with the starting position
    }

    void Update()
    {
        HandleMovement();
        CheckStationary();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0) * speed * Time.deltaTime;
        transform.position += movement;

        // Reset stationary timer if the player moves
        if (movement != Vector3.zero)
        {
            stationaryTimer = 0f;
            lastPosition = transform.position; // Update the last position after movement
        }
    }

    void CheckStationary()
    {
        if (transform.position == lastPosition) // Player is stationary
        {
            stationaryTimer += Time.deltaTime;

            if (stationaryTimer >= stationaryThreshold)
            {
                // Snap player back to last valid position
                Vector3 validPosition = sequenceManager.GetLastValidPosition();
                transform.position = validPosition;

                // Reset the last position to avoid immediate re-trigger
                lastPosition = validPosition;

                //Debug.Log($"Player returned to last valid position: {validPosition}");
                stationaryTimer = 0f; // Reset the timer
            }
        }
        else
        {
            stationaryTimer = 0f; // Reset the timer if the player moves
        }
    }
}
