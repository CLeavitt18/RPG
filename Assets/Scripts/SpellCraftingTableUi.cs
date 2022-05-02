using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCraftingTableUi : MonoBehaviour
{
    public static SpellCraftingTableUi table;

    [SerializeField] private SpellHolder spell;

    [SerializeField] private Text runeSlot;

    [SerializeField] private Transform runeContentHolder;

    [SerializeField] private GameObject slot;

    [SerializeField] private Dropdown materialTypeDropDown;

    public void SetState(bool state)
    {
        transform.GetChild(0).gameObject.SetActive(state);

        materialTypeDropDown.ClearOptions();

        if (state)
        {
            for (int type = 0; type < 11; type++)
            {
                materialTypeDropDown.options.Add(new Dropdown.OptionData(((MaterialType)type).ToString()));
            }

            Inventory inventory = Player.player.Inventory;

            for (int i = inventory.StartIds[2]; i < inventory.StartIds[3]; i++)
            {
                CreateSlot(inventory[i]);
            }
        }

    }

    private void CreateSlot(Item item)
    {
        CraftingSlotAction tempSlot = Instantiate(slot, runeContentHolder).GetComponent<CraftingSlotAction>();

        tempSlot.SetSlot(item);
    }
}
