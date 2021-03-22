using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class FideleManager : MonoBehaviour
{
    public GameCamps myCamp;

    [Header("Caractéristiques")]
    public string fideleNom;
    public string fidelePrenom;

    public int maxHp;
    public int currentHP;
    public bool isAlive;

    [Range(4, 10)]
    public int attackRange;
    [Range(1, 3)]
    public int counterAttackRange;

    [Range(0, 100)]
    public int criticChances;
    [Range(0, 100)]
    public int failChances;

    [Range(0, 100)]
    public int charismaCost;

    public enum FidelePeuple {Humain, Gnome, Golem, Elfe, Animal}
    public FidelePeuple fidelePeuple;

    public FideleManager myTarget;
    private float myTargetScore;

    private List<GameCamps> attackableUnits = new List<GameCamps>();

    private List<FideleManager> unitsInRange = new List<FideleManager>();
    private List<FideleManager> attackableUnitsInRange = new List<FideleManager>();

    void Start()
    {
        SetAttackableUnits();
        currentHP = maxHp;
        isAlive = true;
    }

    private void SetAttackableUnits()
    {
        switch (myCamp)
        {
            case GameCamps.Roi:
                attackableUnits.Add(GameCamps.Fidele);
                attackableUnits.Add(GameCamps.Calamite);
                break;
            case GameCamps.Bandit:
                attackableUnits.Add(GameCamps.Fidele);
                attackableUnits.Add(GameCamps.Roi);
                attackableUnits.Add(GameCamps.Converti);
                break;
            case GameCamps.Calamite:
                attackableUnits.Add(GameCamps.Fidele);
                attackableUnits.Add(GameCamps.Roi);
                attackableUnits.Add(GameCamps.Converti);
                attackableUnits.Add(GameCamps.Bandit);
                break;
            case GameCamps.Converti:
                attackableUnits.Add(GameCamps.Calamite);
                break;
            default:
                break;
        }
    }

    public void SetToConverti()
    {
        myCamp = GameCamps.Converti;
    }

    public void DetermineMyTarget()
    {
        if (unitsInRange.Count > 0)
        {
            attackableUnitsInRange = AttackableUnitsInRangeAmount();
            if (attackableUnitsInRange.Count == 1)
            {
                myTarget = attackableUnitsInRange[0];
                //Debug.Log(myTarget.name + " ciblée !");
                return;
            }
            else if (attackableUnitsInRange.Count == 0)
            {
                Debug.Log("Personne dans ma zone de mouvement");
                return;
            }
            else if (attackableUnitsInRange.Count > 1)
            {
                myTarget = GetInRangeUnitWithSmallestHp();
            }
        }
        else if (unitsInRange.Count == 0)
        {
            Debug.Log("Personne aux alentours");
        }

        /*switch (myCamp)
        {
            case GameCamps.Roi:
                break;
            case GameCamps.Bandit:
                break;
            case GameCamps.Calamite:
                break;
            case GameCamps.Converti:
                break;
            default:
                break;
        }*/
    }

    public void AddUnitInRange (FideleManager uir)
    {
        unitsInRange.Add(uir);
    }

    public void RemoveUnitInRange(Collider2D cir)
    {
        FideleManager tmpFM = cir.GetComponentInParent<FideleManager>();
        if (tmpFM)
        {
            if (unitsInRange.Contains(tmpFM))
            {
                unitsInRange.Remove(tmpFM);
                if (tmpFM == myTarget)
                {
                    myTarget = null;
                }
            }
        }
    }

    public float GetTargetScore()
    {
        return myTargetScore;
    }

    private List<FideleManager> AttackableUnitsInRangeAmount()
    {
        List<FideleManager> tmpauir = new List<FideleManager>();
        foreach (FideleManager fm in unitsInRange)
        {
            if (attackableUnits.Contains(fm.myCamp))
            {
                tmpauir.Add(fm);
            }
        }
        return tmpauir;
    }

    private FideleManager GetInRangeUnitWithSmallestHp()
    {
        float tmpSmallHp = 9999f;
        FideleManager tmpFm = attackableUnitsInRange[0];

        foreach (FideleManager fm in attackableUnitsInRange)
        {
            if (fm.currentHP < tmpSmallHp)
            {
                tmpSmallHp = fm.currentHP;
                tmpFm = fm;
            }
        }
        return tmpFm;
    }
}