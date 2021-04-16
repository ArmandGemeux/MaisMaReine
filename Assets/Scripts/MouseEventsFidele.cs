using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class MouseEventsFidele : MonoBehaviour
{
    private FideleManager myFideleManager;
    private AnimationManager myAnimManager;

    private Interaction myInteraction;

    #region Movement

    private Movement myMovement;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        myFideleManager = GetComponent<FideleManager>();
        myAnimManager = GetComponent<AnimationManager>();

        myInteraction = GetComponentInChildren<Interaction>();

        myMovement = GetComponentInChildren<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(myInteraction.canInteract + gameObject.name);
        
    }

    public void OnMouseEnter()
    {
        if (myMovement.isMoving == false)
        {
            myAnimManager.DisplayInteraction();
        }
    }

    public void OnMouseDown()
    {
        RaycastInteraction.Instance.ResetLauncherInteraction();
    }

    public void OnMouseDrag()
    {
        #region Movement

        if (myFideleManager.myCamp == GameCamps.Fidele)
        {
            myAnimManager.isSelectable = false;
            RaycastInteraction.Instance.ResetLauncherInteraction();
            RaycastInteraction.Instance.ResetReceiverInteraction();

            myMovement.MovingCharacter();
        }

        #endregion
    }

    public void OnMouseUp()
    {

    }

    public void OnMouseOver()
    {

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