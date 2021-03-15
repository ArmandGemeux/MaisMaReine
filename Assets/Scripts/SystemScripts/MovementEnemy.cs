using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class MovementEnemy : MonoBehaviour
{
    private FideleManager myFideleManager;
    private AnimationManager myAnimManager;

    [SerializeField]
    private Transform currentPriority;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        myFideleManager = GetComponentInParent<FideleManager>();
        myAnimManager = GetComponentInParent<AnimationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myFideleManager.currentCamp.ToString() == GameManager.Instance.currentCampTurn.ToString())
        {
            FindTarget();
        }
        else
        {
            agent.isStopped = true;
        }

        //Liste qui défile un par un pour les déplacements
        //Quand c'est le tour de ce personnage :
        //Detecter la priorité principale
        //Faire le chemin pour l'atteindre

        //Si le personnage :
        //Quitte sa zone de déplacemenet OU Quitte la MAP OU Atteint sa destination
    }

    public void FindTarget()
    {
        agent.isStopped = false;
        currentPriority = GameObject.FindGameObjectWithTag("Fidele").transform;
        agent.SetDestination(currentPriority.position);
    }
}
