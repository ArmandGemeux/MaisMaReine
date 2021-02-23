using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private ParticleSystem myCombatEffect;

    // Start is called before the first frame update
    void Start()
    {
        myCombatEffect = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayCombatFeedback()
    {
        myCombatEffect.Play();
    }
}
