using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneTableUI : MonoBehaviour
{
    static public RuneTableUI table;

    [SerializeField] private bool canCraft;
    [SerializeField] private bool canPreview;

    [SerializeField] SpellType spellType;
    [SerializeField] DamageTypeEnum damageType;
    [SerializeField] CastType castType;
    [SerializeField] AttributesEnum costType;
    [SerializeField] int catType;

    [SerializeField] private Transform itemDetailsLocation;
    [SerializeField] private Transform resourceCostDetailsLocation;

    [SerializeField] private Item rune;

    [SerializeField] private DictionaryOfStringAndInt requiredItems = new DictionaryOfStringAndInt(10);

    [SerializeField] private Dropdown spellTypeDropDown;
    [SerializeField] private Dropdown damageTypeDropDown;
    [SerializeField] private Dropdown castTypeDropDown;
    [SerializeField] private Dropdown costTypeDropDown;
    [SerializeField] private Dropdown catTypeDropDown;

    [SerializeField] private BaseRecipes recipesCatalyst;
    [SerializeField] private BaseRecipes recipesCostType;
    [SerializeField] private BaseRecipes[] recipesSpellType;
    [SerializeField] private BaseRecipes[] recipesCastType;


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

    private void Start()
    {
        canPreview = false;

        spellTypeDropDown.options.Clear();
        damageTypeDropDown.options.Clear();
        castTypeDropDown.options.Clear();
        costTypeDropDown.options.Clear();
        catTypeDropDown.options.Clear();

        for (int type = 0; type < 2; type++)
        {
            spellTypeDropDown.options.Add(new Dropdown.OptionData(((SpellType)type).ToString()));
        }

        for (int type = 0; type < 4; type++)
        {
            damageTypeDropDown.options.Add(new Dropdown.OptionData(((DamageTypeEnum)type).ToString()));
        }

        for (int type = 0; type < 5; type++)
        {
            castTypeDropDown.options.Add(new Dropdown.OptionData(((CastType)type).ToString()));
        }

        for (int type = 0; type < 3; type++)
        {
            costTypeDropDown.options.Add(new Dropdown.OptionData(((AttributesEnum)type).ToString()));
        }

        costTypeDropDown.value = 2;

        for (int type = 0; type <= (int)CatType.T6Phys; type++)
        {
            catTypeDropDown.options.Add(new Dropdown.OptionData(((CatType)type).ToString()));
        }

        canPreview = true;
    }

    public void SetState(bool state)
    {
        transform.GetChild(0).gameObject.SetActive(state);

        if (state)
        {
            canPreview = false;
            
            spellTypeDropDown.value = 0;
            damageTypeDropDown.value = 0;
            castTypeDropDown.value = 0;
            costTypeDropDown.value = 2;
            catTypeDropDown.value = 0;
            
            canPreview = true;

            Preview();
        }
    }

    public void SetSpellType()
    {
        bool previewTemp = canPreview;

        SpellType _type = (SpellType)spellTypeDropDown.value;

        canPreview = false;

        if (spellType == SpellType.GolemSpell && _type != SpellType.GolemSpell)
        {
            castTypeDropDown.value = (int)CastType.Channelled;
        }

        spellType = _type;

        if (spellType == SpellType.GolemSpell)
        {
            castTypeDropDown.value = (int)CastType.Aura;
        }

        canPreview = previewTemp;

        if (canPreview)
        {
            Preview();
        }
    }

    public void SetDamageType()
    {
        bool previewTemp = canPreview;

        damageType = (DamageTypeEnum)damageTypeDropDown.value;

        int value = catTypeDropDown.value;
        int start = damageTypeDropDown.value * 6;
        int end = start + 5;

        catTypeDropDown.options.Clear();

        for (int i = start; i <= end; i++)
        {
            catTypeDropDown.options.Add(new Dropdown.OptionData(((CatType)i).ToString()));
        }

        canPreview = false;

        catTypeDropDown.value = value;
        
        canPreview = previewTemp;

        if (canPreview)
        {
            Preview();
        }
    }

    public void SetCastType()
    {
        castType = (CastType)castTypeDropDown.value;

        if (canPreview)
        {
            Preview();
        }
    }

    public void SetCostType()
    {
        costType = (AttributesEnum)costTypeDropDown.value;

        if (canPreview)
        {
            Preview();
        }
    }

    public void SetCatType()
    {
        catType = catTypeDropDown.value;

        if (canPreview)
        {
            Preview();
        }
    }

    public void Preview()
    {
        if (rune != null)
        {
            Destroy(rune.gameObject);
        }

        if (spellType == SpellType.GolemSpell)
        {
            canPreview = false;
            castTypeDropDown.value = (int)CastType.Aura;
            canPreview = true;
        }

        rune = Roller.roller.CreateRune(
            spellType,
            costType,
            castType,
            damageTypeDropDown.value,
            damageTypeDropDown.value * 6 + catType,
            Player.player.GetSkillLevel(SkillType.SpellCrafting));

        Helper.helper.CreateItemDetails(rune, itemDetailsLocation);
        DisplayResourceCost();
    }

    private void DisplayResourceCost()
    {
        if (resourceCostDetailsLocation.childCount != 0)
        {
            Destroy(resourceCostDetailsLocation.GetChild(0).gameObject);
        }

        if (requiredItems.Count != 0)
        {
            requiredItems.Clear();
        }

        ItemAmount temp = recipesSpellType[(int)spellType].ItemsRequired[damageTypeDropDown.value];

        for (int i = 0; i < temp.Item.Length; i++)
        {
            if (requiredItems.ContainsKey(temp.Item[i]))
            {
                requiredItems[temp.Item[i]] += temp.Amount[i];
            }
            else
            {
                requiredItems.Add(temp.Item[i], temp.Amount[i]);
            }
        }

        temp = recipesCastType[castTypeDropDown.value].ItemsRequired[damageTypeDropDown.value];

        for (int i = 0; i < temp.Item.Length; i++)
        {
            if (requiredItems.ContainsKey(temp.Item[i]))
            {
                requiredItems[temp.Item[i]] += temp.Amount[i];
            }
            else
            {
                requiredItems.Add(temp.Item[i], temp.Amount[i]);
            }
        }

        temp = recipesCostType.ItemsRequired[costTypeDropDown.value];

        for (int i = 0; i < temp.Item.Length; i++)
        {
            if (requiredItems.ContainsKey(temp.Item[i]))
            {
                requiredItems[temp.Item[i]] += temp.Amount[i];
            }
            else
            {
                requiredItems.Add(temp.Item[i], temp.Amount[i]);
            }
        }

        temp = recipesCatalyst.ItemsRequired[damageTypeDropDown.value * 6 + catTypeDropDown.value];

        for (int i = 0; i < temp.Item.Length; i++)
        {
            if (requiredItems.ContainsKey(temp.Item[i]))
            {
                requiredItems[temp.Item[i]] += temp.Amount[i];
            }
            else
            {
                requiredItems.Add(temp.Item[i], temp.Amount[i]);
            }

            if (spellType == SpellType.GolemSpell)
            {
                requiredItems[temp.Item[i]]++;
            }
        }

        canCraft = Helper.helper.CreateResourceCostDetails(requiredItems, resourceCostDetailsLocation);
    }

    public void CreateRune()
    {
        rune = Roller.roller.CreateRune(
            spellType,
            (AttributesEnum)costTypeDropDown.value,
            (CastType)castTypeDropDown.value,
            damageTypeDropDown.value,
            damageTypeDropDown.value * 6 + catTypeDropDown.value,
            Player.player.GetSkillLevel(SkillType.SpellCrafting));

        Inventory pInventory = Player.player.Inventory;

        pInventory.AddItem(rune, true, 1);

        foreach (KeyValuePair<string, int> item in requiredItems)
        {
            pInventory.RemoveItem(item.Key, item.Value, GlobalValues.ResourceTag);
        }

        SetState(true);
        Preview();
    }
}
