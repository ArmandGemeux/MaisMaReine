using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{
    public Interaction interactionLauncherInteraction;
    public AnimationManager interactionLauncherAnim;

    public Interaction interactionReceiverInteraction;
    public AnimationManager interactionReceiverAnim;

    #region Singleton

    public static RaycastInteraction Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentCampTurn == GameCamps.Fidele && Input.GetMouseButtonDown(1))
            LookForInteractionLauncher();

        if (GameManager.Instance.currentCampTurn == GameCamps.Fidele && Input.GetMouseButtonDown(1) && interactionLauncherAnim != null)
            LookForInteractionReceiver();
    }

    public void LookForInteractionLauncher()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (interactionLauncherAnim == null && interactionLauncherInteraction == null)
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<AnimationManager>() && hit.collider.gameObject.GetComponent<AnimationManager>().isSelectable && hit.collider.gameObject.GetComponent<FideleManager>().myCamp == GameCamps.Fidele)
            {
                interactionLauncherAnim = hit.collider.GetComponent<AnimationManager>();
                interactionLauncherInteraction = hit.collider.GetComponentInChildren<Interaction>();

                interactionLauncherAnim.ToggleLauncherOutline();

                interactionLauncherAnim.isSelected = true;
                
                foreach (Interaction myCollideInteraction in interactionLauncherInteraction.myCollideInteractionList)
                {
                    if (!interactionLauncherInteraction.alreadyInteractedList.Contains(myCollideInteraction))
                    {
                        myCollideInteraction.canInteract = true;
                        myCollideInteraction.GetComponentInParent<AnimationManager>().ActivateReceiverSelection();
                    }
                }
            }
        }
        else
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<AnimationManager>() && hit.collider.gameObject.GetComponent<AnimationManager>().isSelectable && hit.collider.gameObject.GetComponent<FideleManager>().myCamp == GameCamps.Fidele)
            {
                //Debug.Log("On change de selectionneur");

                ResetReceiverInteraction();
                ResetLauncherInteraction();

                interactionLauncherAnim = hit.collider.GetComponent<AnimationManager>();
                interactionLauncherInteraction = hit.collider.GetComponentInChildren<Interaction>();

                interactionLauncherAnim.ToggleLauncherOutline();

                interactionLauncherAnim.isSelected = true;

                foreach (Interaction myCollideInteraction in interactionLauncherInteraction.myCollideInteractionList)
                {
                    myCollideInteraction.canInteract = true;
                    myCollideInteraction.GetComponentInParent<AnimationManager>().ActivateReceiverSelection();
                }
            }
        }
    }

    public void LookForInteractionReceiver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (hit.collider != null && hit.collider.gameObject.GetComponentInChildren<Interaction>() && hit.collider.gameObject.GetComponentInChildren<Interaction>().canInteract 
            && hit.collider.gameObject.GetComponent<FideleManager>().myCamp != GameCamps.Fidele && interactionLauncherInteraction.myCollideInteractionList.Contains(hit.collider.gameObject.GetComponentInChildren<Interaction>()) 
            && !interactionLauncherInteraction.alreadyInteractedList.Contains(hit.collider.gameObject.GetComponentInChildren<Interaction>()))
        {
            interactionReceiverAnim = hit.collider.GetComponent<AnimationManager>();
            interactionReceiverInteraction = hit.collider.GetComponentInChildren<Interaction>();

            switch (interactionReceiverInteraction.interactionType) //Quel type d'interaction porte l'interactionReceiver ?
            {
                case InteractionType.Dialogue:
                    interactionReceiverInteraction.GetComponent<Dialogue>().DisplayDialogueFeedback();
                    //Debug.Log("Dialogue");
                    break;
                case InteractionType.Recrutement:
                    //Debug.Log("Recrutement");
                    break;
                case InteractionType.Combat:
                    interactionReceiverInteraction.GetComponent<Combat>().StartFight();
                    //Debug.Log("Combat");
                    break;
                case InteractionType.Event:
                    //Debug.Log("Event");
                    break;
                default:
                    break;
            }

            interactionLauncherInteraction.alreadyInteractedList.Add(interactionReceiverInteraction);

            //Debug.Log("Interaction");
            ResetReceiverInteraction();
            ResetLauncherInteraction();
        }
    }

    public void ResetLauncherInteraction()
    {
        if (interactionLauncherAnim != null)
        {
            interactionLauncherAnim.ToggleLauncherOutline();

            interactionLauncherAnim.isSelected = false;

            interactionLauncherAnim = null;
            interactionLauncherInteraction = null;

        }
    }

    public void ResetReceiverInteraction()
    {
        if (interactionReceiverAnim != null)
        {
            interactionReceiverInteraction = null;
            interactionReceiverAnim = null;
        }
    }
}