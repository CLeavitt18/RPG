using UnityEngine;

public class QuestHolder : Item
{
    [Range(1, 24), SerializeField] public int HourAquired;
    [Range(0, 59), SerializeField] public int MintueAquired;

    [SerializeField] public int[] DateAquired;

    [SerializeField] public QuestCompleteType CompletionType;
    [SerializeField] public int MinLevel;
    [SerializeField] public int CurrentQuestStep;
    [SerializeField] public int QuestSteps;
    
    [SerializeField] public int QuestType;

    [SerializeField] private QuestReward[] Rewards;

    [SerializeField] public string NPCName;
    [SerializeField] public string Location;

    //Items or Enemy Targets
    [SerializeField] public QuestItemCompleteConditon[] QuestItems;
    [SerializeField] public QuestEnemyCompleteConditon[] Bosses;

    [SerializeField] public string[] Directions;

    [SerializeField] private bool complete;
    
    public bool RemoveItemFromCheckList(GameObject item)
    {
        return false;
    }

    public bool CheckCompleteCondicution(GameObject questObject)
    {
        bool returnBool = false;

        switch (CompletionType)
        {
            case QuestCompleteType.Item:

                QuestItemHolder item = questObject.GetComponent<QuestItemHolder>();

                if (item != null)
                {
                    for (int i = 0; i < QuestItems.Length; i++)
                    {
                        
                        if (item.Equals(QuestItems[i].item))
                        {
                            QuestItems[i].complete = true;
                            returnBool = true;
                        }
                    }
                }

                break;
            case QuestCompleteType.Boss:

                AIController boss = questObject.GetComponent<AIController>();

                if (boss != null)
                {
                    
                }

                break;
        }

        int itemsComplete = 0;

        for (int i = 0; i < QuestItems.Length; i++)
        {
            if (QuestItems[i].complete)
            {
                itemsComplete++;
            }
        }

        if (itemsComplete == QuestItems.Length)
        {
            complete = true;
        }

        return returnBool;
    }

    public override bool Equals(Item Item)
    {
        if (Item.GetName() == GetName())
        {
            return true;
        }

        return false;
    }

    public QuestReward[] GetReward()
    {
        return Rewards;
    }

    public QuestCompleteType GetCompleteType()
    {
        return CompletionType;
    }

    public bool GetComplete()
    {
        return complete;
    }

    public bool[] GetAllCompletes()
    {
        bool[] completes = new bool[QuestItems.Length];

        for (int i = 0; i < completes.Length; i++)
        {
            completes[i] = QuestItems[i].complete;
        }

        return completes;
    }
    
    public string GetSource()
    {
        return NPCName;
    }

    public string GetLocation()
    {
        return Location;
    }

    public void SetComplete(bool state)
    {
        complete = state;
    }

    public void SetAllCompletes(bool[] completes)
    {
        for (int i = 0; i < completes.Length; i++)
        {
            QuestItems[i].complete = completes[i];
        }
    }

}
