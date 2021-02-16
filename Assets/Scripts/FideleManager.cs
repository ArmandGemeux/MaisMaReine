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
    public bool isDisplayed = false;


    private bool isMoving = false;

    public Canvas myStatisticsCanvas;
    public SpriteRenderer myInteractionZoneSprite;
    public SpriteRenderer myMovementZoneSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelectable)
        {
            myInteractionZoneSprite.enabled = true;
        }
        else
        {
            myInteractionZoneSprite.enabled = false;
        }
    }

    public void DisplayInformations()
    {
        isDisplayed = true;

        myStatisticsCanvas.enabled = true;
        myInteractionZoneSprite.enabled = true;
        myMovementZoneSprite.enabled = true;
    }

    public void HideInformations()
    {
        isDisplayed = false;

        myStatisticsCanvas.enabled = false;
        myInteractionZoneSprite.enabled = false;
        myMovementZoneSprite.enabled = false;
    }

    public void DisplayMovement()
    {
        myMovementZoneSprite.enabled = true;
        myStatisticsCanvas.enabled = false;
        isMoving = true;
    }

    public void HideMovement()
    {
        myMovementZoneSprite.enabled = false;
        myInteractionZoneSprite.enabled = false;
        isMoving = false;
    }

    public void DisplayInteraction()
    {
        myInteractionZoneSprite.enabled = true;
    }

    public void HideInteraction()
    {
        myInteractionZoneSprite.enabled = false;
    }

    public void OnMouseOver()
    {
        Debug.Log("Survol");

        myInteractionZoneSprite.enabled = true;

        if (Input.GetMouseButtonDown(1))
        {
            DisplayInformations();
        }
    }

    public void OnMouseExit()
    {
        if (isMoving == false)
        {
            myInteractionZoneSprite.enabled = false;
        }

        Debug.Log("Pas survol");
        if (isDisplayed && isMoving == false)
        {
            HideInformations();
        }
    }
}