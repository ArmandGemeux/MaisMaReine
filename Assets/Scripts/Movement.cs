using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 mousePosition;
    public float moveSpeed;

    private bool isMoving = false;
    private bool hasMoved = false;

    public GameObject mySprite;
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
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mySprite.transform.position = Vector2.Lerp(mySprite.transform.position, mousePosition, moveSpeed);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMoving = false;
            hasMoved = true;
        }
    }

    public void MovingCharacter()
    {
        Debug.Log("IL BOUUUUUUUUUUGE");
        isMoving = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isMoving && collision.gameObject == myMoveZone)
        {
            Debug.Log("Sorti");
        }
    }
}
