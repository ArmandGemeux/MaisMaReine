using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FideleManager : MonoBehaviour
{
    [Header("Caractéristiques")]
    public string fideleNom;
    public string fidelePrenom;

    public int maxHp;
    private int currentHP;

    [Range(4, 10)]
    public int attackRange;
    [Range(1, 3)]
    public int counteAttackRange;

    [Range(0, 100)]
    public int criticChances;
    [Range(0, 100)]
    public int failChances;

    public int movementZone;

    [Range(0, 100)]
    public int charismaCost;

    public enum FidelePeuple {Humain, Gnome, Golem, Elfe, Animal}
    public FidelePeuple fidelePeuple;

    public enum Camp { Fidele, Roi, Bandit, Calamite}
    public Camp currentCamp;
    
    public bool isSelectable = false;
    public bool isDisplayed = false;


    public Light2D myLight;
    public Canvas myCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayInformations()
    {
        isDisplayed = true;

        if (currentCamp == 0)
        {
            Debug.Log("Changement de lumière");
            myCanvas.enabled = true;
            //myLight.enabled = true;
        }
    }

    public void HideInformations()
    {
        isDisplayed = false;

        if (currentCamp == 0)
        {
            myCanvas.enabled = false;
            //myLight.enabled = false;
        }
    }
}