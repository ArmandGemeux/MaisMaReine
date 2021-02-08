using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interaction;

public class Raycast : MonoBehaviour
{
    //private Interaction myClickedInteraction;

    private FideleManager interactionLauncher;
    private Interaction interactionReceiver;

    public float clickingTime;
    private float currentClickingTime;
    
    public bool isLookingForInteraction = false;
    
    private Interaction myCurrentInteraction;

    // Start is called before the first frame update
    void Start()
    {
        currentClickingTime = clickingTime;
    }

    // Update is called once per frame
    void Update()
    {     
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
            currentClickingTime = clickingTime;
        }
    }

    void LookForInteractionLauncher()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider != null && hit.collider.gameObject.GetComponentInParent<FideleManager>().isSelectable && isLookingForInteraction == false) //Si le raycast touche un fidèle qui est selectionnable...
        {
            interactionLauncher = hit.collider.gameObject.GetComponentInParent<FideleManager>(); //Ce fidèle devient l'interactionLauncher

            foreach (Interaction myCollideInteraction in interactionLauncher.GetComponentInChildren<Interaction>().myCollideInteractionList)
            {
                myCollideInteraction.canInteract = true; //L'élément avec lequel il peut intéragir devient Interacif
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

            currentClickingTime -= Time.deltaTime; //Timer de clic actif

            if (interactionReceiver.canInteract == true && currentClickingTime <= 0) //Si l'interactionReceiver est Interactif et que le timer de clic est fini...
            {
                currentClickingTime = clickingTime; //Reset timer

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
                    myCollideInteraction.canInteract = false; //L'élément avec lequel il peut intéragir devient Interacif
                    myCollideInteraction.mySpriteSlot.sprite = null;
                }

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
            Debug.Log(hit.transform.gameObject);

            if (hit.transform.GetComponent<Interaction>() != null && hit.transform.GetComponent<Interaction>().canInteract && hit.transform.GetComponent<Interaction>() != interactionLauncher
                && hit.transform.GetComponentInParent<FideleManager>().currentCamp != interactionLauncher.GetComponentInParent<FideleManager>().currentCamp)
            {
                if (myCurrentInteraction == null)
                {
                    switch (hit.transform.GetComponent<Interaction>().interactionType)//Quel type d'interaction porte l'élément interactif ?
                    {
                        case InteractionType.Dialogue:
                            hit.transform.GetComponent<Interaction>().mySpriteSlot.sprite = hit.transform.GetComponent<Interaction>().dialogueIcon;
                            interactionLauncher.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = hit.transform.GetComponent<Interaction>().dialogueIcon;
                            break;
                        case InteractionType.Recrutement:
                            hit.transform.GetComponent<Interaction>().mySpriteSlot.sprite = hit.transform.GetComponent<Interaction>().recrutementIcon;
                            interactionLauncher.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = hit.transform.GetComponent<Interaction>().recrutementIcon;
                            break;
                        case InteractionType.Combat:
                            hit.transform.GetComponent<Interaction>().mySpriteSlot.sprite = hit.transform.GetComponent<Interaction>().combatIcon;
                            interactionLauncher.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = hit.transform.GetComponent<Interaction>().combatIcon;
                            break;
                        case InteractionType.Event:
                            break;
                        default:
                            break;
                    }
                    myCurrentInteraction = hit.transform.GetComponent<Interaction>();
                }
                else if (myCurrentInteraction != null && myCurrentInteraction != hit.transform.GetComponent<Interaction>())
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
            }

            foreach (Interaction myCollideInteraction in interactionLauncher.GetComponentInChildren<Interaction>().myCollideInteractionList)
            {
                interactionLauncher.GetComponentInChildren<Interaction>().mySpriteSlot.sprite = null;
                myCollideInteraction.mySpriteSlot.sprite = null;
                
                myCollideInteraction.canInteract = false;
            }

            currentClickingTime = clickingTime;
            isLookingForInteraction = false;
        }
    }
}