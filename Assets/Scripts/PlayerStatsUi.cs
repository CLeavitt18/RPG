using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsUi : IUi
{
    [SerializeField] private GameObject StatsUi;

    [SerializeField] private Transform StatsListHolder;

    [SerializeField] private TextMeshProUGUI StatTextPrefab;
    [SerializeField] private TextMeshProUGUI BannerPrefab;

    [SerializeField] private GameObject SkillTextPrefab;
    [SerializeField] private GameObject InventoryPanel;

    private StringBuilder sb = new StringBuilder();
    private TextMeshProUGUI text;
    private Image bar;

    /*------When button pressed-------
    
    Call TurnItemDetailsOff()
    Clear Inventory holder
    Call StatsUi.SetActive(true)
    Call inventoryUi.SetActive(false)
    Call QuestsUi.SetActive(false)
    */

    public override void Set()
    {
        if (isActive)
        {
            return;
        }

        isActive = true;

        StatsUi.SetActive(true);
        InventoryPanel.SetActive(true);

        //Craete Name Text
        CreateBanner(WorldStateTracker.Tracker.PlayerName, 4);

        //Create Level Text /LevelBar
        sb.Append(GlobalValues.LevelText);
        sb.Append(": ");
        sb.Append(Player.player.GetLevel());

        if (Player.player.GetStoredLevels() >= 1)
        {
            CreateSkillText(1, 1);
        }
        else
        {
            CreateSkillText((double)Player.player.GetLevelProgress(), (double)Player.player.GetRequiredLevelProgress());
        }

        CreateBanner(GlobalValues.SkillsText, 0);

        //Create Skill Texts /Skill Bars
        for (SkillType i = SkillType.Blade; i < SkillType.None; i++)
        {
            string name = i.ToString();
            int id = (int)i;

            for (int x = 0; x < name.Length; x++)
            {
                char c = name[x];

                if (char.IsUpper(c) && x != 0)
                {
                    name = name.Insert(x, " ");
                    x++;
                }
            }

            sb.Append(name);
            sb.Append(": ");
            sb.Append(Player.player.GetSkillLevel(id)); ;

            CreateSkillText((double)Player.player.GetSkillExp(id), (double)Player.player.GetSkillRExp(id));
        }

        CreateBanner(GlobalValues.MasteriesText, 0);

        //Create Mastery Texts /Mastery Bars
        for (MasteryType i = MasteryType.OneHandedMelee; i < MasteryType.None; i++)
        {
            string name = (i).ToString();
            int id = (int)i;

            for (int x = 0; x < name.Length; x++)
            {
                char c = name[x];

                if (char.IsUpper(c) && x != 0)
                {
                    name = name.Insert(x, " ");
                    x++;
                }
            }

            sb.Append(name);
            sb.Append(": ");
            sb.Append(Player.player.GetMasteryLevel(id));

            CreateSkillText((double)Player.player.GetMasteryExp(id), (double)Player.player.GetMasteryRExp(id));
        }

        CreateBanner(GlobalValues.AbilitiesText, 0);

        //Create Ability Texts
        for (int i = 0; i < 3; i++)
        {
            sb.Append(((Abilities)i).ToString());
            sb.Append(": ");
            sb.Append(Player.player.GetAbility(i));

            CreateStatText();
        }

        CreateBanner(GlobalValues.DefensesText, 0);

        sb.Append(GlobalValues.ArmourText);
        sb.Append(": ");
        sb.Append(Player.player.GetArmour());

        CreateStatText();

        //Create Resistence Text
        for (int i = 0; i < 3; i++)
        {
            DisplayResistanceText(i);
        }

        CreateBanner(GlobalValues.OffensesText, 0);

        for (int HandType = 0; HandType < 2; HandType++)
        {
            Item heldItem = Player.player.GetHeldItem(HandType);

            if (heldItem == null)
            {
                continue;
            }

            if (HandType == 0)
            {
                sb.Append(GlobalValues.RightText);
            }
            else
            {
                if (Player.player.GetHeldItem(0) == Player.player.GetHeldItem(1))
                {
                    Destroy(text);
                    continue;
                }

                sb.Append(GlobalValues.LeftText);
            }

            sb.Append(' ');

            CreateBanner(GlobalValues.HandText, 1);

            DisplayHeldItemStats(heldItem, HandType);
        }
    }

    public override void Clear()
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;

        int loops = StatsListHolder.childCount;

        for (int i = 0; i < loops; i++)
        {
            Destroy(StatsListHolder.GetChild(i).gameObject);
        }

        StatsUi.SetActive(false);
    }

    public override void Close()
    {
        StatsUi.SetActive(false);
        InventoryPanel.SetActive(false);
    }

    private void CreateBanner(string title, int type)
    {
        sb.Append(title);

        switch (type)
        {
            case 0:
                sb.Append(GlobalValues.BreakLineExLarge);
                break;
            case 1:
                sb.Append(GlobalValues.BreakLineLarge);
                break;
            case 2:
                sb.Append(GlobalValues.BreakLineMid);
                break;
            case 3:
                sb.Append(GlobalValues.BreakLineSmall);
                break;
            default:
                break;
        }

        CreateBannerText();
    }

    private void CreateStatText()
    {
        text = Instantiate(StatTextPrefab, StatsListHolder);

        text.text = sb.ToString();

        sb.Clear();
    }

    private void CreateBannerText()
    {
        text = Instantiate(BannerPrefab, StatsListHolder);

        text.text = sb.ToString();

        sb.Clear();
    }

    private void CreateSkillText(double curr, double max)
    {
        text = Instantiate(SkillTextPrefab, StatsListHolder).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        bar = text.transform.parent.GetChild(1).GetChild(0).GetComponent<Image>();

        text.text = sb.ToString();
        bar.fillAmount = (float)(curr / max);

        sb.Clear();
    }

    private void DisplayHeldItemStats(Item heldItem, int handType)
    {
        CreateBanner(heldItem.GetName(), 4);

        switch (heldItem.tag)
        {
            case GlobalValues.WeaponTag:

                WeaponHolder weapon = heldItem.GetComponent<WeaponHolder>();

                for (int x = 0; x < weapon.GetDamageRangesCount(); x++)
                {
                    SetDamageStats(weapon, x, weapon.GetDamageType(x));
                }

                WeaponHolder tempWeapon = weapon as WeaponHolder;

                sb.Append(GlobalValues.AttackText);
                sb.Append("s ");
                sb.Append(GlobalValues.PerText);
                sb.Append(' ');
                sb.Append(GlobalValues.SecondText);
                sb.Append(": ");
                sb.Append(tempWeapon.GetAttackSpeed().ToString("0.00"));

                CreateStatText();

                sb.Append(GlobalValues.LifeText);
                sb.Append(' ');
                sb.Append(GlobalValues.StealText);
                sb.Append(": ");
                sb.Append(tempWeapon.GetLifeSteal());
                sb.Append("%");

                CreateStatText();

                sb.Append(GlobalValues.PowerText);
                sb.Append(' ');
                sb.Append(GlobalValues.AttackText);
                sb.Append(' ');
                sb.Append(GlobalValues.CostText);
                sb.Append(": ");

                float cost = heldItem.GetWeight() / 100.0f;
                cost *= 1 + (Mathf.Floor((float)Player.player.GetAbility(AttributesEnum.Health) * 
                            GlobalValues.MDamStrInterval)) * GlobalValues.MDamPerStr;

                sb.Append((int)cost);
                sb.Append(' ');
                sb.Append(AttributesEnum.Stamina.ToString());

                CreateStatText();

                sb.Append(GlobalValues.PowerText);
                sb.Append(' ');
                sb.Append(GlobalValues.AttackText);
                sb.Append(' ');
                sb.Append(GlobalValues.DamageText);
                sb.Append(": ");
                sb.Append(weapon.GetPowerAttack());
                sb.Append("%");

                CreateStatText();

                for (int i = 0; i < weapon.GetDamageRangesCount(); i++)
                {
                    SetStatusStats(weapon, i, weapon.GetDamageType(i));
                }

                float[] multi;

                sb.Append(GlobalValues.MeleeText);
                sb.Append(' ');
                sb.Append(GlobalValues.DamageText);
                sb.Append(' ');

                CreateBanner(GlobalValues.MultipliersText, 2);

                multi = Player.player.GetMeleeMultis(handType);

                for (int x = 0; x < weapon.GetDamageRangesCount(); x++)
                {
                    sb.Append(weapon.GetDamageType(x).ToString());
                    sb.Append(' ');
                    sb.Append(GlobalValues.DamageText);
                    sb.Append(": ");
                    sb.Append(multi[(int)weapon.GetDamageType(x)]);

                    CreateStatText();
                }
                break;
            case GlobalValues.SpellTag:

                SpellHolder spellH = heldItem as SpellHolder;

                Spell rune;

                for (int i = 0; i < 3; i++)
                {
                    rune = spellH.GetRune(i);

                    if (rune == null)
                    {
                        continue;
                    }

                    sb.Append(GlobalValues.SlotText);
                    sb.Append(' ');
                    sb.Append(i.ToString());
                    sb.Append(": ");
                            
                    CreateBanner(rune.GetName(), 2);

                    switch (rune.GetSpellType())
                    {
                        case SpellType.DamageSpell:

                            DamageSpell dRune = rune as DamageSpell;

                            for (int x = 0; x < dRune.GetDamageTypeCount(); x++)
                            {
                                SetDamageStats(dRune, x, dRune.GetDamageType(x));
                            }

                            break;
                        case SpellType.GolemSpell:
                            
                            GolemSpell gRune = rune as GolemSpell;

                            sb.Append(GlobalValues.NumberText);
                            sb.Append(GlobalValues.OfText);
                            sb.Append(GlobalValues.MinionTag);
                            sb.Append("s: ");
                            sb.Append(gRune.Number);

                            CreateStatText();

                            break;
                        default:
                            break;
                    }

                    sb.Append(GlobalValues.ManaText);
                    sb.Append(' ');
                    sb.Append(GlobalValues.CostText);
                    sb.Append(": ");
                    sb.Append(rune.CalculateManaCost(Player.player.GetIntelligence()));

                    CreateStatText();

                    if (rune.GetCastType() != CastType.Aura)
                    {
                        sb.Append(GlobalValues.CastText);
                        sb.Append(' ');
                        sb.Append(GlobalValues.RateText);
                        sb.Append(": ");
                        sb.Append(rune.GetCastRate());

                        CreateStatText();
                    }

                    sb.Append(GlobalValues.CastText);
                    sb.Append(' ');
                    sb.Append(GlobalValues.TypeText);
                    sb.Append(": ");
                    sb.Append(rune.GetCastType().ToString());

                    CreateStatText();

                    switch (rune.GetSpellType())
                    {
                        case SpellType.DamageSpell:

                            DamageSpell dRune = rune as DamageSpell;

                            for (int x = 0; x < dRune.GetDamageTypeCount(); x++)
                            {
                                SetStatusStats(dRune, x, dRune.GetDamageType(x));
                            }

                            break;
                        default:
                            break;
                    }
                }

                break;
            default:
                break;
        }
    }

    private void SetDamageStats(WeaponHolder weapon, int id, DamageTypeEnum type)
    {
        int min = weapon.GetLowerRange(id);
        int max = weapon.GetUpperRange(id);

        DisplayDamageStats(min, max, type);
    }

    private void SetDamageStats(DamageSpell rune, int id, DamageTypeEnum type)
    {
        int min = rune.GetLowerRange(id);
        int max = rune.GetUpperRange(id);

        DisplayDamageStats(min, max, type);
    }

    private void SetStatusStats(WeaponHolder weapon, int id, DamageTypeEnum type)
    {
        int statusChance = weapon.GetStatus(id);

        switch (type)
        {
            case DamageTypeEnum.Physical:
                DisplayPhysStats(statusChance, weapon.GetCrit());
                break;
            case DamageTypeEnum.Fire:
                DisplayFireStats(statusChance);
                break;
            case DamageTypeEnum.Lightning:
                DisplayLightningStats(statusChance);
                break;
            case DamageTypeEnum.Ice:
                DisplayIceStats(statusChance);
                break;
            default:
                break;
        }
    }

    private void SetStatusStats(DamageSpell spell, int id, DamageTypeEnum type)
    {
        int chance = spell.GetStatusChance(id);

        switch (type)
        {
            case DamageTypeEnum.Physical:
                DisplayPhysStats(chance, spell.GetCritDamage());
                break;
            case DamageTypeEnum.Fire:
                DisplayFireStats(chance);
                break;
            case DamageTypeEnum.Lightning:
                DisplayLightningStats(chance);
                break;
            case DamageTypeEnum.Ice:
                DisplayIceStats(chance);
                break;
            default:
                break;
        }

    }

    private void DisplayResistanceText(int id)
    {
        DamageTypeEnum type = (DamageTypeEnum)(id + 1);

        sb.Append(type.ToString());
        sb.Append(' ');
        sb.Append(GlobalValues.ResistanceText);
        sb.Append(": ");
        sb.Append(Player.player.GetResistnce(id));
        sb.Append('%');

        CreateStatText();
    }

    private void DisplayDamageStats(int min, int max, DamageTypeEnum type)
    {
        sb.Append(type.ToString());
        sb.Append(": ");
        sb.Append(min.ToString("n0"));
        sb.Append(GlobalValues.ToText);
        sb.Append(max.ToString("n0"));

        CreateStatText();
    }

    private void DisplayPhysStats(int statusChance, int crit)
    {
        CreateBanner(GlobalValues.CritText, 3);

        sb.Append(GlobalValues.CritText);
        sb.Append(' ');
        sb.Append(GlobalValues.ChanceText);
        sb.Append(": ");
        sb.Append(statusChance);
        sb.Append('%');

        CreateStatText();

        sb.Append(GlobalValues.CritText);
        sb.Append(' ');
        sb.Append(GlobalValues.DamageText);
        sb.Append(": ");
        sb.Append(crit);
        sb.Append('%');

        CreateStatText();
    }

    private void DisplayFireStats(int statsChance)
    {
        CreateBanner(GlobalValues.BurnText, 3);

        sb.Append(GlobalValues.BurnText);
        sb.Append(' ');
        sb.Append(GlobalValues.ChanceText);
        sb.Append(": ");
        sb.Append(statsChance);
        sb.Append("%");

        CreateStatText();

        sb.Append(GlobalValues.BurnText);
        sb.Append(' ');
        sb.Append(GlobalValues.DamageText);
        sb.Append(": ");
        sb.Append(Player.player.GetBurnDamage());
        sb.Append('%');

        CreateStatText();

        sb.Append(GlobalValues.BurnText);
        sb.Append(' ');
        sb.Append(GlobalValues.TicksText);
        sb.Append(": ");
        sb.Append(Player.player.GetTicks());

        CreateStatText();

        sb.Append(GlobalValues.TimeText);
        sb.Append(' ');
        sb.Append(GlobalValues.BetweenText);
        sb.Append(' ');
        sb.Append(GlobalValues.BurnText);
        sb.Append(' ');
        sb.Append(GlobalValues.TicksText);
        sb.Append(": ");
        sb.Append(Mathf.Round(Player.player.GetWaitTime() * 100) * .01f);
        sb.Append("s");

        CreateStatText();
    }

    private void DisplayLightningStats(int statsChance)
    {
        CreateBanner(GlobalValues.ChainText, 3);

        sb.Append(GlobalValues.ChainText);
        sb.Append(' ');
        sb.Append(GlobalValues.ChanceText);
        sb.Append(": ");
        sb.Append(statsChance);
        sb.Append("%");

        CreateStatText();

        sb.Append(GlobalValues.ChainText);
        sb.Append(' ');
        sb.Append(GlobalValues.DamageText);
        sb.Append(": ");
        sb.Append(Player.player.GetChainDamage());
        sb.Append("%");

        CreateStatText();

        sb.Append(GlobalValues.NumberText);
        sb.Append(GlobalValues.OfText);
        sb.Append(GlobalValues.ChainText);
        sb.Append("s: ");
        sb.Append(Player.player.GetChains());

        CreateStatText();

        sb.Append(GlobalValues.ChainText);
        sb.Append(' ');
        sb.Append(GlobalValues.LengthText);
        sb.Append(": ");
        sb.Append(Player.player.GetChainLength());

        CreateStatText();
    }

    private void DisplayIceStats(int statsChance)
    {
        CreateBanner(GlobalValues.ChillText, 3);

        sb.Append(GlobalValues.ChillText);
        sb.Append(' ');
        sb.Append(GlobalValues.ChanceText);
        sb.Append(": ");
        sb.Append(statsChance);
        sb.Append("%");

        CreateStatText();

        sb.Append(GlobalValues.ChillText);
        sb.Append(' ');
        sb.Append(GlobalValues.AffectText);
        sb.Append(": ");
        sb.Append(Player.player.GetChillAffect());
        sb.Append("% ");
        sb.Append(GlobalValues.ReducedText);
        sb.Append(" ");
        sb.Append(GlobalValues.ActionText);
        sb.Append(' ');
        sb.Append(GlobalValues.SpeedText);

        CreateStatText();

        sb.Append(GlobalValues.ChillText);
        sb.Append(' ');
        sb.Append(GlobalValues.DurationText);
        sb.Append(": ");
        sb.Append(Mathf.Round(Player.player.GetChillDuration() * 100) / 100);
        sb.Append("s");

        CreateStatText();
    }
}
