using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FideleManager : MonoBehaviour
{
    [Header("Caractéristiques")]
    public string fideleNom;
    public string fidelePrenom;

    public int maxHp;
    private int currentHP;

    [Range(4, 10)]
    public int attackRange;
    [Range(1, 3)]
    public int counteAttackRange;

    [Range(0, 100)]
    public int criticChances;
    [Range(0, 100)]
    public int failChances;

    public int movementZone;

    [Range(0, 100)]
    public int charismaCost;

    public enum FidelePeuple {Humain, Gnome, Golem, Elfe, Animal}
    public FidelePeuple fidelePeuple;

    public enum Camp { Fidele, Roi, Bandit, Calamite}
    public Camp currentCamp;
    
    public bool isSelectable = false;
    public bool isInformationsDisplayed = false;
    public bool isInteractionDisplayed = false;
    public bool isMovementDisplayed = false;


    public Canvas myStatisticsCanvas;
    public SpriteRenderer myInteractionZoneSprite;
    public SpriteRenderer myMovementZoneSprite;

    private bool canDisplayInteraction = false;

    private Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelectable && isInteractionDisplayed == false)
        {
            DisplayInteraction();
        }
    }

    public void DisplayInformations()
    {
        myStatisticsCanvas.enabled = true;
        DisplayInteraction();
        DisplayMovement();

        isInformationsDisplayed = true;
    }

    public void HideInformations()
    {
        myStatisticsCanvas.enabled = false;
        HideInteraction();
        HideMovement();

        isInformationsDisplayed = false;
    }

    public void DisplayMovement()
    {
        if (isMovementDisplayed == false)
        {
            myAnim.SetTrigger("ActivateMovement");
            isMovementDisplayed = true;
        }
    }

    public void HideMovement()
    {
        if (isMovementDisplayed)
        {
            myAnim.SetTrigger("ActivateMovement");
            isMovementDisplayed = false;
        }
    }

    public void DisplayInteraction()
    {
        if (isInteractionDisplayed == false)
        {
            myAnim.SetTrigger("ActivateInteraction");
            isInteractionDisplayed = true;
        }
    }

    public void HideInteraction()
    {
        if (isInteractionDisplayed)
        {
            myAnim.SetTrigger("ActivateInteraction");
            isInteractionDisplayed = false;
        }
    }

    public void OnMouseOver()
    {
        DisplayInteraction();

        if (Input.GetMouseButtonDown(1) && isInformationsDisplayed == false)
        {
            DisplayInformations();
        }
        else if (Input.GetMouseButtonDown(1) && isInformationsDisplayed)
        {
            HideInformations();
        }
    }

    public void OnMouseExit()
    {
        HideInteraction();

        /*if (isMoving == false && isInteractionDisplayed == false)
        {
            HideInteraction();
        }*/

        //Debug.Log("Pas survol");
        /*if (isInformationsDisplayed && isMoving == false)
        {
            HideInformations();
            HideMovement();
        }*/
    }
}