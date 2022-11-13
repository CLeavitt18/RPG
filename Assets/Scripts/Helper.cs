using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        Text nameText = t.GetChild(0).GetComponent<Text>();

        nameText.text = item.GetName();

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

            Text name = SpawnItemDetailSlot(t);
            name.alignment = TextAnchor.MiddleCenter;

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
            castRate = gSpell.GetCastRate();

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

    private Text SpawnItemDetailSlot(Transform t, bool useQuestText = false)
    {
        Text _text;

        if (useQuestText)
        {
            _text = Instantiate(questStepTextPrebef, t).GetComponent<Text>();
        }
        else
        {
            _text = Instantiate(textSlot, t).GetComponent<Text>();
        }

        _text.text = sb.ToString();

        sb.Clear();

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

        Item itemRef;

        foreach (KeyValuePair<string, int> item in items)
        {
            sb.Append(item.Key);
            sb.Append(" x ");
            sb.Append(item.Value.ToString("n0"));

            color = Color.red;

            itemRef = pInventory.Find(item.Key, GlobalValues.ResourceTag);

            if (itemRef != null &&
                itemRef.GetAmount() >= item.Value)
            {
                color = Color.black;
                count++;
            }

            SpawnResourceDetailsSlot(t, color);
        }

        if (count == items.Count)
        {
            playercanCraft = true;
        }

        return playercanCraft;
    }

    private Text SpawnResourceDetailsSlot(Transform t, Color color)
    {
        Text _text = Instantiate(resourceSlot, t).GetComponent<Text>();

        _text.text = sb.ToString();
        _text.color = color;

        sb.Clear();

        return _text;
    }

    public void CreateQuestDetails(QuestHolder quest, Transform parent)
    {
        GameObject ui = Instantiate(itemDetailsPrefab, parent.position, parent.rotation, parent);
        Transform t = ui.transform.GetChild(0).GetChild(0);

        Text nameText = t.GetChild(0).GetComponent<Text>();

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
}
