using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlotAction : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private bool _clicked;

    [SerializeField] private SpellCraftingTableUi _ui;

    [SerializeField] private Item _item;

    [SerializeField] private float _nextTime = 0.0f;

    [SerializeField] private float _waitTime = 0.25f;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.realtimeSinceStartup >= _nextTime)
        {
            _clicked = false;
        }

        if (_clicked)
        {
            //Debug.Log("Slot clicked twice");
            _ui.SetRune(_item);
            _clicked = false;
        }
        else
        {
            _nextTime = Time.realtimeSinceStartup + _waitTime;
            _clicked = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ui.PreviewRune(_item);
    }

    public void SetSlot(Item item, SpellCraftingTableUi ui)
    {
        _item = item;
        _ui = ui;

        transform.GetChild(0).gameObject.GetComponent<Text>().text = item.Name;
        GetComponent<Image>().color = item.Rarity;
    }
}
