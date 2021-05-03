using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum GameCamps { Fidele, Roi, Bandit, BanditCalamiteux, Calamite, Villageois, Converti}

public class GameManager : MonoBehaviour
{
    public GameCamps currentCampTurn;

    public List<GameCamps> campsInTerritoire;

    public float timeValue;
    public bool canMoveEnemy = true; //Public pour playtest

    public bool switchingTurn = false;

    private List<FideleManager> allMapUnits = new List<FideleManager>();

    public int charismeAmount;

    public bool isGamePaused;

    public ParticleSystem myCampTurningFeedback;

    static public int charismeAmountStatic;

    #region Singleton
    public static GameManager Instance;

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

        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        isGamePaused = false;
        charismeAmount = charismeAmountStatic;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IsAllCampActionsDone()
    {
        for (int i = 0; i < allMapUnits.Count; i++)
        {
            if (allMapUnits[i].myCamp == currentCampTurn)
            {
                if (allMapUnits[i].isAllActionsDone == false)
                {
                    myCampTurningFeedback.gameObject.SetActive(false);
                    return;
                }                
            }
        }
        myCampTurningFeedback.gameObject.SetActive(true);
        Debug.Log("Toutes les actions ont été effectuées");

        if (currentCampTurn != GameCamps.Fidele)
        {
            SwitchTurn();
        }
    }

    public void LoadCharismeValueBetweenScenes()
    {
        charismeAmountStatic = charismeAmount;
    }

    public void AddCharismeValue(int addedCharismeValue)
    {
        charismeAmount += addedCharismeValue;

        if (charismeAmount <= 0)
        {
            charismeAmount = 0;
        }

        if (addedCharismeValue > 0)
        {
            StartCoroutine(RecrutementManager.Instance.AddCharismeAmount(addedCharismeValue));
        }
    }

    public void LowerCharismeValue(int lowerCharismeValue)
    {
        charismeAmount -= lowerCharismeValue;

        if (charismeAmount <= 0)
        {
            charismeAmount = 0;
        }

        if (lowerCharismeValue > 0)
        {
            StartCoroutine(RecrutementManager.Instance.LowerCharismeAmount(lowerCharismeValue));
        }
    }

    public void SwitchTurn()
    {
        myCampTurningFeedback.gameObject.SetActive(false);

        switchingTurn = true;
        currentCampTurn += 1;
        switchingTurn = false;

        var lastTurn = campsInTerritoire.Last();

        if (currentCampTurn == lastTurn + 1)
        {
            currentCampTurn = campsInTerritoire[0];
        }

        ResetTurn();

        if (currentCampTurn == GameCamps.Fidele)
        {
            foreach (FideleManager fm in allMapUnits)
            {
                if (fm.myCamp == GameCamps.Fidele)
                {
                    fm.GetComponentInChildren<Movement>().hasMoved = false;
                }
            }
        }

        if (currentCampTurn == GameCamps.Roi)
        {
            foreach (FideleManager fm in allMapUnits)
            {
                if (fm.myCamp == GameCamps.Roi)
                {
                    fm.GetComponentInChildren<MovementEnemy>().hasMoved = false;
                    StartCoroutine(MoveRoi());
                }
            }
        }

        if (currentCampTurn == GameCamps.Bandit)
        {
            foreach (FideleManager fm in allMapUnits)
            {
                if (fm.myCamp == GameCamps.Bandit)
                {
                    if (fm.isAlive)
                    {
                        fm.GetComponentInChildren<MovementEnemy>().hasMoved = false;
                        StartCoroutine(MoveBandit());
                    }
                }
            }
        }

        if (currentCampTurn == GameCamps.Calamite)
        {
            foreach (FideleManager fm in allMapUnits)
            {
                if (fm.myCamp == GameCamps.Calamite)
                {
                    fm.GetComponentInChildren<MovementEnemy>().hasMoved = false;
                    StartCoroutine(MoveCalamite());
                }
            }
        }
    }

    public void AddAMapUnit(FideleManager newUnit)
    {
        if (!allMapUnits.Contains(newUnit))
        {
            allMapUnits.Add(newUnit);
        }
    }

    public void RemoveAMapUnit(FideleManager removeUnit)
    {
        if (allMapUnits.Contains(removeUnit))
        {
            allMapUnits.Remove(removeUnit);
        }
    }

    public List<FideleManager> GetAllMapUnits()
    {
        return allMapUnits;
    }

    public void CheckIfTurnIsEnded()
    {
        /*foreach (FideleManager fmpc in allMapUnits)
        {
            if (fmpc.myCamp == currentCampTurn && fmpc.myCamp != GameCamps.Fidele)
            {
                if (fmpc.GetComponentInChildren<MovementEnemy>().hasMoved)
                {

                }
            }
        }*/
    }

    public void ResetTurn()
    {
        foreach (FideleManager fm in allMapUnits)
        {
            fm.GetComponentInChildren<Interaction>().alreadyFightedList.Clear();
            fm.GetComponentInChildren<Interaction>().alreadyInteractedList.Clear();
        }
    }

    public IEnumerator MoveRoi()
    {
        for (int i = 0; i < allMapUnits.Count; i++)
        {
            if (allMapUnits[i].myCamp == GameCamps.Roi)
            {
                allMapUnits[i].GetComponentInChildren<MovementEnemy>().MoveToTarget();
                yield return new WaitForSeconds(timeValue);
            }
        }
    }

    public IEnumerator MoveBandit()
    {
        for (int i = 0; i < allMapUnits.Count; i++)
        {
            if (allMapUnits[i].myCamp == GameCamps.Bandit)
            {
                allMapUnits[i].GetComponentInChildren<MovementEnemy>().MoveToTarget();
                yield return new WaitForSeconds(timeValue);
            }
        }
    }

    public IEnumerator MoveCalamite()
    {
        for (int i = 0; i < allMapUnits.Count; i++)
        {
            if (allMapUnits[i].myCamp == GameCamps.Calamite)
            {
                allMapUnits[i].GetComponentInChildren<MovementEnemy>().MoveToTarget();
                yield return new WaitForSeconds(timeValue);
            }
        }
    }    
}