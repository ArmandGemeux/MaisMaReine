using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 mousePosition;
    private float moveSpeed = 2;

    private Vector2 startPosition;

    public bool isMoving = false;
    public bool hasMoved = false;

    private bool isLanbable;

    //public GameObject myCollideZone;
    private FideleManager myFM;
    private GameObject myMoveZone;
    private Collider2D myInteractionZoneCollider;
    private AnimationManager myAnimationManager;
    private Interaction myInteraction;

    // Start is called before the first frame update
    void Start()
    {
        myFM = GetComponentInParent<FideleManager>();
        myInteraction = myFM.GetComponentInChildren<Interaction>();

        DragCamera2D.Instance.followTarget = null;

        myAnimationManager = GetComponentInParent<AnimationManager>();

        myMoveZone = myAnimationManager.GetComponentInChildren<MovementZoneDetection>().gameObject;
        myInteractionZoneCollider = myMoveZone.GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), myAnimationManager.GetComponent<Collider2D>());

        if (isMoving && Input.GetMouseButton(0) && hasMoved == false)
        {
            CursorManager.Instance.SetCursorToMovement();
            // ICI jouer VFX de déplacement en cours
            // ICI jouer SFX de déplacement en cours
            // ICI jouer Anim de déplacement en cours

            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);

            foreach (FideleManager fmir in myFM.unitsInRange)
            {
                if (fmir.myCamp != myFM.myCamp)
                {
                    fmir.GetComponent<AnimationManager>().DisplayInteraction();
                    fmir.GetComponent<AnimationManager>().DisplayInteractionIcon();
                    fmir.GetComponentInChildren<Interaction>().myInteractionIcon.color = Color.gray;
                }
            }

            //myCam.followTarget = transform.parent.gameObject;
        }
        else if (isMoving && Input.GetMouseButtonUp(0))
        {
            CursorManager.Instance.SetCursorToDefault();
            // ICI jouer VFX de déplacement terminé
            // ICI jouer SFX de déplacement terminé
            // ICI jouer Anim de déplacement terminé

            if (isLanbable)
            {
                myAnimationManager.CheckActionsLeftAmout();
                transform.parent.position = transform.position;
                transform.localPosition = Vector3.zero;
                hasMoved = true;

                for (int i = 0; i < myInteraction.myCollideInteractionList.Count; i++)
                {
                    QuestManager.Instance.OnUnitReached(myInteraction.myCollideInteractionList[i].GetComponent<Interaction>());
                }

            }
            else
            {
                transform.localPosition = Vector3.zero;
            }

            myAnimationManager.HideMovement();
            myAnimationManager.HideInteraction();

            foreach (FideleManager fmir in GameManager.Instance.allMapUnits)
            {
                if (fmir.myCamp != myFM.myCamp)
                {
                    fmir.GetComponent<AnimationManager>().HideInteraction();
                    fmir.GetComponent<AnimationManager>().HideInteractionIcon();
                    fmir.GetComponentInChildren<Interaction>().myInteractionIcon.color = Color.white;

                    fmir.GetComponentInChildren<Interaction>().OtherCampDisplayInteractionFeedbacks();
                }
            }

            myInteraction.FideleDisplayInteractionFeedbacks();

            myInteraction.CheckForAvaibleInteractions();

            isMoving = false;
        }
    }

    public void MovingCharacter()
    {
        if (hasMoved == false)
        {
            // ICI jouer VFX de début de déplacement
            // ICI jouer SFX de début de déplacement
            // ICI jouer Anim de déplacement
            isMoving = true;

            // ICI utiliser Coroutine pour attendre la fin des effets pour déplacer

            myAnimationManager.DisplayInteraction();
            myAnimationManager.DisplayMovement();
        }
        else
        {
            myInteraction.FideleDisplayInteractionFeedbacks();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Obstacle"))
        {
            isLanbable = false;
            //Debug.Log(collision.name);
            myAnimationManager.UnableToLand();
        }

        /*if (collision.gameObject == myMoveZone)
        {
            isLanbable = true;
            myAnimationManager.AbleToLand();
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == myMoveZone)
        {
            isLanbable = false;
            //Debug.Log(collision.name);
            myAnimationManager.UnableToLand();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == myMoveZone)
        {
            isLanbable = true;
            myAnimationManager.AbleToLand();
        }
        else if (collision.tag == ("Obstacle"))
        {
            isLanbable = false;
            myAnimationManager.UnableToLand();
        }
    }
}
