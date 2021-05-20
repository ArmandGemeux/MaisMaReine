﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI.Extensions;

public class RecrutementManager : MonoBehaviour
{
    private FideleManager myFMToRecruit;
    private FideleManager myRecruiterFM;

    public GameObject myFideleParent;

    #region Nom et Camp
    [Header("Nom et Camp")]
    public TextMeshProUGUI characterNom;

    public TextMeshProUGUI characterCamp;
    #endregion

    #region Caractéristiques
    [Header("Caractéristiques")]
    public TextMeshProUGUI hpValue;

    public TextMeshProUGUI moveZoneValue;

    public TextMeshProUGUI attackRangeValue;

    public TextMeshProUGUI criticChancesValue;

    public TextMeshProUGUI counterAttackValue;

    public TextMeshProUGUI missChancesValue;

    public TextMeshProUGUI charismeCostValue;

    private Animator myAnim;

    private Sprite recruitedSprite;
    #endregion

    [Header("Feedbacks et Charisme")]

    public TextMeshProUGUI totalCharismeAmountText;
    public TextMeshProUGUI rewardCharismeAmountText;

    public ParticleSystem gainCharismePS;

    public ParticleSystem recruitEffect;

    #region Singleton
    public static RecrutementManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();

        StartCoroutine(AddCharismeAmount(GameManager.Instance.charismeAmount));
    }

    public IEnumerator AddCharismeAmount(int addedCharismeValue)
    {
        rewardCharismeAmountText.text = "+ " + addedCharismeValue.ToString();
        gainCharismePS.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.6f);

        totalCharismeAmountText.text = GameManager.Instance.charismeAmount.ToString();

        yield return new WaitForSeconds(2f);

        gainCharismePS.gameObject.SetActive(false);
    }

    public IEnumerator LowerCharismeAmount(int lowerCharismeValue)
    {
        rewardCharismeAmountText.text = "- " + lowerCharismeValue.ToString();
        gainCharismePS.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.6f);

        totalCharismeAmountText.text = GameManager.Instance.charismeAmount.ToString();

        yield return new WaitForSeconds(2f);

        gainCharismePS.gameObject.SetActive(false);
    }

    public void OpenRecruitementWindow(FideleManager fmToRecruit, Sprite fmToRecruitSprite, FideleManager recruiterFM)
    {
        myAnim.SetBool("isOpen", true);
        recruitedSprite = fmToRecruitSprite;

        characterNom.text = fmToRecruit.fidelePrenom + " " + fmToRecruit.fideleNom;
        characterCamp.text = fmToRecruit.myCamp.ToString();

        hpValue.text = fmToRecruit.maxHp.ToString();
        moveZoneValue.text = fmToRecruit.moveZoneValue.ToString();

        attackRangeValue.text = fmToRecruit.minAttackRange.ToString() + " - " + fmToRecruit.maxAttackRange.ToString();
        criticChancesValue.text = fmToRecruit.criticChances.ToString() + "%";

        counterAttackValue.text = fmToRecruit.minCounterAttackRange.ToString() + " - " + fmToRecruit.maxCounterAttackRange.ToString();
        missChancesValue.text = fmToRecruit.missChances.ToString() + "%";

        charismeCostValue.text = " : " + fmToRecruit.charismaCost.ToString();

        myFMToRecruit = fmToRecruit;
        myRecruiterFM = recruiterFM;
    }

    public void RecruitUnit()
    {
        if (GameManager.Instance.charismeAmount >= myFMToRecruit.charismaCost)
        {
            GameManager.Instance.isGamePaused = false;
            GameManager.Instance.LowerCharismeValue(myFMToRecruit.charismaCost);
            StartCoroutine(SetCampToFidele());
            myAnim.SetBool("isOpen", false);

            
        }
        else
	    {
            GameManager.Instance.isGamePaused = false;
            myAnim.SetBool("isOpen", false);

            myRecruiterFM.GetComponent<AnimationManager>().CheckActionsLeftAmout();

            myFMToRecruit = null;
            recruitedSprite = null;

            //ICI jouer le clignottement de l'icône de charisme
            //ICI jouer SFX d'impossibilité de recruter le personnage
        }
    }

    public void CancelRecruitUnit()
    {
        GameManager.Instance.isGamePaused = false;
        myAnim.SetBool("isOpen", false);

        myRecruiterFM.GetComponent<AnimationManager>().CheckActionsLeftAmout();

        myFMToRecruit = null;
        recruitedSprite = null;
        
    }

    public IEnumerator SetCampToFidele()
    {
        QuestManager.Instance.OnRecruitUnit(myFMToRecruit);        

        myFMToRecruit.myCamp = GameCamps.Fidele;
        myFMToRecruit.gameObject.tag = ("Fidele");

        MovementEnemy myMovementScript = myFMToRecruit.GetComponentInChildren<MovementEnemy>();

        Destroy(myFMToRecruit.GetComponent<MouseEventsEnnemi>());
        myFMToRecruit.gameObject.AddComponent<MouseEventsFidele>();

        myMovementScript.gameObject.AddComponent<Movement>();

        Destroy(myMovementScript);
        Destroy(myMovementScript.GetComponent<NavMeshAgent>());

        myFMToRecruit.currentHP = myFMToRecruit.maxHp;

        myFMToRecruit.GetComponent<AnimationManager>().UpdateMyReferences();


        // ICI jouer VFX de changement d'apparence du personnage
        yield return new WaitForSeconds(0.1f);

        DragCamera2D.Instance.FollowTargetCamera(myFMToRecruit.gameObject);

        recruitEffect.gameObject.transform.position = myFMToRecruit.transform.position;
        recruitEffect.Play();
        // ICI jouer SFX de changement d'apparence du personnage

        yield return new WaitForSeconds(0.2f);

        DragCamera2D.Instance.UnfollowTargetCamera();

        myFMToRecruit.GetComponentInChildren<Interaction>().myInteractionIcon.sprite = null;
        myFMToRecruit.fideleSprite.sprite = recruitedSprite;

        myFMToRecruit.transform.SetParent(myFideleParent.transform);

        /*for (int i = 0; i < myFMToRecruit.GetComponentInChildren<Interaction>().myCollideInteractionList.Count; i++)
        {
            if (myFMToRecruit.GetComponentInChildren<Interaction>().myCollideInteractionList[i].myFideleManager.myCamp == myFMToRecruit.myCamp)
            {
                Debug.Log("On remove des interactions");
                myFMToRecruit.GetComponentInChildren<Interaction>().RemoveCollindingCharacterFromInteractionList(myFMToRecruit.GetComponentInChildren<Interaction>().myCollideInteractionList[i]);
                myFMToRecruit.GetComponentInChildren<Interaction>().RemoveCollidingCharacterFromAMList(myFMToRecruit.GetComponentInChildren<Interaction>().myCollideInteractionList[i].GetComponentInParent<AnimationManager>());

                myFMToRecruit.GetComponentInChildren<Interaction>().myCollideInteractionList[i].DisplayInteractionFeedbacks();
            }
        }*/

        foreach (AnimationManager cam in myFMToRecruit.GetComponentInChildren<Interaction>().myCollideAnimationManagerList)
        {
            //Debug.Log(cam.name + "test");
            //cam.CheckActionsLeftAmout();
            cam.haveAnInteraction = false;

            cam.HideInteraction();

            cam.GetComponentInChildren<Interaction>().RemoveCollidingCharacterFromAMList(myFMToRecruit.GetComponent<AnimationManager>());
        }

        foreach (Interaction iam in myFMToRecruit.GetComponentInChildren<Interaction>().myCollideInteractionList)
        {
            iam.RemoveCollindingCharacterFromInteractionList(myFMToRecruit.GetComponentInChildren<Interaction>());

            iam.FideleDisplayInteractionFeedbacks();
        }

        myFMToRecruit.GetComponentInChildren<Interaction>().myCollideAnimationManagerList.Clear();
        myFMToRecruit.GetComponentInChildren<Interaction>().myCollideInteractionList.Clear();

        myFMToRecruit.GetComponent<AnimationManager>().haveAnInteraction = false;

        myFMToRecruit.GetComponent<AnimationManager>().HideInteraction();
        myFMToRecruit.GetComponent<AnimationManager>().DesactivateReceiverSelection();
        
        myFMToRecruit.GetComponent<AnimationManager>().CheckActionsLeftAmout();

        GameManager.Instance.GlobalActionsCheck();

        recruitedSprite = null;

        myFMToRecruit.isAlive = true;
        myFMToRecruit = null;
        myRecruiterFM = null;
    }
}
