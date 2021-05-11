using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    private Animator myAnim;

    public TextMeshProUGUI atkHP;
    public TextMeshProUGUI atkRange;
    public TextMeshProUGUI atkCriticChance;
    public TextMeshProUGUI atkMissChance;

    public TextMeshProUGUI attaquantName;
    public Image attaquantIcone;

    public TextMeshProUGUI defHP;
    public TextMeshProUGUI defCounterAttackRange;

    public TextMeshProUGUI defenseurName;
    public Image defenseurIcone;

    private FideleManager attaquantFM;
    private FideleManager defenseurFM;

    public Sprite reineSprite;
    public Sprite roiSprite;
    public Sprite calamiteSprite;
    public Sprite banditSprite;

    #region Singleton

    public static CombatManager Instance;

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

    public void OpenCombatWindow(FideleManager atkFM, FideleManager defFM)
    {
        attaquantFM = atkFM;
        defenseurFM = defFM;

        myAnim.SetBool("OpenCombatWindow", true);

        switch (atkFM.myCamp)
        {
            case GameCamps.Fidele:
                attaquantIcone.sprite = reineSprite;
                break;
            case GameCamps.Roi:
                attaquantIcone.sprite = roiSprite;
                break;
            case GameCamps.Bandit:
                attaquantIcone.sprite = banditSprite;
                break;
            case GameCamps.BanditCalamiteux:
                attaquantIcone.sprite = calamiteSprite;
                break;
            case GameCamps.Calamite:
                attaquantIcone.sprite = calamiteSprite;
                break;
            case GameCamps.Villageois:
                attaquantIcone.sprite = reineSprite;
                break;
            case GameCamps.Converti:
                attaquantIcone.sprite = roiSprite;
                break;
            default:
                break;
        }

        switch (defFM.myCamp)
        {
            case GameCamps.Fidele:
                defenseurIcone.sprite = reineSprite;
                break;
            case GameCamps.Roi:
                defenseurIcone.sprite = roiSprite;
                break;
            case GameCamps.Bandit:
                defenseurIcone.sprite = banditSprite;
                break;
            case GameCamps.BanditCalamiteux:
                defenseurIcone.sprite = calamiteSprite;
                break;
            case GameCamps.Calamite:
                defenseurIcone.sprite = calamiteSprite;
                break;
            case GameCamps.Villageois:
                defenseurIcone.sprite = reineSprite;
                break;
            case GameCamps.Converti:
                defenseurIcone.sprite = roiSprite;
                break;
            default:
                break;
        }

        attaquantName.text = atkFM.fideleNom + (" ") + atkFM.fidelePrenom;

        atkHP.text = atkFM.currentHP.ToString();
        atkRange.text = atkFM.minAttackRange.ToString() + (" - ") + atkFM.maxAttackRange.ToString();
        atkCriticChance.text = atkFM.criticChances.ToString() + ("%");
        atkMissChance.text = atkFM.missChances.ToString() + ("%");

        defenseurName.text = defFM.fideleNom + (" ") + defFM.fidelePrenom;

        defHP.text = defFM.currentHP.ToString();
        defCounterAttackRange.text = defFM.minCounterAttackRange.ToString() + (" - ") + defFM.maxCounterAttackRange.ToString();
    }

    public void LaunchCombat()
    {
        defenseurFM.GetComponentInChildren<Combat>().StartFight(attaquantFM);

        myAnim.SetBool("OpenCombatWindow", false);

        attaquantFM = null;
        defenseurFM = null;
    }

    public void CancelCombat()
    {
        myAnim.SetBool("OpenCombatWindow", false);

        attaquantFM = null;
        defenseurFM = null;
    }
}
