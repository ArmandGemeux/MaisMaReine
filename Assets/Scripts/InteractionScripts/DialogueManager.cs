using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterLines;

    private Animator myAnim;

    private Queue<string> lines;

    #region Singleton
    public static DialogueManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        lines = new Queue<string>();
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDialogueWindow(Dialogue dialogue)
    {
        myAnim.SetBool("isOpen", true);

        characterName.text = dialogue.name;

        lines.Clear();

        foreach (string line in dialogue.myDialogue)
        {
            lines.Enqueue(line);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
        else
        {
            // ICI jouer SFX de dialogue qui passe à l'étape suivante
        }

        string line = lines.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeLines(line));
    }

    IEnumerator TypeLines(string line)
    {
        characterLines.text = "";
        foreach (char letter  in line.ToCharArray())
        {
            characterLines.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        /*if (questToSetActive != null)
        {
            questToSetActive.isActive = true;
            //QuestManager.Instance.AddNewQuestToFollow(questToSetActive);
        }*/

        // ICI jouer SFX de fin de dialogue
        myAnim.SetBool("isOpen", false);
    }
}
