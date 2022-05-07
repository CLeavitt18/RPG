using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlotAction : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private bool _clicked;

    [SerializeField] private SpellCraftingTableUi _ui;

    [SerializeField] private Item _item;

    [SerializeField] private float _nextTime = 0.0f;

    [SerializeField] private float _waitTime = 0.1f;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.realtimeSinceStartup >= _nextTime)
        {
            _clicked = false;
        }

        if (_clicked)
        {
            Debug.Log("Slot clicked twice");
            _ui.SetRune(_item);
            _clicked = false;
        }
        else
        {
            _nextTime = Time.realtimeSinceStartup + _waitTime;
            _clicked = true;
        }

        //Debug.Log("Left Mouse Button");
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
