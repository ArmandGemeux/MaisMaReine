using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private ParticleSystem myCombatEffect;

    private FideleManager defenseurFideleManager;
    private FideleManager attaquantFideleManager;

    // Start is called before the first frame update
    void Start()
    {
        #region Preprod
        //myCombatEffect = GetComponentInChildren<ParticleSystem>();

        //Déterminer l'attaquant
        //Tester l'état du combat (Spécial ? Normal ?)

        //Si le combat est normal :

        //Soustraire la valeur d'attaque aléatoire de l'attaquant à la valeur de vie totale du défenseur
        //Tester l'état du défenseur (Mort ? Vivant ?)

        //Si le défenseur est mort :
        //Fin du combat, attribution des récompenses, actualisation des quêtes

        //Si le défenseur n'est pas mort :

        //Soustraire la valeur de contre-attaque aléatoire du défenseur à la vie totale de l'attaquant
        //Tester l'état de l'attaquant (Mort ? Vivant ?)

        //Si l'attaquant est mort :
        //Fin du combat, attribution des récompenses, actualisation des quêtes

        //Si l'attaquant n'est pas mort :
        //Fin du combat, attribution des récompenses, actualisation des quêtes


        //Si le combat est spécial :
        //Et que ce combat est spécial par Coup Critique :

        //Soustraire la valeur d'attaque maximale de l'attaquant à la vie totale du défenseur
        //Fin du combat, attribution des récompenses, actualisation des quêtes

        //Et que le combat est spécial par Echec Critique :

        //Soustraire rien à la vie totale du défenseur
        //Soustraire la valeur de contre-attaque maximale du défenseur à la vie totale de l'attaquant
        //Fin du combat, attribution des récompenses, actualisation des quêtes
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartFight()
    {
        Debug.Log("Combat en cours");
        attaquantFideleManager = RaycastInteraction.Instance.interactionLauncherAnim.GetComponent<FideleManager>();
        defenseurFideleManager = GetComponentInParent<FideleManager>();

        defenseurFideleManager.currentHP -= attaquantFideleManager.attackRange;
        Debug.Log("L'attaquant inflige" + attaquantFideleManager.attackRange + "points de dégâts, laissant son adversaire à " + defenseurFideleManager.currentHP);

        attaquantFideleManager.currentHP -= defenseurFideleManager.counterAttackRange;
        Debug.Log("Le défenseur contre-attaque et inflige" + defenseurFideleManager.counterAttackRange + "points de dégâts, laissant son adversaire à " + attaquantFideleManager.currentHP);

        //Determiner les opposants et le type de combat
    }

    public void Attack()
    {
        //Ici, soustraire la valeur d'attaque de l'attaquant au défenseur
    }

    public void CounterAttack()
    {
        //Ici, soustraire la valeur de contre-attaque du défenseur à l'attaquant
    }

    public void CheckHP()
    {
        //Ici, on teste les points de vie des personnages en combat
    }    

    public void CriticalHit()
    {
        //Ici, Attack() et CounterAttack() mais en prenant les effets d'un coup critique en compte
    }

    public void Missed()
    {
        //Ici, Attack() et CounterAttack() mais en prenant les effets d'un echec critique en compte
    }
}
