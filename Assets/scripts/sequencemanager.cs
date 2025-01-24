using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class SequenceManager : MonoBehaviour
{
    public TextMeshProUGUI sequenceText;
    public TextMeshProUGUI timerText;
    public GameObject player;
    public Transform startPoint;
    public GameObject[] collectibleObjects;
    public GameObject[] obstacles;
    public float displayTime = 3f;
    public float timerDuration = 30f;
    public string currentCondition;
    public string[] specifiedSequence;

    private int collectedCount = 0;
    private float timer = 0f;
    private int invalidClicks = 0;

    private Vector3 lastValidPosition; // Tracks the last valid position

    void Start()
    {
        // Set initial player position to the start point
        player.transform.position = startPoint.position;
        lastValidPosition = startPoint.position; // Initialize the last valid position

        // Hide obstacles at the start
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.SetActive(false);
        }

        // Start the sequence display
        StartCoroutine(DisplaySequence());
    }

    IEnumerator DisplaySequence()
    {
        // Hide collectibles and player
        player.SetActive(false);
        foreach (GameObject collectible in collectibleObjects)
        {
            collectible.SetActive(false);
        }

        string sequence = (currentCondition == "Condition2" && specifiedSequence.Length > 0)
            ? string.Join(" -> ", specifiedSequence)
            : string.Join(" -> ", GetCollectibleNames());

        sequenceText.text = $"Memorize the Sequence:\n{sequence}";
        sequenceText.gameObject.SetActive(true);

        Debug.Log($"Sequence to collect: {sequence}");

        yield return new WaitForSeconds(displayTime);

        // Hide sequence text
        sequenceText.gameObject.SetActive(false);

        // Show player, collectibles, and obstacles
        player.SetActive(true);
        foreach (GameObject collectible in collectibleObjects)
        {
            collectible.SetActive(true);
        }
        foreach (GameObject obstacle in obstacles)
        {
            obstacle.SetActive(true);
        }

        // Start the timer
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        timer = timerDuration;

        while (timer > 0)
        {
            timerText.text = $"Time Left: {timer:F1} seconds";
            yield return new WaitForSeconds(0.1f);
            timer -= 0.1f;

            if (collectedCount == collectibleObjects.Length)
            {
                StopTimerAndLogResults();
                yield break;
            }
        }

        Debug.Log("Not all objects were collected. Restarting the level...");
        timerText.text = "Time's up!";
        yield return new WaitForSeconds(3f);
        RestartCurrentLevel();
    }

    public void RecordCollection(GameObject collectible)
    {
        collectedCount++;
        lastValidPosition = collectible.transform.position; // Update the last valid position

        Debug.Log($"Player collected: {collectible.name}");

        if (collectedCount == collectibleObjects.Length)
        {
            StopTimerAndLogResults();
        }
    }

    public void RecordInvalidCollection(string collectibleName)
    {
        invalidClicks++;
        Debug.Log($"Player clicked on an invalid object: {collectibleName}");
    }

    void StopTimerAndLogResults()
    {
        Debug.Log($"All objects collected! Time taken: {timerDuration - timer:F1} seconds.");
        Debug.Log($"Total invalid clicks: {invalidClicks}");
        timerText.text = "All objects collected!";
    }

    void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private string[] GetCollectibleNames()
    {
        string[] names = new string[collectibleObjects.Length];
        for (int i = 0; i < collectibleObjects.Length; i++)
        {
            names[i] = collectibleObjects[i].name;
        }
        return names;
    }

    public Vector3 GetLastValidPosition()
    {
        return lastValidPosition; // Return the last valid position
    }
}
