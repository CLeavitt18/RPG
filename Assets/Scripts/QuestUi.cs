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
    [SerializeField] private GameObject QuestSlot;
    
    [SerializeField] private Item FocusedItem;


    public override void Set()
    {
        if (isActive)
        {
            return;
        }

        isActive = true;

        List<QuestHolder> Quests = QuestTracker.questTracker.Quests;

        QuestHolder Quest;

        for (int i = 0; i < Quests.Count; i++)
        {
            Quest = Quests[i];

            GameObject NewSlot = Instantiate(QuestSlot, holder);
            SlotsActions slot = NewSlot.GetComponent<SlotsActions>();

            slot.SetState(this, Quest);

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

        Helper.helper.CreateQuestDetails(item as QuestHolder, DetailsUi.transform);
    }

    public void SetQuests(int Id)
    {

    }

    public override void Clear()
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;

        int count = holder.childCount;

        for (int i = 0; i < count; i++)
        {
            Destroy(holder.GetChild(i).gameObject);
        }

        Focus = null;
        FocusedItem = null;
    }

    public Item GetFocus()
    {
        return FocusedItem;
    }
}
