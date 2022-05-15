using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
    public static Roller roller;

    public Catalyst[] Cats;

    [SerializeField] private WeaponRoller weaponRoller;
    [SerializeField] private ArmourRoller armourRoller;
    [SerializeField] private SpellRoller spellRoller;
    [SerializeField] private RuneRoller runeRoller;
    [SerializeField] private PotionRolller potionRolller;
    [SerializeField] private ResourceRoller resourceRoller;
    [SerializeField] private MiscRoller miscRoller;

    private void OnEnable()
    {
        if (roller == null)
        {
            roller = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Item Roll(InventoryState type)
    {
        Item item = null;

        switch (type)
        {
            case InventoryState.Weapons:
                item = weaponRoller.RollWeapon();
                break;
            case InventoryState.Armour:
                item = armourRoller.RollArmour();
                break;
            case InventoryState.Spells:
                item = spellRoller.RollSpell();
                break;
            case InventoryState.Runes:
                item = runeRoller.RollRune();
                break;
            case InventoryState.Potions:
                item = potionRolller.RollPotion();
                break;
            case InventoryState.Resources:
                item = resourceRoller.RollResource();
                break;
            case InventoryState.Misc:
                item = miscRoller.RollMisc();
                break;
            default:
                break;
        }

        return item;
    }

    public Item CreateWeapon(WeaponType type, int pri_Id, int sec_Id, int ter_Id, int cat_Id, int level = 0)
    {
        return weaponRoller.CreateWeapon(type, pri_Id, sec_Id, ter_Id, cat_Id, level);
    }

    public Item CreateSpell(int mat_id, Spell[] runes, bool cleanUp = true)
    {
        return spellRoller.CreateSpell(mat_id, runes, cleanUp);
    }

    public Item CreateRune
    (
        SpellType spellType,
        AttributesEnum costType,
        CastType castType,
        CastTarget castTareget,
        int damageType,
        int cat_id,
        int level
    )
    {
        return runeRoller.CreateRune(spellType, costType, castType, castTareget, damageType, cat_id, level);
    }
}
