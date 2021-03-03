using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MouseEventsFidele : MonoBehaviour
{
    private FideleManager myFideleManager;
    private AnimationManager myAnimManager;

    private Interaction myInteraction;

    public float interactionClickingTime;
    private float currentInteractionClickingTime;

    /*private GameManager myGM;
    private RaycastInteraction myRC;*/

    #region Movement

    private Movement myMovement;

    public float movementClickingTime;
    private float currentMovementClickingTime;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        myFideleManager = GetComponent<FideleManager>();
        myAnimManager = GetComponent<AnimationManager>();

        myInteraction = GetComponentInChildren<Interaction>();

        myMovement = GetComponentInChildren<Movement>();

        currentMovementClickingTime = movementClickingTime;

        currentInteractionClickingTime = interactionClickingTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(myInteraction.canInteract + gameObject.name);
        
    }

    public void OnMouseEnter()
    {
        myAnimManager.DisplayInteraction();
    }

    public void OnMouseDown()
    {
       
    }

    public void OnMouseDrag()
    {
        #region Movement

        if (myFideleManager.currentCamp == Camp.Fidele)
        {
            myAnimManager.isSelectable = false;

            myMovement.MovingCharacter();
        }

        #endregion
    }

    public void OnMouseUp()
    {

    }

    public void OnMouseOver()
    {
        #region Interaction

        #region ReceiveInteraction

        if (myInteraction.canInteract && !RaycastInteraction.Instance.interactionLauncherInteraction.alreadyInteractedList.Contains(myInteraction))
            if (Input.GetMouseButtonDown(1))
            {

                //RaycastInteraction.Instance.interactionLauncherInteraction != null
                Debug.Log("Interaction");
                RaycastInteraction.Instance.interactionLauncherInteraction.alreadyInteractedList.Add(myInteraction);
                myAnimManager.DesactivateReceiverSelection();
            }

        #endregion

        #endregion

        #region InformationDisplaying

        if (Input.GetMouseButtonDown(2) && myAnimManager.isInfoDisplayed == false)
        {
            myAnimManager.DisplayMovement();
            myAnimManager.DisplayStats();
            myAnimManager.isInfoDisplayed = true;
        }
        else if (Input.GetMouseButtonDown(2) && myAnimManager.isInfoDisplayed)
        {
            myAnimManager.HideMovement();
            myAnimManager.HideStats();
            myAnimManager.isInfoDisplayed = false;
        }

        #endregion
    }

    public void OnMouseExit()
    {
        #region InformationHiding

        if (myAnimManager.isInfoDisplayed && myMovement.isMoving == false)
        {
            myAnimManager.HideMovement();
            myAnimManager.HideStats();
            myAnimManager.isInfoDisplayed = false;
        }

        if (myMovement.isMoving == false && myInteraction.myCollideInteractionList.Count == 0)
        {
            myAnimManager.HideInteraction();
            myAnimManager.HideStats();
        }

        #endregion
    }
}
