using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public enum GameCamps { Fidele, Roi, Bandit, Calamite, Converti}

public class GameManager : MonoBehaviour
{
    public GameCamps currentCampTurn;

    public List<GameCamps> campsInTerritoire;

    public float timeValue;
    public bool canMoveEnemy = true; //Public pour playtest

    public bool switchingTurn = false;

    private List<FideleManager> allMapUnits = new List<FideleManager>();

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchTurn()
    {
        switchingTurn = true;
        currentCampTurn += 1;
        switchingTurn = false;

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
                    fm.GetComponentInChildren<MovementEnemy>().hasMoved = false;
                    StartCoroutine(MoveBandit());
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

        var lastTurn = campsInTerritoire.Last();

        if (currentCampTurn == lastTurn+1)
        {
            currentCampTurn = GameCamps.Fidele;
        }

        ResetTurn();
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

    public void ResetTurn()
    {
        foreach (FideleManager fm in allMapUnits)
        {
            fm.GetComponentInChildren<Interaction>().alreadyInteractedList.Clear();
            //Debug.Log("Clear");
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
