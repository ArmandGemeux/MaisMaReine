using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class RecrutementManager : MonoBehaviour
{
    private FideleManager myFMToRecruit;

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

    private Animator myAnim;

    private Sprite recruitedSprite;
    #endregion

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
    }

    // Update is called once per frame
    void Update()
    {
        
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


        myFMToRecruit = fmToRecruit;
    }

    public void RecruitUnit()
    {
        SetCampToFidele();

        myAnim.SetBool("isOpen", false);
        myFMToRecruit.isAlive = true;
        myFMToRecruit = null;
    }

    public void CancelRecruitUnit()
    {
        myAnim.SetBool("isOpen", false);
        myFMToRecruit = null;
        recruitedSprite = null;
    }

    private void SetCampToFidele()
    {
        MovementEnemy myMovementScript = myFMToRecruit.GetComponentInChildren<MovementEnemy>();

        Destroy(myFMToRecruit.GetComponent<MouseEventsEnnemi>());
        myFMToRecruit.gameObject.AddComponent<MouseEventsFidele>();

        myMovementScript.gameObject.AddComponent<Movement>();

        Destroy(myMovementScript);
        Destroy(myMovementScript.GetComponent<NavMeshAgent>());

        myFMToRecruit.myCamp = GameCamps.Fidele;
        myFMToRecruit.gameObject.tag = ("Fidele");
        myFMToRecruit.currentHP = myFMToRecruit.maxHp;
        myFMToRecruit.fideleSprite.sprite = recruitedSprite;
        myFMToRecruit.GetComponent<AnimationManager>().DesactivateReceiverSelection();

        QuestManager.Instance.OnRecruitUnit(myFMToRecruit);
        recruitedSprite = null;
    }
}
