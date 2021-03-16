using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum CampTurn { Fidele, Roi/*, Bandit, Calamite*/, noMoreCamp}
    public CampTurn currentCampTurn;

    public float timeValue;
    public bool canMoveEnemy = true; //Public pour playtest

    public bool switchingTurn = false;

    public List<FideleManager> myFideleList = new List<FideleManager>();

    public List<FideleManager> myRoiList = new List<FideleManager>();

    public List<FideleManager> myBanditList = new List<FideleManager>();

    public List<FideleManager> myCalamiteList = new List<FideleManager>();

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
        myRoiList.AddRange(FindObjectsOfType<FideleManager>());
        myBanditList.AddRange(FindObjectsOfType<FideleManager>());
        myCalamiteList.AddRange(FindObjectsOfType<FideleManager>());
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        foreach (FideleManager myFidele in myFideleList)
        {
            if (myFidele.currentCamp.ToString() != CampTurn.Fidele.ToString())
            {
                myFideleList.Remove(myFidele);
            }
        }

        foreach (FideleManager myRoi in myRoiList)
        {
            if (myRoi.currentCamp.ToString() != CampTurn.Roi.ToString())
            {
                myRoiList.Remove(myRoi);
            }
        }

        /*foreach (FideleManager myBandit in myBanditList)
        {
            if (myBandit.currentCamp.ToString() != CampTurn.Bandit.ToString())
            {
                myBanditList.Remove(myBandit);
            }
        }

        foreach (FideleManager myCalamite in myCalamiteList)
        {
            if (myCalamite.currentCamp.ToString() != CampTurn.Calamite.ToString())
            {
                myCalamiteList.Remove(myCalamite);
            }
        }*/
    }

    public void SwitchTurn()
    {
        switchingTurn = true;
        currentCampTurn += 1;
        switchingTurn = false;

        if (currentCampTurn == CampTurn.Roi)
        {
            //MoveEnemy();
            StartCoroutine(MoveEnemies());
        }

        if (currentCampTurn == CampTurn.noMoreCamp)
        {
            currentCampTurn = CampTurn.Fidele;
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
        foreach (FideleManager myRoi in myRoiList)
        {
            myRoi.GetComponentInChildren<MovementEnemy>().hasMoved = false;
        }
    }

    public void MoveEnemy()
    {
        if (canMoveEnemy)
        {
            for (int i = 0; i < myRoiList.Count; i++)
            {
                if (myRoiList[i].GetComponentInChildren<MovementEnemy>().hasMoved == false)
                {
                    myRoiList[i].GetComponentInChildren<MovementEnemy>().FindTarget();
                    Debug.Log(myRoiList[i].name);
                    StartCoroutine(Delay(timeValue));
                }

                /*if (canMoveEnemy)
                {
                    myRoiList[i].GetComponentInChildren<MovementEnemy>().FindTarget();
                    Debug.Log(myRoiList[i].name);
                    StartCoroutine(Delay(timeValue));
                }*/

            }
        }
    }

    public IEnumerator MoveEnemies()
    {
        for (int i = 0; i < myRoiList.Count; i++)
        {
            myRoiList[i].GetComponentInChildren<MovementEnemy>().FindTarget();
            Debug.Log(myRoiList[i].name);
            yield return new WaitForSeconds(timeValue);
        }
    }

    private IEnumerator Delay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }
}
