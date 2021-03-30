﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType { Dialogue, Recrutement, Combat}

public class Interaction : MonoBehaviour
{
    public InteractionType interactionType;

    [Space]
    [Header("Dev only")]
    public float interactionTimer;
    public bool canInteract = false;
    public bool hasInteract = false;
    

    public List <Interaction> myCollideInteractionList = new List<Interaction>();
    public List <AnimationManager> myCollideAnimationManagerList = new List<AnimationManager>();

    public List<Interaction> alreadyInteractedList = new List<Interaction>();

    private AnimationManager myAnimationManager;
    private FideleManager myFideleManager;

    public Sprite combatIcon;
    public Sprite dialogueIcon;
    public Sprite recrutementIcon;

    public SpriteRenderer myInteractionIcon;

    private void Awake()
    {
        myAnimationManager = GetComponentInParent<AnimationManager>();
        myFideleManager = GetComponentInParent<FideleManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        switch (interactionType)
        {
            case InteractionType.Dialogue:
                gameObject.AddComponent<Dialogue>();
                break;
            case InteractionType.Recrutement:
                gameObject.AddComponent<Recrutement>();
                break;
            case InteractionType.Combat:
                gameObject.AddComponent<Combat>();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (myCollideAnimationManagerList.Count >= 1)
        {
            foreach (AnimationManager myCollideAnimationManager in myCollideAnimationManagerList)
            {
                if (myCollideAnimationManager.GetComponent<FideleManager>().myCamp != GameCamps.Fidele && myFideleManager.myCamp == GameCamps.Fidele)
                {
                    myCollideAnimationManager.haveAnInteraction = true;
                    myCollideAnimationManager.DisplayInteraction();
                }

                if (!alreadyInteractedList.Contains(myCollideAnimationManager.GetComponentInChildren<Interaction>()) && myFideleManager.myCamp == GameCamps.Fidele)
                {
                    myAnimationManager.isSelectable = true;
                    myAnimationManager.ActivateLauncherSelection();
                }
                else if (!alreadyInteractedList.Contains(myCollideAnimationManager.GetComponentInChildren<Interaction>()) && myCollideAnimationManager.GetComponent<FideleManager>().myCamp != GameCamps.Fidele)
                {
                    myCollideAnimationManager.ActivateReceiverSelection();
                }
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Interaction>() != null && collision.GetComponentInParent<FideleManager>().myCamp != myFideleManager.myCamp)
        {
            myCollideInteractionList.Add(collision.GetComponent<Interaction>());
            myCollideAnimationManagerList.Add(collision.GetComponentInParent<AnimationManager>());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Interaction>() != null)
        {
            Interaction myExitingCollision = collision.GetComponent<Interaction>();

            if (myCollideInteractionList.Count >= 1)
            {
                myExitingCollision.canInteract = false;
                myExitingCollision.myInteractionIcon.sprite = null;

                myAnimationManager.isSelectable = false;
                myInteractionIcon.sprite = null;
            }

            AnimationManager myExitingAM = collision.GetComponentInParent<AnimationManager>();

            if (myCollideAnimationManagerList.Count >= 1)
            {
                myExitingAM.haveAnInteraction = false;

                if (myExitingAM != myAnimationManager && myExitingAM.GetComponent<FideleManager>().myCamp != myFideleManager.myCamp && myFideleManager.myCamp == GameManager.Instance.currentCampTurn)
                {
                    myExitingAM.HideInteraction();
                    myExitingAM.DesactivateReceiverSelection();

                    myAnimationManager.DesactivateLauncherSelection();
                }
            }

            myCollideAnimationManagerList.Remove(myExitingAM);
            myCollideInteractionList.Remove(myExitingCollision);
        }
    }
}
