using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private Canvas myDialogueCanvas;
    // Start is called before the first frame update
    void Start()
    {
        myDialogueCanvas = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayDialogueFeedback()
    {
        myDialogueCanvas.enabled = true;
    }

    public void HideDialogueFeedback()
    {
        myDialogueCanvas.enabled = false;
    }
}
