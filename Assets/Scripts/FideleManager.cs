using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public enum Camp { Fidele, Roi, Bandit, Calamite }

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

    [Range(0, 100)]
    public int charismaCost;

    public enum FidelePeuple {Humain, Gnome, Golem, Elfe, Animal}
    public FidelePeuple fidelePeuple;
    
    public Camp currentCamp;
}