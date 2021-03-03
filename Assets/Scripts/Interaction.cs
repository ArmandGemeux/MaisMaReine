using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public enum InteractionType { Dialogue, Recrutement, Combat, Event }
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
            case InteractionType.Event:
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
                if (myCollideAnimationManager.GetComponent<FideleManager>().currentCamp != Camp.Fidele && myFideleManager.currentCamp == Camp.Fidele)
                {
                    myCollideAnimationManager.haveAnInteraction = true;
                    myCollideAnimationManager.DisplayInteraction();

                    if (myCollideAnimationManager != myAnimationManager && !alreadyInteractedList.Contains(myCollideAnimationManager.GetComponentInChildren<Interaction>()))
                    {
                        myAnimationManager.isSelectable = true;
                        myAnimationManager.ActivateLauncherSelection();
                        myCollideAnimationManager.ActivateReceiverSelection();
                    }
                }               
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Interaction>() != null)
        {
            myCollideInteractionList.Add(collision.GetComponent<Interaction>());
            myCollideAnimationManagerList.Add(collision.GetComponentInParent<AnimationManager>());
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Interaction>() != null)
        {
            if (myCollideInteractionList.Count >= 1)
            {
                foreach (Interaction myCollideInteraction in myCollideInteractionList)
                {
                    myInteractionIcon.sprite = null;
                    myCollideInteraction.myInteractionIcon.sprite = null;

                    myAnimationManager.isSelectable = false;
                    myCollideInteraction.canInteract = false;
                }
            }


            if (myCollideAnimationManagerList.Count >= 1)
            {
                foreach (AnimationManager myCollideAnimationManager in myCollideAnimationManagerList)
                {
                    myCollideAnimationManager.haveAnInteraction = false;

                    if (myCollideAnimationManager != myAnimationManager && myCollideAnimationManager.GetComponent<FideleManager>().currentCamp.ToString() != myFideleManager.currentCamp.ToString() && myFideleManager.currentCamp.ToString() == GameManager.Instance.currentCampTurn.ToString())
                    {
                        myCollideAnimationManager.HideInteraction();
                        myAnimationManager.DesactivateLauncherSelection();
                        myCollideAnimationManager.DesactivateReceiverSelection();
                    }
                }
            }

            myCollideAnimationManagerList.Remove(collision.GetComponentInParent<AnimationManager>());
            myCollideInteractionList.Remove(collision.GetComponent<Interaction>());
        }
    }
}
