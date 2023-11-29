using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryPanelManager : MonoBehaviour
{
    public SceneFader sceneFader;
    public TextMeshProUGUI dialogueText;
    public float textSpeed = 2.0f;

    int lastTouchCount = 0;
    Queue<string> lines;
    string[] linesArray = {
        "One peaceful morning you, an axolotl, decided to go on an adventure because you were bored. <Click to continue>",
        "Somehow you ended up in a hostile world where the air is polluted with toxic compounds which will kill you if you're exposed for too long. <Click to continue>",
        "On top of that there are also unknown entities trying to block your escape. <Click to continue>",
        "Luckily, you aren't totally screwed.",
        "That's because you realized that you are not like the other axolotls you grew up around. <Click to continue>",
        "You were born a with condition that allows you to have full control over your sympathetic nervours system. <Click to continue>",
        "By inducing a fight-or-flight response, an excessive amount of adrenaline and cortisol are released, allowing you to perform an explosive dash. <Click to continue>",
        "However the intense stress it puts on your body leads to significant cellular damage. <Click to continue>",
        "Thankfully, you were also born with regenerative capabilities far greater than axolotls normally have. <Click to continue>",
        "Hopefully, with these abilities you can escape this place fast enough before the toxins in the air kill you. <Click to start tutorial>"
    };
    // Start is called before the first frame update
    void Start()
    {
        lines = new Queue<string>(linesArray);
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DisplayNextSentence();
        }

    }

    // Also called in Update()
    public void DisplayNextSentence()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = lines.Dequeue();
        Debug.Log(sentence);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    // Type sentence out letter by letter
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1.0f / textSpeed);
        }
    }

    void StartDialogue()
    {
        Debug.Log("Starting diaogue");
        DisplayNextSentence();
    }

    void EndDialogue()
    {
        sceneFader.FadeToScene(2);
        Debug.Log("Ending diaogue");
    }


    public void Next()
    {
        DisplayNextSentence();
    }
}
