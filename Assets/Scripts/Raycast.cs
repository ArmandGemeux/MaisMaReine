using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interaction;

public class Raycast : MonoBehaviour
{
    //private Interaction myClickedInteraction;

    private FideleManager interactionLauncher;
    private Interaction interactionReceiver;

    public float interactionClickingTime;
    private float currentInteractionClickingTime;

    public float movemementClickingTime;
    private float currentMovementClickingTime;

    public bool isLookingForInteraction = false;
    
    private Movement myCurrentMovement;
    private Interaction myCurrentInteraction;
    //private FideleManager myDisplayedCharacter;

    // Start is called before the first frame update
    void Start()
    {
        currentInteractionClickingTime = interactionClickingTime;
        currentMovementClickingTime = movemementClickingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            LookForMovement();
        }

        /*if (Input.GetMouseButtonDown(1))
        {
            LookForDisplaying();
        }*/

        if (Input.GetMouseButton(0) && isLookingForInteraction == false)
        {
            LookForInteractionLauncher();
        }

        if (Input.GetMouseButton(0) && isLookingForInteraction == true)
        {
            LaunchInteraction();
        }

        if (isLookingForInteraction)
        {
            InteractionFeedback();
        }

        if (Input.GetMouseButtonUp(0))
        {
            currentMovementClickingTime = movemementClickingTime;
            currentInteractionClickingTime = interactionClickingTime;
        }
    }

    void LookForMovement()
    {
        Debug.Log("Test movement");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider != null && hit.collider.gameObject.GetComponentInChildren<Movement>() && hit.collider.gameObject.GetComponent<FideleManager>().currentCamp == 0)
        {
            Debug.Log("Touch");
            myCurrentMovement = hit.collider.gameObject.GetComponentInChildren<Movement>();
            currentMovementClickingTime -= Time.deltaTime;

            if (currentMovementClickingTime <= 0)
            {
                hit.collider.gameObject.GetComponent<FideleManager>().isSelectable = false;
                isLookingForInteraction = false;
                myCurrentMovement.MovingCharacter();
                if (myCurrentInteraction)
                {
                    myCurrentInteraction = null;
                }
                myCurrentMovement = null;
            }
        }
        else
        {
            currentMovementClickingTime = movemementClickingTime;
            Debug.Log("Rien");
        }
    }
    
    /*void LookForDisplaying()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider != null)    
        {
            if (hit.collider.gameObject.GetComponent<FideleManager>() && hit.collider.gameObject.GetComponent<FideleManager>().isDisplayed == false)
            {
                myDisplayedCharacter = hit.collider.gameObject.GetComponent<FideleManager>();
                myDisplayedCharacter.DisplayInformations();
                Debug.Log("Wow on montre des choses");
            }
        }        
    }*/

    void LookForInteractionLauncher()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider != null && hit.collider.gameObject.GetComponent<FideleManager>().isSelectable && isLookingForInteraction == false) //Si le raycast touche un fidèle qui est selectionnable...
        {
            interactionLauncher = hit.collider.gameObject.GetComponent<FideleManager>(); //Ce fidèle devient l'interactionLauncher

            foreach (Interaction myCollideInteraction in interactionLauncher.GetComponentInChildren<Interaction>().myCollideInteractionList)
            {
                myCollideInteraction.canInteract = true;
                myCollideInteraction.GetComponentInParent<FideleManager>().myInteractionZoneSprite.enabled = true;
                    
                //L'élément avec lequel il peut intéragir devient Interacif
            }

            isLookingForInteraction = true; //L'interactionLauncher cherche une interaction
        }
    }

    void LaunchInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider != null && hit.collider.gameObject.GetComponentInChildren<Interaction>().canInteract && interactionLauncher.isSelectable) //Si l'élément est Interactif et que l'interactionLaucher est sélectionnable...
        {
            Debug.Log("Lancement de l'interaction");
            interactionReceiver = hit.collider.gameObject.GetComponentInChildren<Interaction>();//L'élément Interactif devient l'interactionReceiver

            currentInteractionClickingTime -= Time.deltaTime; //Timer de clic actif

            if (interactionReceiver.canInteract == true && currentInteractionClickingTime <= 0 && !interactionLauncher.GetComponentInChildren<Interaction>().alreadyInteractedList.Contains(interactionReceiver)) //Si l'interactionReceiver est Interactif et que le timer de clic est fini...
            {
                currentInteractionClickingTime = interactionClickingTime; //Reset timer

                switch (interactionReceiver.interactionType) //Quel type d'interaction porte l'interactionReceiver ?
                {
                    case InteractionType.Dialogue:
                        Debug.Log("Dialogue");
                        break;
                    case InteractionType.Recrutement:
                        Debug.Log("Recrutement");
                        break;
                    case InteractionType.Combat:
                        Debug.Log("Combat");
                        break;
                    case InteractionType.Event:
                        Debug.Log("Event");
                        break;
                    default:
                        break;
                }
                interactionLauncher.isSelectable = false;
                interactionLauncher.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = null;
                interactionLauncher.GetComponentInChildren<Interaction>().hasInteract = true;

                foreach (Interaction myCollideInteraction in interactionLauncher.GetComponentInChildren<Interaction>().myCollideInteractionList)
                {
                    myCollideInteraction.canInteract = false; //L'élément avec lequel il peut intéragir devient non interacif
                    myCollideInteraction.mySpriteSlot.sprite = null;
                }

                interactionLauncher.GetComponentInChildren<Interaction>().alreadyInteractedList.Add(interactionReceiver);

                isLookingForInteraction = false; //L'interactionLauncher ne cherche plus d'interaction
            }
        }
    } 
    
    void InteractionFeedback()
    {
        //Check si la souris survole une interaction possible -> Afficher le feedback correspondant
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);


        if (hit.collider != null)
        {
            if (hit.transform.GetComponentInChildren<Interaction>() != null && hit.transform.GetComponentInChildren<Interaction>().canInteract && hit.transform.GetComponentInChildren<Interaction>() != interactionLauncher
                && hit.transform.GetComponent<FideleManager>().currentCamp != interactionLauncher.GetComponent<FideleManager>().currentCamp
                && !interactionLauncher.GetComponentInChildren<Interaction>().alreadyInteractedList.Contains(hit.transform.GetComponentInChildren<Interaction>()))
            {
                if (myCurrentInteraction == null)
                {
                    switch (hit.transform.GetComponentInChildren<Interaction>().interactionType)//Quel type d'interaction porte l'élément interactif ?
                    {
                        case InteractionType.Dialogue:
                            hit.transform.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = hit.transform.GetComponentInChildren<Interaction>().dialogueIcon;
                            interactionLauncher.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = hit.transform.GetComponentInChildren<Interaction>().dialogueIcon;
                            break;
                        case InteractionType.Recrutement:
                            hit.transform.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = hit.transform.GetComponentInChildren<Interaction>().recrutementIcon;
                            interactionLauncher.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = hit.transform.GetComponentInChildren<Interaction>().recrutementIcon;
                            break;
                        case InteractionType.Combat:
                            hit.transform.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = hit.transform.GetComponentInChildren<Interaction>().combatIcon;
                            interactionLauncher.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = hit.transform.GetComponentInChildren<Interaction>().combatIcon;
                            break;
                        case InteractionType.Event:
                            break;
                        default:
                            break;
                    }
                    myCurrentInteraction = hit.transform.GetComponentInChildren<Interaction>();
                }
                else if (myCurrentInteraction != null && myCurrentInteraction != hit.transform.GetComponentInChildren<Interaction>())
                {
                    Debug.Log("changement d'interaction");

                    myCurrentInteraction.mySpriteSlot.sprite = null;
                    interactionLauncher.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = null;

                    myCurrentInteraction = null;
                }
            }
        }
        else
        {
            Debug.Log("Rien en vue");

            if (myCurrentInteraction)
            {
                myCurrentInteraction.mySpriteSlot.sprite = null;
                interactionLauncher.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = null;

                myCurrentInteraction = null;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("On annule");

            if (myCurrentInteraction)
            {
                myCurrentInteraction.mySpriteSlot.sprite = null;
                interactionLauncher.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = null;
                myCurrentInteraction = null;
            }

            foreach (Interaction myCollideInteraction in interactionLauncher.GetComponentInChildren<Interaction>().myCollideInteractionList)
            {
                interactionLauncher.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = null;
                myCollideInteraction.mySpriteSlot.sprite = null;
                
                myCollideInteraction.canInteract = false;
            }

            currentInteractionClickingTime = interactionClickingTime;
            isLookingForInteraction = false;
        }
    }
}