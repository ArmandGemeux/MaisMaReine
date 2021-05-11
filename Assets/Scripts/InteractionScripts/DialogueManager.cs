﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterLines;

    [Header("Dialogue début territoire")]

    public bool isStartDialogueExisting;
    public Dialogue dialogueDebutTerritoire;

    private Dialogue currentDialogue;

    private Animator myAnim;
    private FideleManager talkingFM;

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

        if (isStartDialogueExisting)
        {
            OpenDialogueWindow(dialogueDebutTerritoire, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDialogueWindow(Dialogue dialogue, FideleManager talkedFM)
    {
        if (talkedFM != null)
        {
            talkingFM = talkedFM;
        }

        GameManager.Instance.isGamePaused = true;
        myAnim.SetBool("isOpen", true);

        currentDialogue = dialogue;
        characterName.text = currentDialogue.name;

        if (currentDialogue.cameraStartPos != null)
        {
            DragCamera2D.Instance.FollowTargetCamera(currentDialogue.cameraStartPos);
        }

        lines.Clear();

        foreach (string line in currentDialogue.myDialogue)
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
        // ICI jouer SFX de fin de dialogue
        GameManager.Instance.isGamePaused = false;
        myAnim.SetBool("isOpen", false);

        if (currentDialogue.cameraEndPos != null)
        {
            DragCamera2D.Instance.FollowTargetCamera(currentDialogue.cameraEndPos);
        }

        if (currentDialogue.isStartingQuest)
        {
            QuestManager.Instance.SetupQuest(currentDialogue.questIndexToStart);

            foreach (FideleManager fmToSpawn in currentDialogue.unitsToSpawn)
            {
                fmToSpawn.gameObject.SetActive(true);
            }
        }

        if (talkingFM != null)
        {
            if (currentDialogue.nextInteractionType != InteractionType.Aucun)
            {
                talkingFM.GetComponentInChildren<Interaction>().interactionType = currentDialogue.nextInteractionType;
                talkingFM.GetComponentInChildren<Interaction>().AnimationManagerUpdateIcon();
            }
        }

        talkingFM = null;
        currentDialogue = null;
    }
}
