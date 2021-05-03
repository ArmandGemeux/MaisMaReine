using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;
    [TextArea]
    public string[] myDialogue;

    public bool isStartingQuest;
    public int questIndexToStart;

    public GameObject cameraStartPos;
    public GameObject cameraEndPos;

    public List<FideleManager> unitsToSpawn;
}
