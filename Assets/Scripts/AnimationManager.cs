using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    public bool isSelectable = false;
    public bool isStatsDisplayed = false;
    public bool isInteractionDisplayed = false;
    public bool isMovementDisplayed = false;

    public bool isInfoDisplayed = false;
    private bool isLightOn = false;
    private bool isLightColorSwitched = false;

    public bool haveAnInteraction = false;
    public bool movementZoneIsTouchingInteraction = false;

    private Animator myAnim;
    private Movement myMovement;
    private Light2D myLight;
    private Interaction myInteraction;

    public Image myStatisticsImage;
    public Image interactionClickFeedback;
    public Image movementClickFeedback;

    public SpriteRenderer myMovementZone;

    public SpriteRenderer myOutline;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        myMovement = GetComponentInChildren<Movement>();
        myLight = GetComponentInChildren<Light2D>();
        myInteraction = GetComponentInChildren<Interaction>();
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

    public void UnableToLand()
    {
        myMovementZone.color = Color.red;
    }

    public void AbleToLand()
    {
        myMovementZone.color = Color.green;
    }

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
}
