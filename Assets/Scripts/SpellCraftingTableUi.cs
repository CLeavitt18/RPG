using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCraftingTableUi : MonoBehaviour
{
    public static SpellCraftingTableUi table;

    [SerializeField] private SpellHolder spell;

    [SerializeField] private Transform runeContentHolder;

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
        }

        Inventory runes = Player.player.Inventory;

        int numOfRunes = runes.StartIds[3] - runes.StartIds[2];


        for (int i = 0; i < numOfRunes; i++)
        {

        }
    }
}
