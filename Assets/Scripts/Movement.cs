using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 mousePosition;
    public float moveSpeed;

    private Vector2 startPosition;

    public bool isMoving = false;
    private bool hasMoved = false;

    private bool isLanbable;

    //public GameObject myCollideZone;
    public GameObject myMoveZone;
    public Collider2D myInteractionZoneCollider;
    private FideleManager myFideleManager;

    private DragCamera2D myCam;

    // Start is called before the first frame update
    void Start()
    {
        myCam = GameObject.Find("MovingCamera_CM").GetComponent<DragCamera2D>();
        myCam.followTarget = null;

        myFideleManager = GetComponentInParent<FideleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), myFideleManager.GetComponent<Collider2D>());

        if (isMoving && Input.GetMouseButton(0))
        {
            //Debug.Log("IsLandable : " + isLanbable);

            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);

            myCam.followTarget = transform.parent.gameObject;
        }
        else if (isMoving && Input.GetMouseButtonUp(0))
        {
            if (isLanbable)
            {
                transform.parent.position = transform.position;
                transform.localPosition = Vector3.zero;
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }

            myFideleManager.HideMovement();

            myCam.followTarget = null;

            hasMoved = true;
            isMoving = false;
        }
    }

    public void MovingCharacter()
    {
        isMoving = true;
        myFideleManager.DisplayMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Obstacle") && collision != myFideleManager)
        {
            isLanbable = false;
            //Debug.Log(collision.name);
            myFideleManager.UnableToLand();
        }

        if (collision.gameObject == myMoveZone)
        {
            isLanbable = true;
            myFideleManager.AbleToLand();
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == myMoveZone)
        {
            isLanbable = false;
            //Debug.Log(collision.name);
            myFideleManager.UnableToLand();
        }

        if (collision.tag == ("Obstacle") && collision != myFideleManager)
        {
            isLanbable = true;
            myFideleManager.AbleToLand();
        }       
    }
}
