using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class AnimationManager : MonoBehaviour
{
    [HideInInspector]
    public bool isSelectable = false;
    [HideInInspector]
    public bool isSelected = false;

    private FideleManager myFM;

    [HideInInspector]
    public bool isStatsDisplayed = false;
    [HideInInspector]
    public bool isInteractionDisplayed = false;
    [HideInInspector]
    public bool isMovementDisplayed = false;

    [HideInInspector]
    public bool isInfoDisplayed = false;
    private bool isLightOn = false;
    private bool isLightColorSwitched = false;

    [HideInInspector]
    public bool haveAnInteraction = false;
    [HideInInspector]
    public bool movementZoneIsTouchingInteraction = false;

    private Animator myAnim;
    private Movement myMovement;
    private MovementEnemy myMovementEnemy;
    private Light2D myLight;
    private Interaction myInteraction;

    public Canvas myStatsCanvas;
    public TextMeshProUGUI currentHpOnCanvas;
    public TextMeshProUGUI currentAttackRangeOnCanvas;

    public Image healthAmountImage;

    public SpriteRenderer myMovementZone;

    public SpriteRenderer myOutline;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        myFM = GetComponent<FideleManager>();
        myLight = GetComponentInChildren<Light2D>();
        myInteraction = GetComponentInChildren<Interaction>();

        if (GetComponentInChildren<Movement>() != null)
        {
            myMovement = GetComponentInChildren<Movement>();
        }
        else if (GetComponentInChildren<MovementEnemy>() != null)
        {
            myMovementEnemy = GetComponentInChildren<MovementEnemy>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentHpOnCanvas.text = myFM.currentHP.ToString();
        if (myFM.myCamp == GameCamps.Fidele)
        {
            currentAttackRangeOnCanvas.text = (myFM.minAttackRange.ToString() + " - " + myFM.maxAttackRange.ToString());
        }
        else
        {
            currentAttackRangeOnCanvas.text = ("??");
        }
    }

    /*public void AbleToPlay()
    {
        myAnim.SetBool("isCharacterAblePlay", true);
    }

    public void UnableToPlay()
    {
        myAnim.SetBool("isCharacterAblePlay", false);
    }*/

    public void ReceiveDamage()
    {
        myAnim.SetTrigger("CharacterReceiveDamage");
    }

    public void Dying()
    {
        myAnim.SetBool("isCharacterDead", true);
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
            myStatsCanvas.enabled = true;
            isStatsDisplayed = true;
        }
    }

    public void HideStats()
    {
        if (isStatsDisplayed)
        {
            myStatsCanvas.enabled = false;
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

    public void FillAmountHealth()
    {
        Debug.Log("Test");
        healthAmountImage.fillAmount = myFM.currentHP*1f / myFM.maxHp*1f;
    }

    public void DisplayInteractionIcon()
    {
        myInteraction.myInteractionIcon.enabled = true;

        switch (myInteraction.interactionType)
        {
            case InteractionType.Dialogue:
                myInteraction.myInteractionIcon.sprite = myInteraction.dialogueIcon;
                break;
            case InteractionType.Recrutement:
                myInteraction.myInteractionIcon.sprite = myInteraction.recrutementIcon;
                break;
            case InteractionType.Combat:
                myInteraction.myInteractionIcon.sprite = myInteraction.combatIcon;
                break;
            default:
                break;
        }
    }

    public void HideInteractionIcon()
    {
        myInteraction.myInteractionIcon.enabled = false;

        myInteraction.myInteractionIcon.sprite = null;
    }
}
