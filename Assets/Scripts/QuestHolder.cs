using UnityEngine;

public class QuestHolder : Item
{
    [Range(1, 24)] public int HourAquired;
    [Range(0, 59)] public int MintueAquired;

    public int[] DateAquired;

    public int QuestCompletionType;
    public int QuestType;
    public int MinLevel;
    public int CurrentQuestStep;
    public int QuestSteps;

    [SerializeField] private QuestReward[] Rewards;

    public string QuestName;
    public string NPCName;
    public string Location;

    //Items or Enemy Targets
    public GameObject[] QuestItems;

    public string[] Directions;
    

    public QuestReward[] GetReward()
    {
        return Rewards;
    }
}
