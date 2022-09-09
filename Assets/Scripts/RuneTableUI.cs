using UnityEngine;
using UnityEngine.UI;

public class RuneTableUI : MonoBehaviour
{
    static public RuneTableUI table;

    [SerializeField] private bool canCraft;

    [SerializeField] SpellType spellType;

    [SerializeField] private Transform itemDetailsLocation;
    [SerializeField] private Transform resourceCostDetailsLocation;

    [SerializeField] private Item rune;

    [SerializeField] private DictionaryOfStringAndInt requiredItems;

    [SerializeField] private Dropdown spellTypeDropDown;
    [SerializeField] private Dropdown damageTypeDropDown;
    [SerializeField] private Dropdown castTypeDropDown;
    [SerializeField] private Dropdown targetTypeDropDown;
    [SerializeField] private Dropdown costTypeDropDown;
    [SerializeField] private Dropdown catTypeDropDown;

    [SerializeField] private BaseRecipes recipesCatalyst;
    [SerializeField] private BaseRecipes recipesCostType;
    [SerializeField] private BaseRecipes recipeCastTarget;
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

    public void SetState(bool state)
    {
        transform.GetChild(0).gameObject.SetActive(state);

        spellTypeDropDown.options.Clear();
        damageTypeDropDown.options.Clear();
        castTypeDropDown.options.Clear();
        targetTypeDropDown.options.Clear();
        costTypeDropDown.options.Clear();
        catTypeDropDown.options.Clear();

        if (state)
        {
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

            for (int type = 0; type < 2; type++)
            {
                targetTypeDropDown.options.Add(new Dropdown.OptionData(((CastTarget)type).ToString()));
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
        }
    }

    public void SetSpellType()
    {
        SpellType _type = (SpellType)spellTypeDropDown.value;

        if (spellType == SpellType.GolemSpell && _type != SpellType.GolemSpell)
        {
            targetTypeDropDown.value = (int)CastTarget.Other;
            castTypeDropDown.value = (int)CastType.Channelled;
        }

        spellType = _type;

        if (spellType == SpellType.GolemSpell)
        {
            targetTypeDropDown.value = (int)CastTarget.Self;
            castTypeDropDown.value = (int)CastType.Aura;
        }
    }

    public void SetDamageType()
    {
        int value = catTypeDropDown.value;
        int start = damageTypeDropDown.value * 6;
        int end = start + 5;

        catTypeDropDown.options.Clear();

        for (int i = start; i <= end; i++)
        {
            catTypeDropDown.options.Add(new Dropdown.OptionData(((CatType)i).ToString()));
        }

        catTypeDropDown.value = value;
        catTypeDropDown.transform.GetChild(0).GetComponent<Text>().text = ((CatType)(damageTypeDropDown.value * 6 + value)).ToString();
    }

    public void Preview()
    {
        if (rune != null)
        {
            Destroy(rune.gameObject);
        }

        rune = Roller.roller.CreateRune(
            spellType,
            (AttributesEnum)costTypeDropDown.value,
            (CastType)castTypeDropDown.value,
            (CastTarget)targetTypeDropDown.value,
            damageTypeDropDown.value,
            damageTypeDropDown.value * 6 + catTypeDropDown.value,
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

        temp = recipeCastTarget.ItemsRequired[targetTypeDropDown.value];

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
            (CastTarget)targetTypeDropDown.value,
            damageTypeDropDown.value,
            damageTypeDropDown.value * 6 + catTypeDropDown.value,
            Player.player.GetSkillLevel(SkillType.SpellCrafting));

        Player.player.Inventory.AddItem(rune, true, 1);

        SetState(true);
        Preview();
    }
}
