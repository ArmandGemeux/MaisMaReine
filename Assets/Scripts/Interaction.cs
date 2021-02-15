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
    

    public List <Interaction> myCollideInteractionList;
    public List<FideleManager> myCollideCharacterList;

    public List<Interaction> alreadyInteractedList;


    public Interaction myCollideInteraction;
    private FideleManager myCollideCharacter;

    public GameManager myGM;
    public Raycast myRC;
    private FideleManager fideleManager;

    public Sprite combatIcon;
    public Sprite dialogueIcon;
    public Sprite recrutementIcon;

    public SpriteRenderer mySpriteSlot;

    private bool testBool = false;

    private void Awake()
    {
        fideleManager = GetComponentInParent<FideleManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        myCollideInteractionList = new List<Interaction>();
        myCollideCharacterList = new List<FideleManager>();

        alreadyInteractedList = new List<Interaction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (testBool)
        {
            fideleManager.isSelectable = true; //cet objet devient sélectionnable
        }        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        myCollideInteraction = collision.GetComponent<Interaction>();
        myCollideCharacter = collision.GetComponentInParent<FideleManager>();

        if (myCollideInteraction)//Si cet objet rencontre un objet avec un élément interactif...
        {
            myCollideInteractionList.Add(collision.GetComponent<Interaction>());
            myCollideCharacterList.Add(collision.GetComponentInParent<FideleManager>());

            foreach (Interaction myCollideInteraction in myCollideInteractionList)
            {
                if (fideleManager.currentCamp.ToString() == myGM.currentCampTurn.ToString())//et que c'est le tour de cet objet...
                {
                    if (myCollideCharacter.currentCamp.ToString() != fideleManager.currentCamp.ToString())//et que l'élément interactif n'est pas du même camp que cet objet, alors...
                    {
                        Debug.Log("Do Something");
                        Debug.Log("Tour :" + myGM.currentCampTurn);
                        Debug.Log(fideleManager.currentCamp + " interagit avec " + myCollideCharacter.currentCamp);
                    }
                }
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (myCollideInteraction && !alreadyInteractedList.Contains(myCollideInteraction))
        {
            if (myCollideCharacter.currentCamp.ToString() != fideleManager.currentCamp.ToString())
            {
                if (fideleManager.currentCamp.ToString() == myGM.currentCampTurn.ToString())
                {
                    Debug.Log("Encore !");
                    testBool = true;
                }
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //myCollideInteraction = collision.GetComponent<Interaction>();
        if (myCollideInteraction)
        {
            foreach (Interaction myCollideInteraction in myCollideInteractionList)
            {
                mySpriteSlot.sprite = null;
                myCollideInteraction.mySpriteSlot.sprite = null;

                fideleManager.isSelectable = false;
                myCollideInteraction.canInteract = false;
            }
        }
        testBool = false;
        myCollideInteractionList.Remove(collision.GetComponent<Interaction>());
        myCollideCharacterList.Remove(collision.GetComponentInParent<FideleManager>());
    }
}
