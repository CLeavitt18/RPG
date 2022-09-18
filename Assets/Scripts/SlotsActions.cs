using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotsActions : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private SlotState Mode;

    [SerializeField] private Item _Item;

    [SerializeField] private GameObject EquipedIndicator;

    [SerializeField] private float WaitTime = .25f;

    [SerializeField] public InventoryUi UI;

    [SerializeField] public QuestUi QuestUi;

    [SerializeField] private Text displayNameText;
    [SerializeField] private Text indicatorText;

    [SerializeField] private bool Clicked = false;

    [SerializeField] private float NextTime;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("pointer Click");
        if (Mode == SlotState.Quest || UI.AmountUi.activeSelf == true)
        {
            return;
        }

        if (Time.realtimeSinceStartup >= NextTime)
        {
            Clicked = false;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Clicked)
            {
                UI.SetFocus(this, 0, _Item, Player.player.GetMode());
                Clicked = false;
            }
            else
            {
                NextTime = Time.realtimeSinceStartup + WaitTime;
                Clicked = true;
            }

            //Debug.Log("Left Mouse Button");
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Clicked)
            {
                UI.SetFocus(this, 1, _Item, Player.player.GetMode());
                Clicked = false;
            }
            else
            {
                NextTime = Time.realtimeSinceStartup + WaitTime;
                Clicked = true;
            }

            //Debug.Log("Right Mouse Button");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnMouseEnter Called");
        if (UI.GetFocus() == this || UI.AmountUi.activeSelf == true)
        {
            return;
        }

        if (Mode == SlotState.Item)
        {
            UI.SetFocus(this, 0, _Item, Player.player.GetMode());
        }
        else
        {
            QuestUi.SetQuestFocused(this, _Item);
        }
    }

    public void SetState(IUi ui, Item item)
    {
        if(ui as QuestUi )
        {
            QuestUi = ui as QuestUi;
            UI = null;
            Mode = SlotState.Quest;
        }
        else
        {
            UI = ui as InventoryUi;
            QuestUi = null;
            Mode = SlotState.Item;
        }

        _Item = item;

        displayNameText.text = item.name;
    }

    public void SetIndicator(bool state, string text)
    {
        EquipedIndicator.SetActive(state);
        indicatorText.text = text;
    }

    public Item GetItem()
    {
        return _Item;
    }
}
