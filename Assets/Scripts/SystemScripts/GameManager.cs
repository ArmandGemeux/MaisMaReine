using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameCamps { Fidele, Roi, Bandit, Calamite, Converti}

public class GameManager : MonoBehaviour
{
    public GameCamps currentCampTurn;

    public List<GameCamps> campsInTerritoire;

    public float timeValue;
    public bool canMoveEnemy = true; //Public pour playtest

    public bool switchingTurn = false;

    public List<FideleManager> myFideleList;

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

        myFideleList.AddRange(FindObjectsOfType<FideleManager>());
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
            foreach (FideleManager fm in myFideleList)
            {
                if (fm.myCamp == GameCamps.Roi)
                {
                    StartCoroutine(MoveEnemies());
                    fm.GetComponentInChildren<MovementEnemy>().hasMoved = false;
                }
            }
        }

        if (currentCampTurn == GameCamps.Converti)
        {
            currentCampTurn = GameCamps.Fidele;
        }

        ResetTurn();
    }

    public void ResetTurn()
    {
        foreach (FideleManager myFidele in myFideleList)
        {
            myFidele.GetComponentInChildren<Interaction>().alreadyInteractedList.Clear();
            //Debug.Log("Clear");
        }
    }

    public IEnumerator MoveEnemies()
    {
        for (int i = 0; i < myFideleList.Count; i++)
        {
            if (myFideleList[i].myCamp == GameCamps.Roi)
            {
                if (myFideleList[i].GetComponentInChildren<MovementEnemy>().targetLanded == false)
                {
                    myFideleList[i].GetComponentInChildren<MovementEnemy>().FindTarget();
                    yield return new WaitForSeconds(timeValue);
                }
            }
            else
            {
                i++;
            }
        }
    }
}
