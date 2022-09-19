using System.Collections.Generic;
using UnityEngine;

public class QuestTracker : MonoBehaviour
{
    public static QuestTracker questTracker;

    public int[] StartIds = new int[2];

    public List<QuestHolder> Quests = new List<QuestHolder>();

    public void OnEnable()
    {
        questTracker = this;
    }

    public void AddQuestOnLoad(QuestHolder NewQuest)
    {
        if (NewQuest.QuestType == 0)
        {
            Quests.Insert(StartIds[0], NewQuest);
            StartIds[0]++;
            StartIds[1]++;
        }

        if (NewQuest.QuestType == 1)
        {
            Quests.Insert(StartIds[1], NewQuest);
            StartIds[1]++;
        }

        if (NewQuest.QuestType == 2)
        {
            Quests.Add(NewQuest);
        }

        NewQuest.transform.parent = transform;
    }

    public void AddQuest(QuestHolder NewQuest)
    {
        WorldStateTracker.Tracker.UpdateTracker();

        NewQuest.HourAquired = (int)WorldStateTracker.Tracker.TimeOfDay;

        int temp = (int)(WorldStateTracker.Tracker.TimeOfDay * 100);
        temp -= ((int)WorldStateTracker.Tracker.TimeOfDay) * 100;

        NewQuest.MintueAquired = (int)((float)temp / (float)100 * 60);

        NewQuest.DateAquired[0] = WorldStateTracker.Tracker.Day;
        NewQuest.DateAquired[1] = WorldStateTracker.Tracker.Month;
        NewQuest.DateAquired[2] = WorldStateTracker.Tracker.Year;

        NewQuest.NPCName = Player.player.GetHit().GetComponent<AIController>().GetName();

        if (SceneManagerOwn.Manager != null)
        {
            NewQuest.Location = SceneManagerOwn.Manager.SceneName;
        }
        else
        {
            NewQuest.Location = "Test";
        }

        if (NewQuest.QuestType == 0)
        {
            Quests.Insert(StartIds[0], NewQuest);
            StartIds[0]++;
            StartIds[1]++;
        }

        if (NewQuest.QuestType == 1)
        {
            Quests.Insert(StartIds[1], NewQuest);
            StartIds[1]++;
        }

        if (NewQuest.QuestType == 2)
        {
            Quests.Add(NewQuest);
        }

        NewQuest.transform.parent = transform;
    }

    public void RemoveQuest(QuestHolder Quest)
    {
        int Index = 0;

        for (int i = 0; i < Quests.Count; i++)
        {
            if (Quest.QuestName == Quests[i].QuestName)
            {
                Index = i;
                break;
            }
        }

        int QuestTypeID = Quest.QuestType;

        if (QuestTypeID == 0)
        {
            Quests.RemoveAt(Index);
            StartIds[0]--;
            StartIds[1]--;
        }

        if (QuestTypeID == 1)
        {
            Quests.RemoveAt(Index);
            StartIds[1]--;
        }

        if (QuestTypeID == 2)
        {
            Quests.RemoveAt(Index);
        }
    }

    public void UpdateQuest(GameObject Item)
    {
        for (int i = 0; i < Quests.Count; i++)
        {
            if (Quests[i].CheckCompleteCondicution(Item))
            {
                Quests[i].CurrentQuestStep++;
                return;
            }
        }
    }

    public void UpdateQuest(int id)
    {

    }
}
