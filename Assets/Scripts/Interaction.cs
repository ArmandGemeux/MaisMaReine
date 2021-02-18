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


    //public Interaction myCollideInteraction;
    //private FideleManager myCollideFideleManager;

    public GameManager myGM;
    public Raycast myRC;
    private FideleManager fideleManager;

    public Sprite combatIcon;
    public Sprite dialogueIcon;
    public Sprite recrutementIcon;

    public SpriteRenderer mySpriteSlot;

    private bool isColliding = false;

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
            foreach (FideleManager myCollideCharacter in myCollideFideleManagerList)
            {
                if (myCollideCharacter.currentCamp.ToString() != fideleManager.currentCamp.ToString())
                {
                    myCollideCharacter.DisplayInteraction();
                }
            }

            foreach (FideleManager myCollideFideleManager in myCollideFideleManagerList)
            {
                if (myCollideFideleManager != fideleManager && myCollideFideleManager.currentCamp.ToString() != fideleManager.currentCamp.ToString() && fideleManager.currentCamp.ToString() == myGM.currentCampTurn.ToString())
                {
                    fideleManager.isSelectable = true;
                    Debug.Log("Coucou");
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

            if (myCollideInteractionList.Count >= 1)//Si cet objet rencontre un objet avec un élément interactif...
            {
                Debug.Log("Nouvelle interaction ajoutée");

                foreach (FideleManager myCollideFideleManager in myCollideFideleManagerList)
                {
                    Debug.Log("Blou");
                    if (myCollideFideleManager != fideleManager && myCollideFideleManager.currentCamp.ToString() != fideleManager.currentCamp.ToString() && fideleManager.currentCamp.ToString() == myGM.currentCampTurn.ToString())
                    {
                        //Debug.Log("GOOOOOD !!");
                        //fideleManager.isSelectable = true;
                        //isColliding = true;

                        //Ici : Feedback d'interaction disponible
                    }
                }
            }
        }
    }

    /*public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Interaction>() != null && myCollideInteractionList.Count >= 1)
        {
            foreach (FideleManager myCollideFideleManager in myCollideFideleManagerList)
            {
                if (myCollideFideleManager != fideleManager && myCollideFideleManager.currentCamp.ToString() != fideleManager.currentCamp.ToString() && fideleManager.currentCamp.ToString() == myGM.currentCampTurn.ToString())
                {
                    fideleManager.isSelectable = true;
                    Debug.Log("Coucou");
                }
            }
        }
    }*/

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Interaction>() != null)
        {
            //myCollideInteraction = collision.GetComponent<Interaction>();
            if (myCollideInteractionList.Count >= 1)
            {
                foreach (Interaction myCollideInteraction in myCollideInteractionList)
                {
                    mySpriteSlot.sprite = null;
                    myCollideInteraction.mySpriteSlot.sprite = null;

                    fideleManager.isSelectable = false;
                    myCollideInteraction.canInteract = false;
                }
            }

            if (myCollideFideleManagerList.Count >= 1)
            {
                foreach (FideleManager myCollideFideleManager in myCollideFideleManagerList)
                {
                    myCollideFideleManager.HideInteraction();
                }
            }

            //isColliding = false;
            myCollideFideleManagerList.Remove(collision.GetComponentInParent<FideleManager>());
            myCollideInteractionList.Remove(collision.GetComponent<Interaction>());
        }
    }
}
