using UnityEngine;
using UnityEngine.UI;

public class DeathUIManager : MonoBehaviour
{
    public PlayerController playerController; // Assign in the inspector
    public TMPro.TextMeshProUGUI deathText; // Assign your 'You Died' Text component in the inspector
    public LevelTimer levelTimer; // Assign your LevelTimer script in the inspector
    public TMPro.TextMeshProUGUI finalTimeText; // Assign your final time Text component in the inspector

    private bool hasPlayerDied = false;

    void Start()
    {
        // Make sure the 'You Died' text and final time text is not visible at the start
        if (deathText != null) deathText.gameObject.SetActive(false);
        if (finalTimeText != null) finalTimeText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Only update the UI once after the player has died
        if (!hasPlayerDied && playerController.death)
        {
            hasPlayerDied = true; // Prevents this block from running again

            // Show the 'You Died' text
            if (deathText != null)
            {
                deathText.gameObject.SetActive(true);
            }

            // Show the final time
            if (finalTimeText != null && levelTimer != null)
            {
                finalTimeText.text = "Final Time: " + levelTimer.GetCurrentTime(); // Assuming GetFinalTime() is your method for getting the time
                finalTimeText.gameObject.SetActive(true);
            }
        }
    }
}
