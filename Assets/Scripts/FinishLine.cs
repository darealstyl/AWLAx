using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    public PlayerController playerController; // Assign in the inspector
    public TMPro.TextMeshProUGUI levelCompleteText; // Assign your 'Level Complete' Text component in the inspector
    public LevelTimer levelTimer; // Assign your LevelTimer script in the inspector
    public TMPro.TextMeshProUGUI finalTimeText; // Assign your final time Text component in the inspector
    [SerializeField] private SceneFader sceneFader;
    public bool isFinished;

    public BackgroundMusicManager musicManager;
    private void Start()
    {
        // Make sure the 'Level Complete' text and final time text is not visible at the start
        if (levelCompleteText != null) levelCompleteText.gameObject.SetActive(false);
        if (finalTimeText != null) finalTimeText.gameObject.SetActive(false);
        isFinished = false;
    }

    private void Update()
    {
        if (isFinished && Input.GetKeyDown(KeyCode.Escape))
        {
            sceneFader.FadeToScene(0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            musicManager.PlayWinSFX();
            // Stop player activity
            // Assuming the player has a script named 'PlayerController' that controls movement
            Rigidbody2D rb = playerController.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero; // Stop all movement
                rb.isKinematic = true; // Prevent further physics interactions
            }
            playerController.enabled = false;
            // Display 'Level Complete' text
            if (levelCompleteText != null)
            {
                levelCompleteText.gameObject.SetActive(true);
            }

            levelTimer.levelStarted = false;

            // Display the final time
            if (finalTimeText != null && levelTimer != null)
            {
                finalTimeText.text = "Final Time: " + levelTimer.GetCurrentTime(); // Assuming GetCurrentTime() is your method for getting the time
                finalTimeText.gameObject.SetActive(true);
            }
            isFinished = true;
        }


    }
}
