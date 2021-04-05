using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectif : MonoBehaviour
{
    [Header ("Objectif")]

    public InteractionType objectifInteractionType;

    public List<FideleManager> interactionTarget;

    public GameCamps targetCamp;

    public int interactionAmountToDo;

    private int currentInteractionAmountToDo;

    public bool objectifComplete = false;

    private Quest myQuest;

    // Start is called before the first frame update
    void Start()
    {
        myQuest = GetComponentInParent<Quest>();

        QuestEvents.Instance.onEntityRecruited += OnEntityRecruited;
        QuestEvents.Instance.onThisEntityKilled += OnThisEntityKilled;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEntityRecruited(FideleManager thisFM)
    {
        if (myQuest.isActive)
        {
            if (objectifInteractionType == InteractionType.Recrutement && interactionTarget.Count >= 1)
            {
                if (interactionTarget != null && interactionTarget.Contains(thisFM))
                {
                    //Debug.Log("Cet ennemi est mort : " + thisFM.fidelePrenom);
                    interactionTarget.Remove(thisFM);
                    if (interactionTarget.Count == 0)
                    {
                        objectifComplete = true;
                        TestIfObjectivesAreCompleted();
                        Debug.Log("Objectif de recrutement précis atteint");
                    }
                }
            }
            else if (objectifInteractionType == InteractionType.Recrutement && interactionAmountToDo >= 1)
            {
                if (thisFM.myCamp == targetCamp)
                {
                    currentInteractionAmountToDo++;
                    if (currentInteractionAmountToDo == interactionAmountToDo)
                    {
                        objectifComplete = true;
                        TestIfObjectivesAreCompleted();
                        Debug.Log("Objectif de nombre de recrutement atteint");
                    }
                }
            }
        }
    }

    private void OnThisEntityKilled(FideleManager thisFM)
    {
        if (myQuest.isActive)
        {
            if (objectifInteractionType == InteractionType.Combat && interactionTarget.Count >= 1)
            {
                if (interactionTarget != null && interactionTarget.Contains(thisFM))
                {
                    //Debug.Log("Cet ennemi est mort : " + thisFM.fidelePrenom);
                    interactionTarget.Remove(thisFM);
                    if (interactionTarget.Count == 0)
                    {
                        objectifComplete = true;
                        TestIfObjectivesAreCompleted();
                        Debug.Log("Objectif de kill précis atteint");
                    }
                }
            }
            else if (objectifInteractionType == InteractionType.Combat && interactionAmountToDo >= 1)
            {
                if (thisFM.myCamp == targetCamp)
                {
                    currentInteractionAmountToDo++;
                    if (currentInteractionAmountToDo == interactionAmountToDo)
                    {
                        objectifComplete = true;
                        TestIfObjectivesAreCompleted();
                        Debug.Log("Objectif de nombre de kill atteint");
                    }
                }
            }
        }
    }

    private void TestIfObjectivesAreCompleted()
    {
        GetComponentInParent<Quest>().CheckCurrentQuestState();
    }
}