using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private Canvas myDialogueCanvas;

    [TextArea]
    public List<string> myDialogueText;

    // Start is called before the first frame update
    void Start()
    {
        myDialogueCanvas = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        //Mettre tous les scripts d'interaction sur tous les personnages, l'interaction jouée est celle définie dans le script Interaction
        //Dialogue : Liste de String qui incrémente à chaque lancement de dialogue pour avancer. Il n'est pas possible de relire
        //A la fin d'une interaction, pouvoir choisir si l'interaction mets une nouvelle interaction
    }

    public void OpenDialogueWindow(FideleManager thisFM)
    {
        myDialogueCanvas.enabled = true;
    }

    public void HideDialogueFeedback()
    {
        myDialogueCanvas.enabled = false;
    }
}
