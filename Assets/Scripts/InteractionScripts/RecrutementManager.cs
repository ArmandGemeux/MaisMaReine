using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI.Extensions;

public class RecrutementManager : MonoBehaviour
{
    private FideleManager myFMToRecruit;

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

    public void OpenRecruitementWindow(FideleManager fmToRecruit, Sprite fmToRecruitSprite)
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
    }

    public void RecruitUnit()
    {
        if (GameManager.Instance.charismeAmount >= myFMToRecruit.charismaCost)
        {
            GameManager.Instance.isGamePaused = false;
            GameManager.Instance.LowerCharismeValue(myFMToRecruit.charismaCost);
            StartCoroutine(SetCampToFidele());
            myAnim.SetBool("isOpen", false);

            GameManager.Instance.IsAllCampActionsDone();
        }
        else
	    {
            GameManager.Instance.isGamePaused = false;
            myAnim.SetBool("isOpen", false);
            myFMToRecruit = null;
            recruitedSprite = null;

            GameManager.Instance.IsAllCampActionsDone();
            //ICI jouer le clignottement de l'icône de charisme
            //ICI jouer SFX d'impossibilité de recruter le personnage
        }
    }

    public void CancelRecruitUnit()
    {
        GameManager.Instance.isGamePaused = false;
        myAnim.SetBool("isOpen", false);
        myFMToRecruit = null;
        recruitedSprite = null;

        GameManager.Instance.IsAllCampActionsDone();
    }

    public IEnumerator SetCampToFidele()
    {
        QuestManager.Instance.OnRecruitUnit(myFMToRecruit);

        MovementEnemy myMovementScript = myFMToRecruit.GetComponentInChildren<MovementEnemy>();

        Destroy(myFMToRecruit.GetComponent<MouseEventsEnnemi>());
        myFMToRecruit.gameObject.AddComponent<MouseEventsFidele>();

        myMovementScript.gameObject.AddComponent<Movement>();

        Destroy(myMovementScript);
        Destroy(myMovementScript.GetComponent<NavMeshAgent>());

        myFMToRecruit.myCamp = GameCamps.Fidele;
        myFMToRecruit.gameObject.tag = ("Fidele");
        myFMToRecruit.currentHP = myFMToRecruit.maxHp;

        myFMToRecruit.GetComponent<AnimationManager>().UpdateMyReferences();

        myFMToRecruit.GetComponent<AnimationManager>().DesactivateReceiverSelection();
        myFMToRecruit.GetComponent<AnimationManager>().DesactivateLauncherSelection();

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

        recruitedSprite = null;

        myFMToRecruit.isAlive = true;
        myFMToRecruit = null;

        GameManager.Instance.IsAllCampActionsDone();
    }
}
