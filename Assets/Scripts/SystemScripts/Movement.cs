﻿using System.Collections;
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
    private GameObject myMoveZone;
    private Collider2D myInteractionZoneCollider;
    private AnimationManager myAnimationManager;

    private DragCamera2D myCam;

    // Start is called before the first frame update
    void Start()
    {
        myCam = GameObject.Find("MovingCamera_CM").GetComponent<DragCamera2D>();
        myCam.followTarget = null;

        myAnimationManager = GetComponentInParent<AnimationManager>();

        myMoveZone = myAnimationManager.GetComponentInChildren<MovementZoneDetection>().gameObject;
        myInteractionZoneCollider = myAnimationManager.GetComponentInChildren<MovementZoneDetection>().GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), myAnimationManager.GetComponent<Collider2D>());

        if (isMoving && Input.GetMouseButton(0))
        {
            Cursor.visible = false;
            // ICI jouer VFX de déplacement en cours
            // ICI jouer SFX de déplacement en cours
            // ICI jouer Anim de déplacement en cours

            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = Vector2.Lerp(transform.position, mousePosition, moveSpeed);

            //myCam.followTarget = transform.parent.gameObject;
        }
        else if (isMoving && Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true;
            // ICI jouer VFX de déplacement terminé
            // ICI jouer SFX de déplacement terminé
            // ICI jouer Anim de déplacement terminé

            if (isLanbable)
            {
                transform.parent.position = transform.position;
                transform.localPosition = Vector3.zero;
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }

            myAnimationManager.HideMovement();

            //myCam.followTarget = null;

            hasMoved = true;
            isMoving = false;
        }
    }

    public void MovingCharacter()
    {
        // ICI jouer VFX de début de déplacement
        // ICI jouer SFX de début de déplacement
        // ICI jouer Anim de déplacement
        isMoving = true;

        // ICI utiliser Coroutine pour attendre la fin des effets pour déplacer
        myAnimationManager.DisplayMovement();
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
