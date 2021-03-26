using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Recrutement : MonoBehaviour
{
    private FideleManager myFM;
    private MovementEnemy myMovementEnemy;
    // Start is called before the first frame update
    void Start()
    {
        myFM = GetComponentInParent<FideleManager>();
        myMovementEnemy = GetComponentInParent<MovementEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenRecruitementWindow(FideleManager fmToRecruit)
    {
        Debug.Log("Vous recrutez " + fmToRecruit.fideleNom + " " + fmToRecruit.fidelePrenom);
        if (myFM.myCamp == GameCamps.Bandit)
            SetBanditCampToFidele();

        /*else if (myFM.myCamp == GameCamps.Villageois)
            SetVillageoisCampToFidele()*/
    }

    private void SetBanditCampToFidele()
    {
        Destroy(myFM.GetComponent<MouseEventsEnnemi>());
        myFM.gameObject.AddComponent<MouseEventsFidele>();

        myMovementEnemy.gameObject.AddComponent<Movement>();

        Destroy(myMovementEnemy);
        Destroy(myMovementEnemy.GetComponent<NavMeshAgent>());

        myFM.myCamp = GameCamps.Fidele;

        QuestEvents.Instance.EntityRecruited();
        return;
    }

    private void SetVillageoisCampToFidele()
    {
        Destroy(myFM.GetComponent<MouseEventsEnnemi>());
        myFM.gameObject.AddComponent<MouseEventsFidele>();

        myMovementEnemy.gameObject.AddComponent<Movement>();

        Destroy(myMovementEnemy);
        Destroy(myMovementEnemy.GetComponent<NavMeshAgent>());

        myFM.myCamp = GameCamps.Fidele;

        QuestEvents.Instance.EntityRecruited();
    }
}
