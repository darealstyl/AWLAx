using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    public bool levelStarted { get; private set; }
    private float startTime;
    public TMPro.TextMeshProUGUI timerText;

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check if the spacebar was pressed and the level has not started yet
        if (Input.GetKeyDown(KeyCode.Space) && !levelStarted)
        {
            StartLevel();
        }

        // If the level has started, update the timer
        if (levelStarted)
        {
            float currentTime = Time.time - startTime;
            // You can use currentTime to display the timer on the screen
            // Update the timerText with the current time
            timerText.text = "Timer: " + currentTime.ToString("F2");
        }
    }

    void StartLevel()
    {
        // Set levelStarted to true and record the start time
        levelStarted = true;
        startTime = Time.time;
        // You can also trigger any other actions that need to happen when the level starts
    }
}
