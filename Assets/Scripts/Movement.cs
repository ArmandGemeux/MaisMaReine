using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 mousePosition;
    public float moveSpeed;

    private Vector2 startPosition;

    [SerializeField]
    public bool isMoving = false;
    private bool hasMoved = false;

    private bool isLanbable;

    //public GameObject myCollideZone;
    public GameObject myMoveZone;
    public Collider2D myInteractionZoneCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving && Input.GetMouseButton(0))
        {
            Debug.Log("IsLandable : " + isLanbable);
            //myInteractionZoneCollider.enabled = false;

            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
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

            GetComponentInParent<FideleManager>().HideMovement();

            hasMoved = true;
            isMoving = false;

            //myInteractionZoneCollider.enabled = true;
        }
    }

    public void MovingCharacter()
    {
        isMoving = true;
        GetComponentInParent<FideleManager>().DisplayMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == ("Obstacle"))
        {
            isLanbable = false;
        }

        if (collision.gameObject == myMoveZone)
        {
            isLanbable = true;
        }

        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == myMoveZone)
        {
            isLanbable = false;
        }

        if (collision.tag == ("Obstacle"))
        {
            isLanbable = true;
        }       
    }
}
