using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SceneFader sceneFader;
    public void StartGame()
    {
        //SceneManager.LoadScene(1);
        sceneFader.FadeToScene(1);
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void Quit()
    {
        // If we're running in the Unity Editor
#if UNITY_EDITOR
        // Stop playing the scene in the editor
        EditorApplication.isPlaying = false;
#endif

        // Quit the application
        Application.Quit();
    }
}
