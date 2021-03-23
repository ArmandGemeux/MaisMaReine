using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class MovementEnemy : MonoBehaviour
{
    private FideleManager myFideleManager;
    private AnimationManager myAnimManager;
    public Collider2D myMoveZoneCollider;

    [SerializeField]
    private Transform myTarget;

    private NavMeshAgent agent;

    private Interaction targetInteraction;
    private Collider2D targetInteractionZone;

    public bool hasMoved = false;
    public bool isMoving = false;
    public bool targetLanded = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.isStopped = true;

        myFideleManager = GetComponentInParent<FideleManager>();
        myAnimManager = GetComponentInParent<AnimationManager>();
        //myMoveZoneCollider = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (myFideleManager.currentCamp.ToString() == GameManager.Instance.currentCampTurn.ToString())
        {
            FindTarget();
        }
        else
        {
            agent.isStopped = true;
        }*/


        //Liste qui défile un par un pour les déplacements FAIT
        //Quand c'est le tour de ce personnage : FAIT
        //Detecter la priorité principale A FAIRE
        //Faire le chemin pour l'atteindre FAIT (obstacles à ajouter)

        //Si le personnage :
        //Quitte sa zone de déplacemenet OU Quitte la MAP OU Atteint sa destination
        //L'arrêter FAIT
    }

    public void MoveToTarget()
    {
        myFideleManager.DetermineMyTarget();
        myTarget = myFideleManager.myTarget.transform;

        targetInteraction = myTarget.GetComponentInChildren<Interaction>();
        targetInteractionZone = targetInteraction.GetComponent<PolygonCollider2D>();

        if (myFideleManager.myCamp == GameManager.Instance.currentCampTurn)
        {
            if (hasMoved == false && targetLanded == false)
            {
                isMoving = true;
                agent.isStopped = false;

                agent.SetDestination(myTarget.position);
            }
            else if (hasMoved == false && targetLanded == true)
            {
                targetInteraction.GetComponent<Combat>().StartFight(myFideleManager);
                myTarget = null;
                myFideleManager.myTarget = null;
            }
            else if (hasMoved == true && targetLanded == true)
            {
                myTarget = null;
                myFideleManager.myTarget = null;
                return;
            }
        }
    }

    public void StopMoving()
    {
        myTarget = null;
        myFideleManager.myTarget = null;

        agent.isStopped = true;
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero;

        isMoving = false;
        hasMoved = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == targetInteractionZone && myFideleManager.myCamp == GameManager.Instance.currentCampTurn)
        {
            StopMoving();
            targetInteraction.GetComponent<Combat>().StartFight(myFideleManager);
            targetLanded = true;
            //Debug.Log("Je suis arrivé");
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == myMoveZoneCollider)
        {
            StopMoving();
            //agent.SetDestination(gameObject.transform.position);
            //Debug.Log("Je suis sorti de ma zone");
        }
        if (collision == targetInteractionZone)
        {
            targetLanded = false;
        }
    }
}
