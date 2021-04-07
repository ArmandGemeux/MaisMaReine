using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Recrutement : MonoBehaviour
{
    private FideleManager myFM;
    private MovementEnemy myMovementEnemy;

    public Sprite myRecruitedSprite;

    // Start is called before the first frame update
    void Start()
    {
        myFM = GetComponentInParent<FideleManager>();
        myMovementEnemy = GetComponentInParent<MovementEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchRecruitement(FideleManager fmToRecruit)
    {
        RecrutementManager.Instance.OpenRecruitementWindow(fmToRecruit, myRecruitedSprite);
    }
}
