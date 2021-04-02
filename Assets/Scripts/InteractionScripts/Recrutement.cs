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

    public void LaunchRecruitement(FideleManager fmToRecruit)
    {
        RecrutementManager.Instance.OpenRecruitementWindow(fmToRecruit);
    }

    private void SetBanditCampToFidele()
    {
        Destroy(myFM.GetComponent<MouseEventsEnnemi>());
        myFM.gameObject.AddComponent<MouseEventsFidele>();

        myMovementEnemy.gameObject.AddComponent<Movement>();

        Destroy(myMovementEnemy);
        Destroy(myMovementEnemy.GetComponent<NavMeshAgent>());

        myFM.myCamp = GameCamps.Fidele;
        myFM.gameObject.tag = ("Fidele");
        myFM.GetComponent<AnimationManager>().DesactivateReceiverSelection();

        QuestEvents.Instance.EntityRecruited(myFM);
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
        myFM.gameObject.tag = ("Fidele");
        myFM.GetComponent<AnimationManager>().DesactivateReceiverSelection();

        QuestEvents.Instance.EntityRecruited(myFM);
    }
}
