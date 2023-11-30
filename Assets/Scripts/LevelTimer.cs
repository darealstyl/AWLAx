using UnityEngine;
using UnityEngine.UI;

public class LevelTimer : MonoBehaviour
{
    public bool levelStarted { get; set; }
    public float timeToBeat = 45f;
    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI startText;
    public float currentTime;
    [SerializeField] FinishLine fl;

    void Start()
    {
        // Set levelStarted to false at the start
        levelStarted = false;
        // You can also trigger any other actions that need to happen when the level starts
        if (startText != null)
            startText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check if the spacebar was pressed and the level has not started yet
        if (Input.GetKeyDown(KeyCode.Space) && !levelStarted && !fl.isFinished)
        {
            StartLevel();
        }

        // If the level has started, update the timer
        if (levelStarted)
        {
            currentTime = timeToBeat - Time.time;
            // You can use currentTime to display the timer on the screen
            // Update the timerText with the current time
            timerText.text = "Time left: " + currentTime.ToString("F2");
        }
    }

    void StartLevel()
    {
        // Set levelStarted to true and record the start time
        levelStarted = true;
        timeToBeat += Time.time;
        // You can also trigger any other actions that need to happen when the level starts
        if (startText != null)
            startText.gameObject.SetActive(false);
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
}
