using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Helper : MonoBehaviour
{
    public static Helper helper;

    [SerializeField] private GameObject itemDetailsPrefab;
    [SerializeField] private GameObject textSlot;

    [SerializeField] private GameObject costDetailsPrefab;
    [SerializeField] private GameObject resourceSlot;

    private void OnEnable()
    {
        if (helper != null && helper != this)
        {
            Destroy(helper);
        }
        else if (helper != this)
        {
            helper = this;
        }
    }

    public void CreateItemDetails(Item item, Transform parent)
    {
        GameObject Ui = Instantiate(itemDetailsPrefab, parent.position, parent.rotation, parent);

        Transform t = Ui.transform.GetChild(0).GetChild(0);
        Text nameText = t.GetChild(0).GetComponent<Text>();

        StringBuilder sb = new StringBuilder();

        nameText.text = item.Name;

        SpawnItemDetailSlot("", t);

        switch (item.tag)
        {
            case GlobalValues.WeaponTag:
                CreateWeaponText(item, t, sb);
                break;
            case GlobalValues.ArmourTag:
                CreateArmourText(item, t, sb);
                break;
            case GlobalValues.SpellTag:
                CreateSpellText(item, t, sb);
                break;
            case GlobalValues.RuneTag:
                CreateRuneText(item, t, sb);
                break;
            case GlobalValues.PotionTag:
                CreatePotionText(item, t, sb);
                break;
            default:
                break;
        }
    }

    private void CreateWeaponText(Item item, Transform t, StringBuilder sb)
    {
        WeaponHolder weapon = item.GetComponent<WeaponHolder>();

        float TempDamage;

        int DPS = 0;

        switch (weapon.Type)
        {
            case WeaponType.Sword:
            case WeaponType.Dagger:
            case WeaponType.GreatSword:

                sb.Append("Blade: ");
                sb.Append(weapon.Materials[0].ToString());

                SpawnItemDetailSlot(sb.ToString(), t);

                sb.Clear();

                sb.Append("Hilt: ");
                sb.Append(weapon.Materials[1].ToString());

                SpawnItemDetailSlot(sb.ToString(), t);

                sb.Clear();

                sb.Append("Grip: ");
                sb.Append(weapon.Materials[2].ToString());

                SpawnItemDetailSlot(sb.ToString(), t);

                sb.Clear();

                break;
            case WeaponType.Axe:

                sb.Append("Blade: ");
                sb.Append(weapon.Materials[0].ToString());

                SpawnItemDetailSlot(sb.ToString(), t);

                sb.Clear();

                sb.Append("Top: ");
                sb.Append(weapon.Materials[1].ToString());

                SpawnItemDetailSlot(sb.ToString(), t);

                sb.Clear();

                sb.Append("Handle: ");
                sb.Append(weapon.Materials[2].ToString());

                SpawnItemDetailSlot(sb.ToString(), t);

                sb.Clear();

                break;
            default:
                break;
        }

        SpawnItemDetailSlot("", t);

        for (int i = 0; i < weapon.DamageRanges.Count; i++)
        {
            sb.Append(weapon.DamageRanges[i].Type.ToString());
            sb.Append(": ");
            sb.Append(weapon.DamageRanges[i].LDamage.ToString("n0"));
            sb.Append(" to ");
            sb.Append(weapon.DamageRanges[i].HDamage.ToString("n0"));

            SpawnItemDetailSlot(sb.ToString(), t);

            TempDamage = (weapon.DamageRanges[i].LDamage + weapon.DamageRanges[i].HDamage) * .5f;
            DPS += (int)TempDamage;

            sb.Clear();
        }

        SpawnItemDetailSlot("", t);

        sb.Append("Damage: ");
        sb.Append(DPS.ToString("n0"));

        SpawnItemDetailSlot(sb.ToString(), t);

        sb.Clear();

        sb.Append("Attack Speed: ");
        sb.Append(weapon.ActionsPerSecond.ToString("0.00"));

        SpawnItemDetailSlot(sb.ToString(), t);

        sb.Clear();

        sb.Append("Attacks Per Second: ");
        sb.Append(weapon.ActionsPerSecond.ToString("0.00"));


        SpawnItemDetailSlot(sb.ToString(), t);
    }

    private void CreateArmourText(Item item, Transform t, StringBuilder sb)
    {
        ArmourHolder armour = item.GetComponent<ArmourHolder>();
    }

    private void CreateSpellText(Item item, Transform t, StringBuilder sb)
    {
        SpellHolder SpellH = item.GetComponent<SpellHolder>();

        int NumOfSpells = SpellH.GetNumOfSpells();

        sb.Append("Material: ");
        sb.Append(SpellH.Type.ToString());

        SpawnItemDetailSlot(sb.ToString(), t);

        sb.Clear();

        SpawnItemDetailSlot("", t);

        for (int i = 0; i < 3; i++)
        {
            sb.Append("Slot ");
            sb.Append((i + 1).ToString("n0"));
            sb.Append(": ");

            if (i < NumOfSpells && SpellH.Spells[i] != null)
            {
                sb.Append(SpellH.Spells[i].SpellType.ToString());
            }
            else
            {
                sb.Append("Empty");
            }

            SpawnItemDetailSlot(sb.ToString(), t);

            sb.Clear();
        }

        for (int i = 0; i < NumOfSpells; i++)
        {
            SpawnItemDetailSlot("", t);

            Text name = SpawnItemDetailSlot(SpellH.Spells[i].Name, t);
            name.alignment = TextAnchor.MiddleCenter;

            SpawnItemDetailSlot("", t);

            CreateRuneStatsText(SpellH.Spells[i], t, sb);
        }
    }

    private void CreateRuneStatsText(Spell rune, Transform t, StringBuilder sb)
    {
        float castRate = rune.CastsPerSecond;
     
        if (rune is DamageSpell dSpell)
        {
            float TempDamage;

            int DPS = 0;

            for (int x = 0; x < dSpell.DamageRanges.Count; x++)
            {
                sb.Append(dSpell.DamageRanges[x].Type.ToString());
                sb.Append(": ");
                sb.Append(dSpell.DamageRanges[x].LDamage);
                sb.Append(" to ");
                sb.Append(dSpell.DamageRanges[x].HDamage);

                TempDamage = (dSpell.DamageRanges[x].LDamage + dSpell.DamageRanges[x].HDamage) * .5f;
                DPS += (int)TempDamage;

                SpawnItemDetailSlot(sb.ToString(), t);

                sb.Clear();
            }

            SpawnItemDetailSlot("", t);

            sb.Append("Damage: ");
            sb.Append(DPS.ToString("n0"));

            SpawnItemDetailSlot(sb.ToString(), t);

            sb.Clear();

        }
        else if (rune is GolemSpell gSpell)
        {
            castRate = gSpell.CastsPerSecond;

            sb.Append("Minions: ");
            sb.Append(gSpell.Number);

            SpawnItemDetailSlot(sb.ToString(), t);

            sb.Clear();
        }

        SpawnItemDetailSlot("", t);

        sb.Append(rune.CostType.ToString());
        sb.Append(" Cost: ");
        sb.Append(rune.Cost.ToString("n0"));

        SpawnItemDetailSlot(sb.ToString(), t);

        sb.Clear();

        if (rune.CastType != CastType.Aura)
        {
            sb.Append("Cast Rate: ");
            sb.Append(castRate.ToString("n0"));

            SpawnItemDetailSlot(sb.ToString(), t);

            sb.Clear();
        }

        sb.Append("Cast Type: ");
        sb.Append(rune.CastType.ToString());

        SpawnItemDetailSlot(sb.ToString(), t);

        sb.Clear();
    }

    private void CreateRuneText(Item item, Transform t, StringBuilder sb)
    {
        CreateRuneStatsText(item.GetComponent<RuneHolder>().spell, t, sb);
    }

    private void CreatePotionText(Item item, Transform t, StringBuilder sb)
    {
        Consumable potion = item.GetComponent<Consumable>();

        if (potion is GainPotion gPotion)
        {
            sb.Append("Gain: ");
            sb.Append(gPotion.LowerRange);
            sb.Append(" to ");
            sb.Append(gPotion.UpperRange);
            sb.Append(' ');
            sb.Append(gPotion.Type.ToString());

            SpawnItemDetailSlot(sb.ToString(), t);
        }
    }

    private Text SpawnItemDetailSlot(string text, Transform t)
    {
        Text _text = Instantiate(textSlot, t).GetComponent<Text>();

        _text.text = text;

        return _text;
    }

    public bool CreateResourceCostDetails(DictionaryOfStringAndInt items, Transform parent)
    {
        bool playercanCraft = false;
        int count = 0;

        GameObject ui = Instantiate(costDetailsPrefab, parent.position, parent.rotation, parent);

        Transform t = ui.transform.GetChild(0).GetChild(0);

        StringBuilder sb = new StringBuilder();

        Color color;

        Inventory pInventory = Player.player.Inventory;

        int start = pInventory.GetStart(GlobalValues.ResourceStart);
        int end = pInventory.GetStart(GlobalValues.MiscStart);

        foreach (KeyValuePair<string, int> item in items)
        {
            sb.Append(item.Key);
            sb.Append(" x ");
            sb.Append(item.Value.ToString("n0"));

            color = Color.red;

            for (int x = start; x < end; x++)
            {
                if (pInventory[x].gameObject.name == item.Key && 
                pInventory[x].GetComponent<ResourceHolder>().Amount >= item.Value)
                {
                    color = Color.black;
                    count++;
                    break;
                }
            }

            SpawnResourceDetailsSlot(sb.ToString(), t, color);

            sb.Clear();
        }

        if (count == items.Count)
        {
            playercanCraft = true;
        }

        return playercanCraft;
    }

    private Text SpawnResourceDetailsSlot(string text, Transform t, Color color)
    {
        Text _text = Instantiate(resourceSlot, t).GetComponent<Text>();

        _text.text = text;
        _text.color = Color.red;

        return _text;
    }
}
