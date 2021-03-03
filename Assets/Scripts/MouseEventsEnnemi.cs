using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEventsEnnemi : MonoBehaviour
{
    private FideleManager myFideleManager;
    private AnimationManager myAnimManager;

    private Interaction myInteraction;

    public float interactionClickingTime;
    private float currentInteractionClickingTime;

    // Start is called before the first frame update
    void Start()
    {
        myFideleManager = GetComponent<FideleManager>();
        myAnimManager = GetComponent<AnimationManager>();

        myInteraction = GetComponentInChildren<Interaction>();

        currentInteractionClickingTime = interactionClickingTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {

    }

    public void OnMouseDrag()
    {
        /*#region LaunchInteraction

        if (myInteraction.canInteract)
        {
            currentInteractionClickingTime -= Time.deltaTime;
            myAnimManager.interactionClickFeedback.enabled = true;
            myAnimManager.interactionClickFeedback.fillAmount = currentInteractionClickingTime / interactionClickingTime;

            if (currentInteractionClickingTime <= 0)
            {
                Debug.Log(myInteraction.interactionType);

                myAnimManager.interactionClickFeedback.enabled = false;
                myAnimManager.interactionClickFeedback.fillAmount = currentInteractionClickingTime = interactionClickingTime;

                foreach (Interaction item in collection)
                {

                }
                //myInteraction.alreadyInteractedList.Add(myInteraction);
            }
        }

        #endregion*/
    }

    public void OnMouseUp()
    {

    }
}
