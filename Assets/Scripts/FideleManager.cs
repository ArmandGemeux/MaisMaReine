using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

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
    public bool isStatsDisplayed = false;
    public bool isInteractionDisplayed = false;
    public bool isMovementDisplayed = false;

    private bool isInfoDisplayed = false;
    private bool isLightOn = false;
    private bool isLightColorSwitched = false;

    public bool haveAnInteraction = false;
    public bool movementZoneIsTouchingInteraction = false;

    private Animator myAnim;
    private Movement myMovement;
    private Light2D myLight;
    public Image myStatisticsImage;
    public Image interactionClickFeedback;
    public Image movementClickFeedback;

    public SpriteRenderer myOutline;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        myMovement = GetComponentInChildren<Movement>();
        myLight = GetComponentInChildren<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelectable && isInteractionDisplayed == false)
        {
            DisplayInteraction();
        }
    }

    public void ActivateLauncherSelection()
    {
        myAnim.SetBool("isInteractionLauncherSelected", true);
    }

    public void DesactivateLauncherSelection()
    {
        myAnim.SetBool("isInteractionLauncherSelected", false);
    }

    public void ToggleLauncherOutline()
    {
        myOutline.enabled = !myOutline.enabled;
    }

    public void ActivateReceiverSelection()
    {
        myAnim.SetBool("isInteractionReceiverCanInteract", true);
    }

    public void DesactivateReceiverSelection()
    {
        myAnim.SetBool("isInteractionReceiverCanInteract", false);
    }

    public void DisplayStats()
    {
        if (isStatsDisplayed == false)
        {
            myStatisticsImage.enabled = true;
            isStatsDisplayed = true;
        }
    }

    public void HideStats()
    {
        if (isStatsDisplayed)
        {
            myStatisticsImage.enabled = false;
            isStatsDisplayed = false;
        }
    }

    /*public void DislayInteractionLight()
    {
        if (myLight != null)
        {
            if (isLightOn == false)
            {
                myLight.enabled = true;
                isLightOn = true;
            }
        }
    }*/

    /*public void HideInteractionLight()
    {
        if (myLight != null)
        {
            if (isLightOn == true)
            {
                myLight.enabled = false;
                isLightOn = false;
            }
        }
    }*/

    /*public void SwitchInteractionLightColor()
    {
        if (myLight != null)
        {
            if (isLightOn == true && isLightColorSwitched == false)
            {
                Debug.Log("Switch");
                myAnim.SetTrigger("ActivateLight");
                isLightColorSwitched = true;
            }
        }
    }*/

    /*public void SwitchBackInteractionLightColor()
    {
        if (myLight != null)
        {
            if (isLightOn == true && isLightColorSwitched == true)
            {
                myAnim.SetTrigger("ActivateLight");
                isLightColorSwitched = false;
            }
        }
    }*/

    public void DisplayMovement()
    {
        if (isMovementDisplayed == false)
        {
            myAnim.SetBool("ActivateMovementBool", true);
            isMovementDisplayed = true;
        }
    }

    public void HideMovement()
    {
        if (isMovementDisplayed)
        {
            myAnim.SetBool("ActivateMovementBool", false);
            isMovementDisplayed = false;
        }
    }

    public void DisplayInteraction()
    {
        if (isInteractionDisplayed == false)
        {
            myAnim.SetBool("ActivateInteractionBool", true);
            isInteractionDisplayed = true;
        }
    }

    public void HideInteraction()
    {
        if (isInteractionDisplayed && haveAnInteraction == false)
        {
            myAnim.SetBool("ActivateInteractionBool", false);
            isInteractionDisplayed = false;
        }
    }

    public void OnMouseEnter()
    {
        DisplayInteraction();
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && isInfoDisplayed == false)
        {           
            DisplayMovement();
            DisplayStats();
            isInfoDisplayed = true;
        }
        else if (Input.GetMouseButtonDown(1) && isInfoDisplayed)
        {
            HideMovement();
            HideStats();
            isInfoDisplayed = false;
        }
    }

    public void OnMouseExit()
    {
        if (isInfoDisplayed && myMovement.isMoving == false)
        {
            HideMovement();
            HideStats();
            isInfoDisplayed = false;
        }

        if (myMovement.isMoving == false)
        {
            HideInteraction();
            HideStats();
        }
    }
}