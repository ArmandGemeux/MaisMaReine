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
    public List <FideleManager> myCollideFideleManagerList = new List<FideleManager>();

    public List<Interaction> alreadyInteractedList = new List<Interaction>();
    
    public GameManager myGM;
    public Raycast myRC;
    private FideleManager fideleManager;

    public Sprite combatIcon;
    public Sprite dialogueIcon;
    public Sprite recrutementIcon;

    public SpriteRenderer myInteractionIcon;

    private void Awake()
    {
        fideleManager = GetComponentInParent<FideleManager>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (myCollideFideleManagerList.Count >= 1)
        {
            foreach (FideleManager myCollideFideleManager in myCollideFideleManagerList)
            {
                if (myCollideFideleManager.currentCamp.ToString() != fideleManager.currentCamp.ToString() && fideleManager.currentCamp.ToString() == myGM.currentCampTurn.ToString())
                {
                    myCollideFideleManager.haveAnInteraction = true;
                    myCollideFideleManager.DisplayInteraction();

                    if (myCollideFideleManager != fideleManager && !alreadyInteractedList.Contains(myCollideFideleManager.GetComponentInChildren<Interaction>()))
                    {
                        fideleManager.isSelectable = true;
                        Debug.Log("Coucou");
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
            myCollideFideleManagerList.Add(collision.GetComponentInParent<FideleManager>());
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

                    fideleManager.isSelectable = false;
                    myCollideInteraction.canInteract = false;
                }
            }


            if (myCollideFideleManagerList.Count >= 1)
            {
                foreach (FideleManager myCollideFideleManager in myCollideFideleManagerList)
                {
                    myCollideFideleManager.haveAnInteraction = false;

                    if (myCollideFideleManager != fideleManager && myCollideFideleManager.currentCamp.ToString() != fideleManager.currentCamp.ToString() && fideleManager.currentCamp.ToString() == myGM.currentCampTurn.ToString())
                    {
                        myCollideFideleManager.HideInteraction();
                    }
                }
            }

            myCollideFideleManagerList.Remove(collision.GetComponentInParent<FideleManager>());
            myCollideInteractionList.Remove(collision.GetComponent<Interaction>());
        }
    }
}
