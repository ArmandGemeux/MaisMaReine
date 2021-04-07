﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QuestManager : MonoBehaviour
{
    [Serializable]
    public struct QuestSetup
    {
        public GameObject questParent;
        public TextMeshProUGUI questDesc;
    }

    [Serializable]
    public struct UnitByAmount
    {
        public GameCamps unitCamp;
        public int unitAmout;
    }

    [Serializable]
    public struct QuestObjective
    {
        public string objectiveDesc;

        public List<FideleManager> specificUnitsToRecruit;
        public List<UnitByAmount> unitsToRecruitByAmount;

        public List<FideleManager> specificUnitsToKill;
        public List<UnitByAmount> unitsToKillByAmount;

        public List<FideleManager> specificUnitsToTalkTo;
        public List<UnitByAmount> unitsToTalkToByAmount;
    }

    [Serializable]
    public struct QuestSubObjectives
    {
        public int specificUnitsToRecruitLeft;
        public int unitsToRecruitByAmountLeft;
        public int specificUnitsToKillLeft;
        public int unitsToKillByAmountLeft;
        public int specificUnitsToTalkToLeft;
        public int unitsToTalkToByAmountLeft;
    }

    public List<QuestObjective> mapQuests;

    private List<QuestSubObjectives> questsSubObjectivesLeft = new List<QuestSubObjectives>();

    public List<QuestSetup> questsSetup;

    #region Singleton
    public static QuestManager Instance;

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
        SetupQuestsSubObjectivesLeft();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            SetupQuest(1);
        }
    }

    private void SetupQuestsSubObjectivesLeft()
    {
        foreach (QuestObjective qo in mapQuests)
        {
            QuestSubObjectives qsoTmp = new QuestSubObjectives();
            qsoTmp.specificUnitsToRecruitLeft = qo.specificUnitsToRecruit.Count;
            foreach (UnitByAmount uba in qo.unitsToRecruitByAmount)
            {
                qsoTmp.unitsToRecruitByAmountLeft += uba.unitAmout;
            }

            qsoTmp.specificUnitsToKillLeft = qo.specificUnitsToKill.Count;
            foreach (UnitByAmount uba in qo.unitsToKillByAmount)
            {
                qsoTmp.unitsToKillByAmountLeft += uba.unitAmout;
            }

            qsoTmp.specificUnitsToTalkToLeft = qo.specificUnitsToTalkTo.Count;
            foreach (UnitByAmount uba in qo.unitsToTalkToByAmount)
            {
                qsoTmp.unitsToTalkToByAmountLeft += uba.unitAmout;
            }
            questsSubObjectivesLeft.Add(qsoTmp);
        }
    }

    public void SetupQuest(int questIdx)
    {
        for (int i = 0; i < questsSetup.Count; i++)
        {
            if (questsSetup[i].questDesc.text.Length == 0)
            {
                questsSetup[i].questDesc.text = mapQuests[questIdx].objectiveDesc;
                questsSetup[i].questParent.SetActive(true);
                break;
            }
        }
    }

    public void OnRecruitUnit(FideleManager recruitedUnit)
    {
        for (int i = 0; i < mapQuests.Count; i++)
        {
            QuestSubObjectives qsoTmp = new QuestSubObjectives();
            qsoTmp = questsSubObjectivesLeft[i];
            if (mapQuests[i].specificUnitsToRecruit.Count > 0) //modif ici
            {
                if (mapQuests[i].specificUnitsToRecruit.Contains(recruitedUnit)) //modif ici
                {
                    qsoTmp.specificUnitsToRecruitLeft--; //modif ici
                    questsSubObjectivesLeft[i] = qsoTmp;
                }
            }
            foreach (UnitByAmount uba in mapQuests[i].unitsToRecruitByAmount) //modif ici
            {
                if (uba.unitCamp == recruitedUnit.myCamp)
                {
                    qsoTmp.unitsToRecruitByAmountLeft--; //modif ici
                    questsSubObjectivesLeft[i] = qsoTmp;
                }
            }
            CheckQuestsTasksLeft();
        }
    }

    public void OnKillUnit(FideleManager killedUnit)
    {
        for (int i = 0; i < mapQuests.Count; i++)
        {
            QuestSubObjectives qsoTmp = new QuestSubObjectives();
            qsoTmp = questsSubObjectivesLeft[i];
            if (mapQuests[i].specificUnitsToKill.Count > 0) //modif ici
            {
                if (mapQuests[i].specificUnitsToKill.Contains(killedUnit)) //modif ici
                {
                    qsoTmp.specificUnitsToKillLeft--; //modif ici
                    questsSubObjectivesLeft[i] = qsoTmp;
                }
            }
            foreach (UnitByAmount uba in mapQuests[i].unitsToKillByAmount) //modif ici
            {
                if (uba.unitCamp == killedUnit.myCamp)
                {
                    qsoTmp.unitsToKillByAmountLeft--; //modif ici
                    questsSubObjectivesLeft[i] = qsoTmp;
                }
            }
            CheckQuestsTasksLeft();
        }
    }

    public void OnTalkedUnit(FideleManager talkedUnit)
    {
        for (int i = 0; i < mapQuests.Count; i++)
        {
            QuestSubObjectives qsoTmp = new QuestSubObjectives();
            qsoTmp = questsSubObjectivesLeft[i];
            if (mapQuests[i].specificUnitsToTalkTo.Count > 0) //modif ici
            {
                if (mapQuests[i].specificUnitsToTalkTo.Contains(talkedUnit)) //modif ici
                {
                    qsoTmp.specificUnitsToTalkToLeft--; //modif ici
                    questsSubObjectivesLeft[i] = qsoTmp;
                }
            }
            foreach (UnitByAmount uba in mapQuests[i].unitsToTalkToByAmount) //modif ici
            {
                if (uba.unitCamp == talkedUnit.myCamp)
                {
                    qsoTmp.unitsToTalkToByAmountLeft--; //modif ici
                    questsSubObjectivesLeft[i] = qsoTmp;
                }
            }
            CheckQuestsTasksLeft();
        }
    }

    private void CheckQuestsTasksLeft()
    {
        for (int i = 0; i < questsSubObjectivesLeft.Count; i++)
        {
            QuestSubObjectives qsoTmp = new QuestSubObjectives();
            qsoTmp = questsSubObjectivesLeft[i];
            if (qsoTmp.specificUnitsToKillLeft == 0 && qsoTmp.specificUnitsToRecruitLeft == 0 && qsoTmp.specificUnitsToTalkToLeft == 0 && qsoTmp.unitsToKillByAmountLeft == 0 && qsoTmp.unitsToRecruitByAmountLeft == 0 && qsoTmp.unitsToTalkToByAmountLeft == 0)
            {
                ValidateQuest(i);
            }
        }
    }

    private void ValidateQuest(int qIdx)
    {
        Debug.Log("la quête numéro : " + qIdx);
        for (int i = 0; i < questsSetup.Count; i++)
        {
            if (questsSetup[i].questDesc.text == mapQuests[qIdx].objectiveDesc)
            {
                questsSetup[i].questDesc.text = "Quête terminée";
                //questsSetup[i].questParent.SetActive(false);
            }
        }
    }
}