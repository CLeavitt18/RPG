using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUi : IUi
{
    [SerializeField] private SlotsActions Focus;

    [SerializeField] private List<SlotsActions> Slots;

    [SerializeField] private Text ItemNameText;
    [SerializeField] private Text detailSlot;

    [SerializeField] private Transform holder;

    [SerializeField] private GameObject DetailsUi;
    [SerializeField] private GameObject detailUiHolder;
    [SerializeField] private GameObject QuestSlot;
    
    [SerializeField] private Item FocusedItem;


    public override void Set()
    {
        List<QuestHolder> Quests = QuestTracker.questTracker.Quests;

        QuestHolder Quest;

        for (int i = 0; i < Quests.Count; i++)
        {
            Quest = Quests[i];

            GameObject NewSlot = Instantiate(QuestSlot, holder);
            SlotsActions slot = NewSlot.GetComponent<SlotsActions>();

            slot.QuestUi = this;
            slot.UI = null;
            slot.transform.GetChild(0).gameObject.GetComponent<Text>().text = Quest.QuestName;

            Slots.Add(slot);
        }

        DetailsUi.SetActive(true);
    }

    public void SetQuestFocused(SlotsActions slot, Item item)
    {
        if (slot == Focus)
        {
            return;
        }

        Focus = slot;
        FocusedItem = item;

        detailUiHolder.SetActive(true);
        //QuestInfoUi.SetActive(true);

        QuestHolder Quest = item as QuestHolder;

        ItemNameText.text = Quest.QuestName;

        StringBuilder sb = new StringBuilder("Time  ");
        sb.Append(Quest.HourAquired);
        sb.Append(": ");

        if (Quest.MintueAquired < 10)
        {
            sb.Append('0');
            sb.Append(Quest.MintueAquired);
        }
        else
        {
            sb.Append(Quest.MintueAquired);
        }

        //QuestInfoUi.transform.GetChild(0).gameObject.GetComponent<Text>().text = sb.ToString();

        sb.Clear();

        sb.Append("Date  ");
        sb.Append(Quest.DateAquired[0].ToString());
        sb.Append(": ");
        sb.Append(Quest.DateAquired[1]);
        sb.Append(": ");
        sb.Append(Quest.DateAquired[2]);

        //QuestInfoUi.transform.GetChild(1).gameObject.GetComponent<Text>().text = sb.ToString();

        sb.Clear();

        sb.Append("Location: ");
        sb.Append(Quest.Location);

        //QuestInfoUi.transform.GetChild(2).gameObject.GetComponent<Text>().text = sb.ToString();

        sb.Clear();

        sb.Append("NPC: ");
        sb.Append(Quest.NPCName);

        //QuestInfoUi.transform.GetChild(3).gameObject.GetComponent<Text>().text = sb.ToString();

        sb.Clear();

        StringBuilder sb2 = new StringBuilder();

        for (int i = 0; i < Quest.CurrentQuestStep; i++)
        {
            sb2.Clear();

            sb2.Append("- ");
            sb2.Append(Quest.Directions[i]);

            sb.Append(sb2.ToString());
            sb.Append("\n");
        }

        //QuestInfoUi.transform.GetChild(4).gameObject.GetComponent<Text>().text = sb.ToString();
    }

    public void SetQuests(int Id)
    {

    }

    public override void Clear()
    {
        int count = holder.childCount;

        if (count == 0)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Destroy(holder.GetChild(i).gameObject);
        }

        Focus = null;
        FocusedItem = null;
    }
}
