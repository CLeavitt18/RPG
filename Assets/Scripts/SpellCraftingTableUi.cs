using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCraftingTableUi : MonoBehaviour
{
    public static SpellCraftingTableUi table;

    [SerializeField] private SpellHolder spell;

    [SerializeField] private int CurrentSpellSlot;

    [SerializeField] private Item[] runesFocused;

    [SerializeField] private Transform runeContentHolder;

    [SerializeField] private GameObject slot;

    [SerializeField] private Dropdown materialTypeDropDown;
    [SerializeField] private Dropdown slotIdDropDown;

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

            CreateRuneSlots();

            //Add logic for default display of spell stats
        }
        else
        {
            materialTypeDropDown.ClearOptions();

            ClearRuneSlots();

            //add logic for cleaing up details ui for spell statss
        }

    }

    private void CreatePlaceHolderSpell()
    {
        Spell[] spells = new Spell[3];

        for (int i = 0; i < 3; i++)
        {
            if (runesFocused[i] == null)
            {
                continue;
            }

            spells[i] = runesFocused[i].GetComponent<Spell>();
        }

        spell = Roller.roller.spellRoller.CreateSpell(materialTypeDropDown.value, spells, false).GetComponent<SpellHolder>();
    }

    private void CreateRuneSlots()
    {
        if (runeContentHolder.childCount != 0)
        {
            ClearRuneSlots();
        }

        Inventory inventory = Player.player.Inventory;

        for (int i = inventory.StartIds[2]; i < inventory.StartIds[3]; i++)
        {
            if ((runesFocused[0] != null && inventory[i] == runesFocused[0]) ||
                (runesFocused[1] != null && inventory[i] == runesFocused[1]) ||
                (runesFocused[2] != null &&inventory[i] == runesFocused[2])) 
            {
                continue;
            }

            CraftingSlotAction tempSlot = Instantiate(slot, runeContentHolder).GetComponent<CraftingSlotAction>();

            tempSlot.SetSlot(inventory[i], this);
        }
    }

    private void ClearRuneSlots()
    {
        int loops = runeContentHolder.childCount;

        for (int i = loops; i > 0; i--)
        {
            Destroy(runeContentHolder.GetChild(i).gameObject);
        }

        runeContentHolder.DetachChildren();
    }

    public void SetRune(Item rune)
    {
        runesFocused[CurrentSpellSlot] = rune;

        CreateRuneSlots();
        CreatePlaceHolderSpell();
    }

    public void SetCurrentSepllSlot()
    {
        CurrentSpellSlot = slotIdDropDown.value;
    }
}
