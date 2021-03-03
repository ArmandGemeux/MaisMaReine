using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{
    public Interaction interactionLauncherInteraction;
    public AnimationManager interactionLauncherAnim;

    #region Singleton

    public static RaycastInteraction Instance;

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
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.currentCampTurn.ToString() == Camp.Fidele.ToString() && Input.GetMouseButtonDown(1))
            LookForInteractionLauncher();
    }

    public void LookForInteractionLauncher()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        if (interactionLauncherAnim == null && interactionLauncherInteraction == null)
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<AnimationManager>() && hit.collider.gameObject.GetComponent<AnimationManager>().isSelectable && hit.collider.gameObject.GetComponent<FideleManager>().currentCamp == Camp.Fidele)
            {
                interactionLauncherAnim = hit.collider.GetComponent<AnimationManager>();
                interactionLauncherInteraction = hit.collider.GetComponentInChildren<Interaction>();

                interactionLauncherAnim.ToggleLauncherOutline();
                interactionLauncherAnim.ActivateLauncherSelection();

                foreach (Interaction myCollideInteraction in interactionLauncherInteraction.myCollideInteractionList)
                {
                    if (!interactionLauncherInteraction.alreadyInteractedList.Contains(myCollideInteraction))
                    {
                        myCollideInteraction.canInteract = true;
                        myCollideInteraction.GetComponentInParent<AnimationManager>().ActivateReceiverSelection();
                    }
                }
            }
        }
        else
        {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<AnimationManager>() && hit.collider.gameObject.GetComponent<AnimationManager>().isSelectable && hit.collider.gameObject.GetComponent<FideleManager>().currentCamp == Camp.Fidele)
            {
                interactionLauncherAnim.ToggleLauncherOutline();
                interactionLauncherAnim.DesactivateLauncherSelection();

                interactionLauncherAnim = hit.collider.GetComponent<AnimationManager>();
                interactionLauncherInteraction = hit.collider.GetComponentInChildren<Interaction>();

                interactionLauncherAnim.ToggleLauncherOutline();
                interactionLauncherAnim.ActivateLauncherSelection();

                foreach (Interaction myCollideInteraction in interactionLauncherInteraction.myCollideInteractionList)
                {
                    myCollideInteraction.canInteract = true;
                    myCollideInteraction.GetComponentInParent<AnimationManager>().ActivateReceiverSelection();
                }
            }
        }
    }
}
