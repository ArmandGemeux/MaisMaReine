using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementZoneDetection : MonoBehaviour
{
    public Collider2D myInteraction;
    public Movement myMovement;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.GetComponent<Interaction>() && collision != myInteraction && myMovement.isMoving)
        {
            Debug.Log(collision.name);
            collision.GetComponentInParent<FideleManager>().DisplayInteraction();
        }
    }
}
