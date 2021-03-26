using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectif : MonoBehaviour
{
    public InteractionType objectifInteractionType;

    public List<FideleManager> interactionTarget;

    public GameCamps targetCamp;

    public int interactionAmountToDo;

    private int currentInteractionAmountToDo;

    public bool objectifComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        QuestEvents.Instance.onEntityRecruited += OnEntityRecruited;
        QuestEvents.Instance.onThisEntityKilled += OnThisEntityKilled;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEntityRecruited()
    {
        if (objectifInteractionType == InteractionType.Recrutement)
        {
            //Debug.Log("Une entité vient d'être recrutée ! Saporisti !");
        }
    }

    private void OnThisEntityKilled(FideleManager thisFM)
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

    private void TestIfObjectivesAreCompleted()
    {
        GetComponentInParent<Quest>().CheckCurrentQuestState();
    }
}