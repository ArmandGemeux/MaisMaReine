using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public List<TextMeshProUGUI> myQuestDescription;
    public List<Quest> myQuestsInTerritory = new List<Quest>();

    private int followedQuestAmount = 0;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNewQuestToFollow(Quest qta) //Quest to add
    {

    }

    public void RemoveQuestToFollow(Quest qtr) //Quest to remove
    {
        /*foreach (Quest quest in myQuestsInTerritory)
        {
            if (qtr == quest)
            {
                myQuestsInTerritory.Remove(quest);
            }
        }*/
    }
}
