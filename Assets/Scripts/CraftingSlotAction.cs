using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlotAction : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private bool Clicked;

    [SerializeField] private SpellCraftingTableUi _ui;

    [SerializeField] private Item _item;

    [SerializeField] private float NextTime = 0.0f;

    [SerializeField] private float WaitTime = 0.1f;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.realtimeSinceStartup >= NextTime)
        {
            Clicked = false;
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (Clicked)
            {
                //UI.SetFocus(this, 0, item, Player.player.GetMode());
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
                //UI.SetFocus(this, 1, item, Player.player.GetMode());
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
        /*if (Mode == SlotState.Item)
        {
            UI.SetFocus(this, 0, item, Player.player.GetMode());
        }
        else
        {
            QuestUi.SetQuestFocused(this, item);
        }*/
    }

    public void SetSlot(Item item, SpellCraftingTableUi ui)
    {
        _item = item;
        _ui = ui;

        transform.GetChild(0).gameObject.GetComponent<Text>().text = item.Name;
    }
}
