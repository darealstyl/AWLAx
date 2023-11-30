using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectPanel;

    [SerializeField] Button[] levelButtons;

    private bool isMoving;
    private float moveSpeed = 900f;
    private Vector3 mainMenuStart;
    private Vector3 levelSelectStart;
    private Vector3 mainMenuTarget;
    private Vector3 levelSelectTarget;
    private float movementTime = 0f;
    private float totalMovementDuration = 1f; // Duration of the entire movement

    public GameObject TutComp;
    public GameObject Comp1;
    public GameObject Comp2;

    private void Start()
    {
        if (mainMenuPanel != null && levelSelectPanel != null)
        {
            mainMenuPanel.transform.position = Vector3.zero;
            levelSelectPanel.transform.position = new Vector3(Screen.width, 0, 0);
        }

        if (StaticManager.hasDoneLevel1)
        {
            Comp1.SetActive(true);
        }
        else
        {
            Comp1.SetActive(false);
        }

        if (StaticManager.hasDoneLevel2)
        {
            Comp2.SetActive(true);
        }
        else
        {
            Comp2.SetActive(false);
        }

        if (StaticManager.hasDoneTutorial)
        {
            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i].interactable = true;
            }
            TutComp.SetActive(true);
        }
        else
        {
            for (int i = 0; i < levelButtons.Length; i++)
            {
                if (i == 0)
                {
                    levelButtons[i].interactable = true;
                }
                else
                {
                    levelButtons[i].interactable = false;
                }
            }
            TutComp.SetActive(false);
        }

    }

    public void StartGame(int i)
    {
        //SceneManager.LoadScene(1);
        if (i == 1)
        {
            StaticManager.hasDoneTutorial = true;
        }
        sceneFader.FadeToScene(i);
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void MoveToLevelSelect()
    {
        mainMenuStart = mainMenuPanel.transform.position;
        levelSelectStart = levelSelectPanel.transform.position;

        mainMenuTarget = new Vector3(-Screen.width, 0, 0);
        levelSelectTarget = Vector3.zero;

        movementTime = 0f;
        isMoving = true;
    }

    public void BackToMainMenu()
    {
        mainMenuStart = mainMenuPanel.transform.position;
        levelSelectStart = levelSelectPanel.transform.position;

        mainMenuTarget = Vector3.zero;
        levelSelectTarget = new Vector3(Screen.width, 0, 0);

        movementTime = 0f;
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            movementTime += Time.deltaTime;
            float progress = movementTime / totalMovementDuration;
            float easedProgress = Mathf.SmoothStep(0, 1, progress); // Easing using SmoothStep

            mainMenuPanel.transform.position = Vector3.Lerp(mainMenuStart, mainMenuTarget, easedProgress);
            levelSelectPanel.transform.position = Vector3.Lerp(levelSelectStart, levelSelectTarget, easedProgress);

            if (progress >= 1f)
            {
                isMoving = false;
            }
        }
    }
}
