using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Helper : MonoBehaviour
{
    public static Helper helper;

    [SerializeField] private GameObject itemDetailsPrefab;
    [SerializeField] private GameObject textSlot;
    [SerializeField] private GameObject questStepTextPrebef;

    [SerializeField] private GameObject costDetailsPrefab;
    [SerializeField] private GameObject resourceSlot;

    private StringBuilder sb = new StringBuilder();

    private void OnEnable()
    {
        if (helper != null && helper != this)
        {
            Destroy(helper);
        }
        else
        {
            helper = this;
        }
    }

    public void CreateItemDetails(Item item, Transform parent)
    {
        GameObject Ui = Instantiate(itemDetailsPrefab, parent.position, parent.rotation, parent);
        Transform t = Ui.transform.GetChild(0).GetChild(0);
        t.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = item.GetName();
        t.GetChild(0).GetComponent<RawImage>().color = item.GetRarity();

        SpawnItemDetailSlot(t);

        switch (item.tag)
        {
            case GlobalValues.WeaponTag:
                CreateWeaponText(item, t);
                break;
            case GlobalValues.ArmourTag:
            case GlobalValues.ShieldTag:
                CreateArmourText(item, t);
                break;
            case GlobalValues.SpellTag:
                CreateSpellText(item, t);
                break;
            case GlobalValues.RuneTag:
                CreateRuneText(item, t);
                break;
            case GlobalValues.PotionTag:
                CreatePotionText(item, t);
                break;
            default:
                break;
        }
    }

    private void CreateWeaponText(Item item, Transform t)
    {
        WeaponHolder weapon = item.GetComponent<WeaponHolder>();

        float TempDamage;

        int DPS = 0;

        switch (weapon.GetWeaponType())
        {
            case WeaponType.Sword:
            case WeaponType.Dagger:
            case WeaponType.GreatSword:

                sb.Append("Blade: ");
                sb.Append(weapon.GetMaterialType(0).ToString());

                SpawnItemDetailSlot(t);

                sb.Append("Hilt: ");
                sb.Append(weapon.GetMaterialType(1).ToString());

                SpawnItemDetailSlot(t);

                sb.Append("Grip: ");
                sb.Append(weapon.GetMaterialType(2).ToString());

                SpawnItemDetailSlot(t);
                break;
            case WeaponType.Axe:

                sb.Append("Blade: ");
                sb.Append(weapon.GetMaterialType(0).ToString());

                SpawnItemDetailSlot(t);

                sb.Append("Top: ");
                sb.Append(weapon.GetMaterialType(1).ToString());

                SpawnItemDetailSlot(t);

                sb.Append("Handle: ");
                sb.Append(weapon.GetMaterialType(2).ToString());

                SpawnItemDetailSlot(t);
                break;
            default:
                break;
        }

        SpawnItemDetailSlot(t);

        for (int i = 0; i < weapon.GetDamageRangesCount(); i++)
        {
            sb.Append(weapon.GetDamageType(i).ToString());
            sb.Append(": ");
            sb.Append(weapon.GetLowerRange(i).ToString("n0"));
            sb.Append(" to ");
            sb.Append(weapon.GetUpperRange(i).ToString("n0"));

            SpawnItemDetailSlot(t);

            TempDamage = (weapon.GetLowerRange(i) + weapon.GetUpperRange(i)) * .5f;
            DPS += (int)TempDamage;
        }

        SpawnItemDetailSlot(t);

        sb.Append("Damage: ");
        sb.Append(DPS.ToString("n0"));

        SpawnItemDetailSlot(t);

        sb.Append("Attack Speed: ");
        sb.Append(weapon.GetAttackSpeed().ToString("0.00"));

        SpawnItemDetailSlot(t);
    }

    private void CreateArmourText(Item item, Transform t)
    {
        ArmourHolder armour = item.GetComponent<ArmourHolder>();

        sb.Append("Weight Class: ");

        switch (armour.GetSkillType())
        {
            case SkillType.LightArmour:
                sb.Append("Light");
                break;
            case SkillType.MediumArmour:
                sb.Append("Medium");
                break;
            case SkillType.HeavyArmour:
                sb.Append("Heavy");
                break;
            default:
                break;
        }

        SpawnItemDetailSlot(t);

        SpawnItemDetailSlot(t);

        sb.Append(GlobalValues.ArmourText);
        sb.Append(": ");
        sb.Append(armour.GetArmour());

        SpawnItemDetailSlot(t);

        for (int i = 0; i < armour.GetResistenceCount(); i++)
        {
            sb.Append(armour.GetResistenceType(i).ToString());
            sb.Append(' ');
            sb.Append(GlobalValues.ResistanceText);
            sb.Append(": ");
            sb.Append(armour.GetResistence(i));
            sb.Append('%');

            SpawnItemDetailSlot(t);
        }

    }

    private void CreateSpellText(Item item, Transform t)
    {
        SpellHolder SpellH = item.GetComponent<SpellHolder>();

        int NumOfSpells = SpellH.GetNumOfSpells();

        sb.Append("Material: ");
        sb.Append(SpellH.GetMaterialType().ToString());

        SpawnItemDetailSlot(t);

        SpawnItemDetailSlot(t);

        for (int i = 0; i < 3; i++)
        {
            sb.Append("Slot ");
            sb.Append((i + 1).ToString("n0"));
            sb.Append(": ");

            if (SpellH.GetRune(i) != null)
            {
                sb.Append(SpellH.GetRune(i).GetSpellType().ToString());
            }
            else
            {
                sb.Append("Empty");
            }

            SpawnItemDetailSlot(t);
        }

        for (int i = 0; i < 3; i++)
        {
            if (SpellH.GetRune(i) == null)
            {
                continue;
            }

            SpawnItemDetailSlot(t);

            sb.Append(SpellH.GetRune(i).GetName());

            TextMeshProUGUI name = SpawnItemDetailSlot(t);
            name.alignment = TextAlignmentOptions.MidlineGeoAligned;
            name.fontStyle |= FontStyles.Underline;

            SpawnItemDetailSlot(t);

            CreateRuneStatsText(SpellH.GetRune(i), t);
        }
    }

    private void CreateRuneStatsText(Spell rune, Transform t)
    {
        float castRate = rune.GetCastRate();

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

                SpawnItemDetailSlot(t);
            }

            SpawnItemDetailSlot(t);

            sb.Append("Damage: ");
            sb.Append(DPS.ToString("n0"));

            SpawnItemDetailSlot(t);

        }
        else if (rune is GolemSpell gSpell)
        {
            float TempDamage;

            int DPS = 0;

            sb.Append(gSpell.DamageRange.Type.ToString());
            sb.Append(": ");
            sb.Append(gSpell.DamageRange.LDamage);
            sb.Append(" to ");
            sb.Append(gSpell.DamageRange.HDamage);

            TempDamage = (gSpell.DamageRange.LDamage + gSpell.DamageRange.HDamage) * .5f;
            DPS += (int)TempDamage;

            SpawnItemDetailSlot(t);
            
            SpawnItemDetailSlot(t);

            sb.Append("Damage: ");
            sb.Append(DPS.ToString("n0"));

            SpawnItemDetailSlot(t);

            sb.Append("Minions: ");
            sb.Append(gSpell.Number);

            SpawnItemDetailSlot(t);
        }

        SpawnItemDetailSlot(t);

        sb.Append(rune.GetCostType().ToString());
        sb.Append(" Cost: ");
        sb.Append(rune.GetCost().ToString("n0"));

        SpawnItemDetailSlot(t);

        if (rune.GetCastType() != CastType.Aura)
        {
            sb.Append("Cast Rate: ");
            sb.Append(castRate.ToString("n0"));

            SpawnItemDetailSlot(t);
        }

        sb.Append("Cast Type: ");
        sb.Append(rune.GetCastType().ToString());

        SpawnItemDetailSlot(t);
    }

    private void CreateRuneText(Item item, Transform t)
    {
        CreateRuneStatsText((item as RuneHolder).GetSpell(), t);
    }

    private void CreatePotionText(Item item, Transform t)
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

            SpawnItemDetailSlot(t);
        }
    }

    private TextMeshProUGUI SpawnItemDetailSlot(Transform t, bool useQuestText = false, bool isBold = false)
    {
        TextMeshProUGUI _text;

        if (useQuestText)
        {
            _text = Instantiate(questStepTextPrebef, t).GetComponent<TextMeshProUGUI>();
        }
        else
        {
            _text = Instantiate(textSlot, t).GetComponent<TextMeshProUGUI>();
        }

        _text.text = sb.ToString();

        sb.Clear();

        return _text;
    }

    public bool CreateResourceCostDetails(DictionaryOfStringAndInt items, Transform parent)
    {
        bool playerCanCraft = false;
        int count = 0;

        GameObject ui = Instantiate(costDetailsPrefab, parent.position, parent.rotation, parent);

        Transform t = ui.transform.GetChild(0).GetChild(0);

        Color color;

        Inventory pInventory = Player.player.Inventory;

        Item itemRef;

        foreach (KeyValuePair<string, int> item in items)
        {
            sb.Append(item.Key);
            sb.Append(" x ");
            sb.Append(item.Value.ToString("n0"));

            color = Color.red;

            itemRef = pInventory.Find(item.Key, GlobalValues.ResourceTag);

            int runeString = item.Key.IndexOf("Rune");
            
            if (itemRef != null &&
                itemRef.GetAmount() >= item.Value ||
                runeString != -1)
            {
                color = Color.black;
                count++;
            }

            SpawnResourceDetailsSlot(t, color);
        }

        if (count == items.Count)
        {
            playerCanCraft = true;
        }

        return playerCanCraft;
    }

    private void SpawnResourceDetailsSlot(Transform t, Color color)
    {
        TextMeshProUGUI _text = Instantiate(resourceSlot, t).GetComponent<TextMeshProUGUI>();

        _text.text = sb.ToString();
        _text.color = color;

        sb.Clear();
    }

    public void CreateQuestDetails(QuestHolder quest, Transform parent)
    {
        GameObject ui = Instantiate(itemDetailsPrefab, parent.position, parent.rotation, parent);
        Transform t = ui.transform.GetChild(0).GetChild(0);

        TextMeshProUGUI nameText = t.GetChild(0).GetComponent<TextMeshProUGUI>();

        nameText.text = quest.GetName();

        StringBuilder sb = new StringBuilder("Time  ");
        sb.Append(quest.HourAquired);
        sb.Append(": ");

        if (quest.MintueAquired < 10)
        {
            sb.Append('0');
        }

        sb.Append(quest.MintueAquired);

        SpawnItemDetailSlot(t);

        sb.Append("Date  ");
        sb.Append(quest.DateAquired[0].ToString());
        sb.Append(": ");
        sb.Append(quest.DateAquired[1]);
        sb.Append(": ");
        sb.Append(quest.DateAquired[2]);

        SpawnItemDetailSlot(t);

        sb.Append("Location: ");
        sb.Append(quest.Location);

        SpawnItemDetailSlot(t);

        sb.Append("NPC: ");
        sb.Append(quest.NPCName);

        SpawnItemDetailSlot(t);

        for (int i = 0; i < quest.CurrentQuestStep; i++)
        {
            if (i != quest.CurrentQuestStep - 1)
            {
                sb.Append("\u2713");
            }

            sb.Append("- ");
            sb.Append(quest.Directions[i]);
            sb.Append("\n");
        }

        SpawnItemDetailSlot(t, true);
    }

    public Button CreateCraftingButton(GameObject prefab, string text, Transform parent)
    {
        GameObject intsance = Instantiate(prefab, parent);

        intsance.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;

        Button button = intsance.GetComponent<Button>();

        return button;
    }
}
