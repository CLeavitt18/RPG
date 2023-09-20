using UnityEngine;
using TMPro;

public class SpellCraftingTableUi : MonoBehaviour
{
    public static SpellCraftingTableUi table;

    [SerializeField] private SpellHolder spell;

    [SerializeField] private int CurrentSpellSlot;

    [SerializeField] private bool canCraft;

    [SerializeField] private Item[] runesFocused;

    [SerializeField] private Transform runeContentHolder;
    [SerializeField] private Transform runeItemDetailsLocation;
    [SerializeField] private Transform spellItemDetailsLocation;
    [SerializeField] private Transform costDetailsLocation;

    [SerializeField] private GameObject slot;
    [SerializeField] private GameObject confirmCreateUi;

    [SerializeField] private TMP_Dropdown materialTypeDropDown;
    [SerializeField] private TMP_Dropdown slotIdDropDown;

    [SerializeField] private BaseRecipes materialRecipes;

    [SerializeField] private DictionaryOfStringAndInt requiredItems = new DictionaryOfStringAndInt(0);

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

    public void SetOpen(bool state)
    {
        transform.GetChild(0).gameObject.SetActive(state);

        materialTypeDropDown.ClearOptions();

        if (state)
        {

            for (int type = 0; type < 11; type++)
            {
                materialTypeDropDown.options.Add(new TMP_Dropdown.OptionData(((MaterialType)type).ToString()));
            }
            
            materialTypeDropDown.value++;
            materialTypeDropDown.value = 0;

            CreateRuneSlots();

            CreatePlaceHolderSpell();

            Preview();
        }
        else
        {
            ClearRuneSlots();

            for (int i = 0; i < 3; i++)
            {
                runesFocused[i] = null;
            }

            Destroy(spell.gameObject);
            Destroy(spellItemDetailsLocation.GetChild(0).gameObject);

            if (runeItemDetailsLocation.transform.childCount != 0)
            {
                Destroy(runeItemDetailsLocation.GetChild(0).gameObject);
            }
            //add logic for cleaing up details ui for spell stats
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

        spell = Roller.roller.CreateSpell(materialTypeDropDown.value, spells, false).GetComponent<SpellHolder>();
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

        Preview();
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
            Destroy(costDetailsLocation.GetChild(0).gameObject);
        }

        Helper.helper.CreateItemDetails(spell, spellItemDetailsLocation);

        DisplayResourceCost();
    }

    private void DisplayResourceCost()
    {
        int matId = materialTypeDropDown.value;

        ItemAmount items = materialRecipes.ItemsRequired[matId];

        requiredItems.Clear();

        for (int i = 0; i < items.Item.Length; i++)
        {
            if (requiredItems.ContainsKey(items.Item[i]))
            {
                requiredItems[items.Item[i]] += items.Amount[i];
            }
            else
            {
                requiredItems.Add(items.Item[i], items.Amount[i]);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (runesFocused[i] == null)
            {
                continue;
            }

            string runeName = runesFocused[i].GetName();

            if (requiredItems.ContainsKey(runeName))
            {
                requiredItems[runeName]++;
            }
            else
            {
                requiredItems.Add(runeName, 1);
            }
        }

        canCraft = Helper.helper.CreateResourceCostDetails(requiredItems, costDetailsLocation);
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

    public void CheckCanCreateSpell()
    {
        bool hasARune = false;

        for (int i = 0; i < 3; i++)
        {
            if (runesFocused[i] != null)
            {
                hasARune = true;
                break;
            }
        }

        if (canCraft && hasARune)
        {
            confirmCreateUi.SetActive(true);
        }
    }

    public void CancelCraft()
    {
        confirmCreateUi.SetActive(false);
    }

    public void CreateSpell()
    {
        Player.player.Inventory.AddItem(spell, true, 1);

        for (int i = 0; i < 3; i++)
        {
            if (runesFocused[i] == null)
            {
                continue;
            }
            
            Player.player.Inventory.RemoveItem(runesFocused[i], 1);
        }

        confirmCreateUi.SetActive(false);
        SetOpen(false);
        SetOpen(true);
    }
}
