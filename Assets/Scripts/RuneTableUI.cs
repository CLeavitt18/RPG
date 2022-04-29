using UnityEngine;
using UnityEngine.UI;

public class RuneTableUI : MonoBehaviour
{
    static public RuneTableUI table;

    [SerializeField] SpellType type;

    [SerializeField] private Spell rune;

    [SerializeField] private Dropdown spellTypeDropDown;
    [SerializeField] private Dropdown damageTypeDropDown;
    [SerializeField] private Dropdown castTypeDropDown;
    [SerializeField] private Dropdown targetTypeDropDown;
    [SerializeField] private Dropdown costTypeDropDown;
    [SerializeField] private Dropdown catTypeDropDown;

    private void OnEnable()
    {
        table = this;
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

    public void SetType()
    {
        SpellType _type = (SpellType)spellTypeDropDown.value;

        if (type == SpellType.GolemSpell && _type != SpellType.GolemSpell)
        {
            targetTypeDropDown.value = (int)CastTarget.Other;
            castTypeDropDown.value = (int)CastType.Channelled;
        }

        type = _type;

        if (type == SpellType.GolemSpell)
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

        rune = (Roller.roller.runeRoller.CreateRune(
            type, 
            (AttributesEnum)costTypeDropDown.value, 
            (CastType)castTypeDropDown.value, 
            (CastTarget)targetTypeDropDown.value, 
            damageTypeDropDown.value, 
            damageTypeDropDown.value * 6 + catTypeDropDown.value, 
            Player.player.GetSkillLevel((int)SkillType.SpellCrafting))).GetComponent<Spell>();
    }

    public void CreateRune()
    {
        rune = (Roller.roller.runeRoller.CreateRune(
            type,
            (AttributesEnum)costTypeDropDown.value,
            (CastType)castTypeDropDown.value,
            (CastTarget)targetTypeDropDown.value,
            damageTypeDropDown.value,
            damageTypeDropDown.value * 6 + catTypeDropDown.value,
            Player.player.GetSkillLevel((int)SkillType.SpellCrafting))).GetComponent<Spell>();

        Player.player.Inventory.AddItem(rune.GetComponent<Item>(), true, 1);

        SetState(true);
        Preview();
    }
}
