using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Application.Quit();
    }
}
