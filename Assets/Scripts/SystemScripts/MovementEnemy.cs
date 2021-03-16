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
    private Transform currentPriority;

    private NavMeshAgent agent;

    private Interaction targetInteraction;
    private Collider2D targetInteractionZone;

    public bool hasMoved = false;

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
        Debug.Log(agent.isStopped);

        /*if (myFideleManager.currentCamp.ToString() == GameManager.Instance.currentCampTurn.ToString())
        {
            FindTarget();
        }
        else
        {
            agent.isStopped = true;
        }*/


        //Liste qui défile un par un pour les déplacements
        //Quand c'est le tour de ce personnage :
        //Detecter la priorité principale
        //Faire le chemin pour l'atteindre

        //Si le personnage :
        //Quitte sa zone de déplacemenet OU Quitte la MAP OU Atteint sa destination
    }

    public void FindTarget()
    {
        if (hasMoved == false)
        {
            agent.isStopped = false;
            targetInteraction = currentPriority.GetComponentInChildren<Interaction>();
            targetInteractionZone = targetInteraction.GetComponent<PolygonCollider2D>();

            agent.SetDestination(currentPriority.position);
        }
        else
        {
            agent.isStopped = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == targetInteractionZone)
        {
            agent.isStopped = true;
            transform.parent.position = transform.position;
            transform.localPosition = Vector3.zero;
            hasMoved = true;
            Debug.Log("Je suis arrivé");
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == myMoveZoneCollider)
        {
            agent.isStopped = true;
            transform.parent.position = transform.position;
            transform.localPosition = Vector3.zero;
            hasMoved = true;
            agent.SetDestination(gameObject.transform.position);
            Debug.Log("Je suis sorti de ma zone");
        }
    }
}
