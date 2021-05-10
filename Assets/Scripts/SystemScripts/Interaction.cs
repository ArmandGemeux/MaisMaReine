using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType {Aucun ,Dialogue, Recrutement, Combat}

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
    public FideleManager myFideleManager;

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Interaction>() != null && collision.GetComponentInParent<FideleManager>().myCamp != myFideleManager.myCamp)
        {
            myCollideInteractionList.Add(collision.GetComponent<Interaction>());
            myCollideAnimationManagerList.Add(collision.GetComponentInParent<AnimationManager>());

            DisplayInteractionFeedbacks();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Interaction>() != null)
        {
            Interaction myExitingCollision = collision.GetComponent<Interaction>();

            RemoveCollindingCharacterFromInteractionList(myExitingCollision);

            AnimationManager myExitingAM = collision.GetComponentInParent<AnimationManager>();

            RemoveCollidingCharacterFromAMList(myExitingAM);

            DisplayInteractionFeedbacks();
        }
    }

    public void DisplayInteractionFeedbacks()
    {
        if (myCollideAnimationManagerList.Count >= 1)
        {
            foreach (AnimationManager myCollideAnimationManager in myCollideAnimationManagerList)
            {
                if (myCollideAnimationManager.GetComponent<FideleManager>().myCamp != GameCamps.Fidele && myFideleManager.myCamp == GameCamps.Fidele)
                {
                    myCollideAnimationManager.haveAnInteraction = true;
                    //myCollideAnimationManager.DisplayInteraction();
                    //myAnimationManager.DisplayInteraction();

                    if (!alreadyInteractedList.Contains(myCollideAnimationManager.GetComponentInChildren<Interaction>()))
                    {
                        myAnimationManager.isSelectable = true;
                        myAnimationManager.ActivateLauncherSelection();

                        myCollideAnimationManager.ActivateReceiverSelection();
                        myCollideAnimationManager.DisplayInteractionIcon();
                    }
                }
            }
        }
    }

    public void RemoveCollidingCharacterFromAMList(AnimationManager aMToRemove)
    {
        if (myCollideAnimationManagerList.Count >= 1)
        {
            aMToRemove.haveAnInteraction = false;
            aMToRemove.HideInteractionIcon();

            if (aMToRemove != myAnimationManager && aMToRemove.GetComponent<FideleManager>().myCamp != myFideleManager.myCamp && myFideleManager.myCamp == GameManager.Instance.currentCampTurn)
            {
                aMToRemove.HideInteraction();
                aMToRemove.DesactivateReceiverSelection();

                myAnimationManager.HideInteraction();
                myAnimationManager.DesactivateLauncherSelection();
            }
        }

        myCollideAnimationManagerList.Remove(aMToRemove);
        DisplayInteractionFeedbacks();
    }

    public void RemoveCollindingCharacterFromInteractionList(Interaction interactionToRemove)
    {
        if (myCollideInteractionList.Count >= 1)
        {
            interactionToRemove.canInteract = false;
            interactionToRemove.myInteractionIcon.sprite = null;

            myAnimationManager.isSelectable = false;
            myInteractionIcon.sprite = null;
        }

        myCollideInteractionList.Remove(interactionToRemove);
        DisplayInteractionFeedbacks();
    }
}
