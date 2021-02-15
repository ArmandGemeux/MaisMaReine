using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 mousePosition;
    public float moveSpeed;

    private Vector2 startPosition;

    private bool isMoving = false;
    private bool hasMoved = false;

    private bool isLanbable;

    //public GameObject myCollideZone;
    public GameObject myMoveZone;

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
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);
        }
        else if (isMoving && Input.GetMouseButtonUp(0))
        {
            isMoving = false;
            hasMoved = true;

            if (isLanbable)
            {
                transform.parent.position = transform.position;
                transform.localPosition = Vector3.zero;
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }
        }
    }

    public void MovingCharacter()
    {
        isMoving = true;
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
