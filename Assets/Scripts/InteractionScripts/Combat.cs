using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    private ParticleSystem myCombatEffect;

    private FideleManager defenseurFideleManager;
    private Interaction defenseurInteraction;
    private AnimationManager defenseurAM;

    private FideleManager attaquantFideleManager;
    private Interaction attaquantInteraction;
    private AnimationManager attaquantAM;

    private ParticleSystem dealingDamage;
    private ParticleSystem receiveDamage;

    private int isCritical;
    private int isMissed;

    // Start is called before the first frame update
    void Start()
    {
        #region Preprod
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

    public void StartFight(FideleManager atkFM)
    {
        //Debug.Log("Combat en cours");



        defenseurFideleManager = GetComponentInParent<FideleManager>();
        attaquantFideleManager = atkFM;
        dealingDamage = defenseurFideleManager.GetComponentInChildren<ParticleSystem>();
        receiveDamage = GetComponentInChildren<ParticleSystem>();

        //Debug.Log(attaquantFideleManager, defenseurFideleManager);

        isCritical = Random.Range(0, 100);
        if (isCritical <= attaquantFideleManager.criticChances)
        {
            CriticalHit();
        }
        else
        {
            isMissed = Random.Range(0, 100);
            if (isMissed <= attaquantFideleManager.failChances)
            {
                Missed();
            }
            else
            {
                Attack();
            }
        }
        //Determiner les opposants et le type de combat
    }

    public void Attack()
    {
        if (attaquantFideleManager.isAlive && defenseurFideleManager.isAlive)
        {
            //Ici, soustraire la valeur d'attaque de l'attaquant au défenseur
            defenseurFideleManager.currentHP -= attaquantFideleManager.attackRange;
            Debug.Log("L'attaquant inflige" + attaquantFideleManager.attackRange + "points de dégâts, laissant son adversaire à " + defenseurFideleManager.currentHP);
            receiveDamage.Play();
            CheckHP();
            CounterAttack();
        }
    }

    public void CounterAttack()
    {
        if (attaquantFideleManager.isAlive && defenseurFideleManager.isAlive)
        {
            //Ici, soustraire la valeur de contre-attaque du défenseur à l'attaquant
            attaquantFideleManager.currentHP -= defenseurFideleManager.counterAttackRange;
            Debug.Log("Le défenseur contre-attaque et inflige" + defenseurFideleManager.counterAttackRange + "points de dégâts, laissant son adversaire à " + attaquantFideleManager.currentHP);
            dealingDamage.Play();
            CheckHP();
            EndFightNoDead();
        }
    }

    public void CheckHP()
    {
        //Ici, on teste les points de vie des personnages en combat
        if (attaquantFideleManager.currentHP <= 0)
        {
            attaquantFideleManager.isAlive = false;
            Debug.Log("L'attaquant est vaincu !");

            Die(attaquantFideleManager, defenseurFideleManager);
        }
        else if (defenseurFideleManager.currentHP <= 0)
        {
            defenseurFideleManager.isAlive = false;
            Debug.Log("Le défenseur est vaincu !");

            Die(defenseurFideleManager, attaquantFideleManager);
        }
        else
        {
            return;
        }
    }    

    public void CriticalHit()
    {
        //Ici, Attack() et CounterAttack() mais en prenant les effets d'un coup critique en compte
        receiveDamage.Play();
        defenseurFideleManager.currentHP -= attaquantFideleManager.attackRange*2;
        Debug.Log("OUH ! CRITIQUE !!");
        Debug.Log("Avec un coup critique, l'attaquant inflige" + attaquantFideleManager.attackRange + "points de dégâts, laissant son adversaire à " + defenseurFideleManager.currentHP);
        CheckHP();
    }

    public void Missed()
    {
        //Ici, Attack() et CounterAttack() mais en prenant les effets d'un echec critique en compte
        dealingDamage.Play();
        defenseurFideleManager.currentHP -= attaquantFideleManager.attackRange * 2;
        Debug.Log("Loupé !! Aie aie aie !!");
        Debug.Log("Avec un coup critique, l'attaquant inflige" + attaquantFideleManager.attackRange + "points de dégâts, laissant son adversaire à " + defenseurFideleManager.currentHP);
        CheckHP();
    }

    public void Die(FideleManager deadFM, FideleManager winFM)
    {
        winFM.GetComponentInChildren<Interaction>().myCollideAnimationManagerList.Remove(deadFM.GetComponent<AnimationManager>());
        winFM.GetComponentInChildren<Interaction>().myCollideInteractionList.Remove(deadFM.GetComponentInChildren<Interaction>());
        GameManager.Instance.RemoveAMapUnit(deadFM);
        Destroy(deadFM.gameObject);
        EndFightDead();
        //Ici, appliquez les éléments à prendre en compte lorsqu'un combat est fini
        //Debug.Log("Combat terminé");
    }

    public void EndFightNoDead()
    {
        Debug.Log("Combat terminé");
    }

    public void EndFightDead()
    {
        Debug.Log("Combat terminé avec un mort !");
    }
}
