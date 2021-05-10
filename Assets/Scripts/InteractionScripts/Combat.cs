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
        defenseurFideleManager = GetComponentInParent<FideleManager>();
        defenseurAM = GetComponentInParent<AnimationManager>();

        DragCamera2D.Instance.FollowTargetCamera(defenseurAM.gameObject);

        attaquantFideleManager = atkFM;
        attaquantAM = attaquantFideleManager.GetComponent<AnimationManager>();

        dealingDamage = defenseurFideleManager.GetComponentInChildren<ParticleSystem>();
        receiveDamage = GetComponentInChildren<ParticleSystem>();

        isCritical = Random.Range(0, 100);
        if (isCritical <= attaquantFideleManager.criticChances)
        {
            StartCoroutine(CriticalHit());
        }
        else
        {
            isMissed = Random.Range(0, 100);
            if (isMissed <= attaquantFideleManager.missChances)
            {
                StartCoroutine(Missed());
            }
            else
            {
                StartCoroutine(Defend());
            }
        }
    }

    public IEnumerator Defend()
    {
        if (attaquantFideleManager.isAlive && defenseurFideleManager.isAlive)
        {
            int attackValue = Random.Range(attaquantFideleManager.minAttackRange, attaquantFideleManager.maxAttackRange);
            defenseurFideleManager.currentHP -= attackValue;
            Debug.Log(attaquantFideleManager.name +  " inflige " + attackValue + " points de dégâts, laissant " + defenseurFideleManager.name + " à " + defenseurFideleManager.currentHP);

            // ICI jouer VFX d'attaque simple
            // ICI jouer SFX d'attaque simple

            receiveDamage.Play();

            yield return new WaitForSeconds(0.2f);

            // Ici jouer Anim dégâts reçus sur defenseur
            defenseurAM.ReceiveDamage();
            defenseurAM.FillAmountHealth();

            yield return new WaitForSeconds(0.5f);

            DragCamera2D.Instance.UnfollowTargetCamera();



            CheckHP();
            StartCoroutine(CounterAttack());
        }
    }

    public IEnumerator CounterAttack()
    {
        if (attaquantFideleManager.isAlive && defenseurFideleManager.isAlive)
        {
            int counterAttackValue = Random.Range(defenseurFideleManager.minCounterAttackRange, defenseurFideleManager.maxCounterAttackRange);
            attaquantFideleManager.currentHP -= counterAttackValue;
            Debug.Log("Le défenseur contre-attaque et inflige" + counterAttackValue + "points de dégâts, laissant son adversaire à " + attaquantFideleManager.currentHP);


            // ICI jouer VFX de contre-attaque simple
            // ICI jouer SFX de contre-attaque simple
            dealingDamage.Play();

            yield return new WaitForSeconds(0.2f);

            // Ici jouer Anim dégâts reçus sur attaquant
            attaquantAM.ReceiveDamage();
            attaquantAM.FillAmountHealth();

            yield return new WaitForSeconds(0.5f);

            DragCamera2D.Instance.UnfollowTargetCamera();


            CheckHP();
            EndFightNoDead();
        }
    }

    public void CheckHP()
    {
        if (attaquantFideleManager.currentHP <= 0)
        {
            attaquantFideleManager.isAlive = false;
            Debug.Log("L'attaquant est vaincu !");

            StartCoroutine(Die(attaquantFideleManager, defenseurFideleManager));
        }
        else if (defenseurFideleManager.currentHP <= 0)
        {
            defenseurFideleManager.isAlive = false;
            Debug.Log("Le défenseur est vaincu !");

            StartCoroutine(Die(defenseurFideleManager, attaquantFideleManager));
        }
    }    

    public IEnumerator CriticalHit()
    {
        defenseurFideleManager.currentHP -= attaquantFideleManager.maxAttackRange*2;
        Debug.Log("OUH ! CRITIQUE !!");
        Debug.Log("Avec un coup critique, " + attaquantFideleManager.name + " inflige " + attaquantFideleManager.maxAttackRange*2 + " points de dégâts, laissant " + defenseurFideleManager.name + " à " + defenseurFideleManager.currentHP);


        // ICI jouer VFX de coup critiique
        // ICI jouer SFX de coup critique

        receiveDamage.Play();

        yield return new WaitForSeconds(0.2f);

        // ICI jouer Anim dégâts reçus sur defenseur
        defenseurAM.ReceiveDamage();
        defenseurAM.FillAmountHealth();

        yield return new WaitForSeconds(0.5f);

        DragCamera2D.Instance.UnfollowTargetCamera();

        CheckHP();
    }

    public IEnumerator Missed()
    {
        attaquantFideleManager.currentHP -= defenseurFideleManager.maxCounterAttackRange;
        Debug.Log("Loupé !! Aie aie aie !!");
        Debug.Log("Avec un l'échec critique de l'attaquant, le défenseur contre attaque et inflige " + defenseurFideleManager.maxCounterAttackRange + "points de dégâts, laissant son adversaire à " + attaquantFideleManager.currentHP);


        // ICI jouer VFX d'echec critiique
        // ICI jouer SFX d'echec critique

        dealingDamage.Play();

        yield return new WaitForSeconds(0.2f);

        // ICI jouer Anim dégâts reçus sur attaquant
        attaquantAM.ReceiveDamage();
        attaquantAM.FillAmountHealth();

        yield return new WaitForSeconds(0.5f);

        DragCamera2D.Instance.UnfollowTargetCamera();

        CheckHP();
    }

    public IEnumerator Die(FideleManager deadFM, FideleManager winFM)
    {
        Debug.Log("On Tue quelqu'un");
        DragCamera2D.Instance.UnfollowTargetCamera();

        QuestManager.Instance.OnKillUnit(deadFM);

        if (deadFM.myCamp != GameCamps.Bandit)
        {
            deadFM.GetComponent<AnimationManager>().Dying();
            // ICI jouer Anim de mort
            // ICI jouer SFX de mort

            yield return new WaitForSeconds(1f);


            if (deadFM.myCamp == GameCamps.Fidele)
            {
                GameManager.Instance.AddCharismeValue(-5);
            }

            foreach (AnimationManager dfmcam in deadFM.GetComponentInChildren<Interaction>().myCollideAnimationManagerList)
            {
                dfmcam.GetComponentInChildren<Interaction>().RemoveCollidingCharacterFromAMList(deadFM.GetComponent<AnimationManager>());
            }

            foreach (Interaction dfmci in deadFM.GetComponentInChildren<Interaction>().myCollideInteractionList)
            {
                dfmci.RemoveCollindingCharacterFromInteractionList(deadFM.GetComponentInChildren<Interaction>());
            }

            foreach (FideleManager dfmcfm in deadFM.unitsInRange)
            {
                dfmcfm.unitsInRange.Remove(deadFM);
            }

            //winFM.GetComponentInChildren<Interaction>().myCollideAnimationManagerList.Remove(deadFM.GetComponent<AnimationManager>());
            //winFM.GetComponentInChildren<Interaction>().myCollideInteractionList.Remove(deadFM.GetComponentInChildren<Interaction>());

            GameManager.Instance.RemoveAMapUnit(deadFM);
            winFM.KillUnit(deadFM);
            Destroy(deadFM.gameObject);
        }
        else if (deadFM.myCamp == GameCamps.Bandit && winFM.myCamp == GameCamps.Fidele)
        {
            SwitchInteractionType(deadFM);
        }
    }

    public void EndFightNoDead()
    {
        Debug.Log("Combat terminé");
    }

    public void SwitchInteractionType(FideleManager deadFM)
    {
        // ICI jouer SFX de mort
        // ICI jouer anim du Bandit qui change de camp
        // ICI changer graphisme du bandit qui change de camp
        GetComponent<Interaction>().interactionType = InteractionType.Recrutement;
        
        deadFM.isAlive = false;
    }
}
