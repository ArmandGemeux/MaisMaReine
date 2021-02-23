using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementZoneDetection : MonoBehaviour
{
    private FideleManager myFideleManager;
    public Movement myMovement;

    public List<FideleManager> myCollidingFideleManager = new List<FideleManager>();

    // Start is called before the first frame update
    void Start()
    {
        myFideleManager = GetComponentInParent<FideleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        /*//Si je touche quelque chose... 
        if (collision != null)
        {
            //ET que ce quelque chose possede un FideleManager ET que ce n'est pas le mien ET que je suis en cours de mouvement...
            if (collision.GetComponent<FideleManager>() && collision != myFideleManager && myMovement.isMoving)
            {
                Debug.Log("ça touche");
                collision.GetComponent<FideleManager>().DisplayInteraction();
            }
        }*/


        /*if (collision != null)
        {
            if (collision.GetComponent<FideleManager>() != null)
            {
                myCollidingFideleManager.Add(collision.GetComponent<FideleManager>());

                for (int i = 0; i < myCollidingFideleManager.Count; i++)
                {
                    if (myCollidingFideleManager[i].currentCamp == Camp.Fidele)
                    {
                        myCollidingFideleManager.Remove(myCollidingFideleManager[i]);
                    }
                    else
                    {
                        myCollidingFideleManager[i].DisplayInteraction();
                    }
                }
            }
        }
    }*/

        /*public void OnTriggerExit2D(Collider2D collision)
        {
            if (collision != null)
            {
                if (collision.GetComponent<FideleManager>() != null && myMovement.isMoving)
                {
                    myCollidingFideleManager.Add(collision.GetComponent<FideleManager>());

                    for (int i = 0; i < myCollidingFideleManager.Count; i++)
                    {
                        if (myCollidingFideleManager[i].currentCamp == Camp.Fidele)
                        {
                            myCollidingFideleManager.Remove(myCollidingFideleManager[i]);
                        }
                        else
                        {
                            myCollidingFideleManager[i].HideInteraction();
                        }
                    }
                }
            }*/
    }
}