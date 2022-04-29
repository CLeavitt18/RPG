using UnityEngine;
using UnityEngine.EventSystems;

public class SlotsActions : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public SlotState Mode;

    public Item item;

    public GameObject EquipedIndicator;

    private float WaitTime = .25f;

    public InventoryUi UI;

    public QuestUi QuestUi;

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
                UI.SetFocus(this, 0, item, Player.player.GetMode());
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
                UI.SetFocus(this, 1, item, Player.player.GetMode());
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
        if (UI.Focus == this || UI.AmountUi.activeSelf == true)
        {
            return;
        }

        if (Mode == SlotState.Item)
        {
            UI.SetFocus(this, 0, item, Player.player.GetMode());
        }
        else
        {
            QuestUi.SetQuestFocused(this, item);
        }
    }
}
