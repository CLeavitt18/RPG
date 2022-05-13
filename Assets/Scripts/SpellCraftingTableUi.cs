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
    [SerializeField] private Transform runeItemDetailsLocation;
    [SerializeField] private Transform spellItemDetailsLocation;
    [SerializeField] private Transform costDetailsLocation;

    [SerializeField] private GameObject slot;

    [SerializeField] private Dropdown materialTypeDropDown;
    [SerializeField] private Dropdown slotIdDropDown;

    private void OnEnable()
    {
        if (table != null && table != this)
        {
            Destroy(gameObject);
        }
        else
        {
            table = this;
        }
    }

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

            CreatePlaceHolderSpell();

            Preview();
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
        if (spell != null)
        {
            Destroy(spell.gameObject);
        }

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

        if (runeItemDetailsLocation.childCount != 0)
        {
            Destroy(runeItemDetailsLocation.GetChild(0).gameObject);
        }

        Inventory inventory = Player.player.Inventory;

        int start = inventory.GetStart(GlobalValues.RuneStart);
        int end = inventory.GetStart(GlobalValues.PotionStart);

        for (int i = start; i < end; i++)
        {
            if ((runesFocused[0] != null && inventory[i] == runesFocused[0]) ||
                (runesFocused[1] != null && inventory[i] == runesFocused[1]) ||
                (runesFocused[2] != null && inventory[i] == runesFocused[2])) 
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

        for (int i = 0; i < loops; i++)
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

        Preview();
    }

    public void RemoveRune()
    {
        SetRune(null);
    }

    public void Preview()
    {
        if (spellItemDetailsLocation.childCount != 0)
        {
            Destroy(spellItemDetailsLocation.GetChild(0).gameObject);
        }

        Helper.helper.CreateItemDetails(spell, spellItemDetailsLocation);
    }

    public void PreviewRune(Item item)
    {
        if (runeItemDetailsLocation.childCount != 0)
        {
            Destroy(runeItemDetailsLocation.GetChild(0).gameObject);
        }

        Helper.helper.CreateItemDetails(item, runeItemDetailsLocation);
    }

    public void MaterialTypeChange()
    {
        CreateRuneSlots();

        CreatePlaceHolderSpell();

        Preview();
    }

    public void SetCurrentSepllSlot()
    {
        CurrentSpellSlot = slotIdDropDown.value;
    }

    public Transform GetRuneItemDetailsLocation()
    {
        return runeItemDetailsLocation;
    }
}
