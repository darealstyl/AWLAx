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

    Queue<string> lines;
    string[] linesArray = {
        "In a tranquil underwater haven, there lived a curious and spirited axolotl. One day, driven by an adventurous itch, it ventured out to explore a legendary swamp, whispered among its kin as a place of strange beauty and eerie wonder.\n<Click to continue>",
        "En route to this mythical swamp, the axolotl, entranced by the underwater landscapes, strayed from its path and became hopelessly lost. Its journey took an unexpected turn as it wandered into a bizarre and unforgiving world. This new environment was treacherous, filled with toxic air and mysterious, menacing entities.\n<Click to continue>",
        "Fortunately, this was no ordinary axolotl. It possessed a rare genetic trait that granted it control over its sympathetic nervous system. This ability enabled the axolotl to perform explosive dashes, propelling itself forward with bursts of adrenaline and cortisol, vital for navigating this perilous world.\n<Click to continue>",
        "These dashes, while powerful, taxed the axolotl's body, causing significant damage. But nature had equipped it with another remarkable gift: an enhanced regenerative capability, allowing it to recover from the physical toll of its rapid movements.\n<Click to continue>",
        "Now, the axolotl's simple desire to explore the swamp transformed into a critical mission: to find its way back home. It must rely on its unique abilities to avoid dangers, escape the toxic environment, and outsmart the mysterious creatures that stand in its way.\n<Click to continue>",
        "As the player, you are tasked with guiding the axolotl through this hostile world. Utilize its swift dashes and rapid healing to overcome obstacles and hazards. The journey is a testament to survival and the longing for home, as the axolotl seeks a path back to its familiar, peaceful waters. \n<Click to start tutorial>"
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
