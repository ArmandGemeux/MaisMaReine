using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum CampTurn { Fidele, Roi, /*Bandit, Calamite,*/ noMoreCamp}
    public CampTurn currentCampTurn;

    public bool switchingTurn = false;

    public List<FideleManager> myFideleList = new List<FideleManager>();

    #region Singleton
    public static GameManager GM;

    private void Awake()
    {
        if (GM != null)
        {
            Destroy(this);
        }
        else
        {
            GM = this;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        myFideleList.AddRange(FindObjectsOfType<FideleManager>());
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
    }

    public void SwitchTurn()
    {
        switchingTurn = true;
        currentCampTurn += 1;
        switchingTurn = false;

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
            Debug.Log("Clear");
        }
    }
}
