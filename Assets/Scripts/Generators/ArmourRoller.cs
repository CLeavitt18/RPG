using System.Text;
using UnityEngine;

public class ArmourRoller : MonoBehaviour
{
    [SerializeField] private BaseArmour[] Armour;

    [SerializeField] private MatMults[] Multis;

    [SerializeField] private DropTable[] TypeTable;
    [SerializeField] private DropTable[] ClassTable;
    [SerializeField] private DropTable[] MatTable;
    [SerializeField] private DropTable[] CatTable;

    [SerializeField] private char[] ArmourClasses;

    public Item RollArmour()
    {
        int Mat_Id = 0;
        int Cat_Id = 0;

        SkillType skill = 0;

        ArmourType State = 0;

        int Chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        for (int i = 0; i < MatTable[0].Chances.Length; i++)
        {
            if (Chance <= MatTable[0].Chances[i])
            {
                Mat_Id = i;
                break;
            }
        }

        Chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        for (int i = 0; i < TypeTable[0].Chances.Length; i++)
        {
            if (Chance <= TypeTable[0].Chances[i])
            {
                State = (ArmourType)i;
                break;
            }
        }

        Chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        for (int i = 0; i < ClassTable[0].Chances.Length; i++)
        {
            if (Chance <= ClassTable[0].Chances.Length)
            {
                skill = (SkillType)i;
                break;
            }
        }

        Chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        if (Chance <= 748)
        {
            //Physical
            Chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

            for (int i = 0; i < CatTable[0].Chances.Length; i++)
            {
                if (Chance <= CatTable[0].Chances[i])
                {
                    Cat_Id = i;
                    break;
                }
            }
        }
        else if (Chance >= 749 && Chance <= 832)
        {
            //Fire
            Chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

            for (int i = 0; i < CatTable[0].Chances.Length; i++)
            {
                if (Chance <= CatTable[0].Chances[i])
                {
                    Cat_Id = 6 + i;
                    break;
                }
            }
        }
        else if (Chance >= 833 && Chance <= 915)
        {
            //Lightning
            Chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

            for (int i = 0; i < CatTable[0].Chances.Length; i++)
            {
                if (Chance <= CatTable[0].Chances[i])
                {
                    Cat_Id = 12 + i;
                    break;
                }
            }
        }
        else if (Chance >= 916 && Chance <= 999)
        {
            //Ice
            Chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

            for (int i = 0; i < CatTable[0].Chances.Length; i++)
            {
                if (Chance <= CatTable[0].Chances[i])
                {
                    Cat_Id = 18 + i;
                    break;
                }
            }
        }

        return CreateArmour(Mat_Id, State, skill, Cat_Id);
    }

    public Item CreateArmour(int Mat_Id, ArmourType Type, SkillType SkillType, int Cat_Id)
    {
        Item armour;

        Catalyst[] Cats = Roller.roller.Cats;

        if (Type == ArmourType.Shield)
        {
            armour = Instantiate(PrefabIDs.prefabIDs.ShieldHolder).GetComponent<Item>();
        }
        else
        {
            armour = Instantiate(PrefabIDs.prefabIDs.ArmourHolder).GetComponent<Item>();
        }

        BaseArmour ArmourBase;

        int Catogory = (int)SkillType;

        int rarityId = Cat_Id % 6;

        ArmourStats stats = new ArmourStats();

        switch (Type)
        {
            case ArmourType.Helmet:
                ArmourBase = Armour[0];
                break;
            case ArmourType.RightPauldron:
            case ArmourType.LeftPauldron:
                ArmourBase = Armour[1];
                break;
            case ArmourType.ChestPlate:
                ArmourBase = Armour[2];
                break;
            case ArmourType.RightGauntlet:
            case ArmourType.LeftGauntlet:
                ArmourBase = Armour[3];
                break;
            case ArmourType.Leggings:
                ArmourBase = Armour[4];
                break;
            case ArmourType.RightBoot:
            case ArmourType.LeftBoot:
                ArmourBase = Armour[5];
                break;
            default: // shield
                ArmourBase = Armour[6];
                break;
        }

        stats.Rarity = GlobalValues.rarities[rarityId];

        ArmourHolder ArmourH = armour as ArmourHolder;

        for (int i = 0; i < 4; i++)
        {
            if (Cats[Cat_Id].CatMultis[i] == 0)
            {
                continue;
            }

            if (i == 0)
            {
                stats.Armour = ArmourBase.Armour[Catogory] * Multis[Mat_Id].Multi * Cats[Cat_Id].CatMultis[i];
                continue;
            }

            ResistenceType resistenceType = new ResistenceType
            {
                Type = (DamageTypeEnum)i,
                Resistence = (byte)(ArmourBase.Resistnce[i - 1][Catogory] * (1 + rarityId))
            };

            stats.Resistences.Add(resistenceType);
        }

        stats.MaxDurability = ArmourBase.Durability;
        stats.CurrentDurability = stats.MaxDurability;

        stats.Weight = ArmourBase.Weight[Catogory];
        stats.ArmourType = Type;

        stats.SkillType = (SkillType)((byte)SkillType + (byte)SkillType.LightArmour);

        StringBuilder sb = new StringBuilder();

        if (Type == ArmourType.Helmet || Type == ArmourType.ChestPlate || Type == ArmourType.Leggings)
        {
            sb.Append(((MaterialType)Mat_Id).ToString());
            sb.Append(Type.ToString());
        }
        else
        {
            sb.Append(Type.ToString());

            int numOfInserts = 1;

            string cname = sb.ToString();

            cname = sb.ToString();

            int insertsFound = 0;

            for (int i = 0; i < cname.Length; i++)
            {
                if (char.IsUpper(cname[i]))
                {
                    if (insertsFound != numOfInserts)
                    {
                        insertsFound++;
                        continue;
                    }

                    sb.Insert(i, ((MaterialType)Mat_Id).ToString());
                    break;
                }
            }
        }

        sb.Append(" (");
        sb.Append(ArmourClasses[Catogory]);
        sb.Append(')');

        string name = sb.ToString();

        for (int i = 0; i < name.Length; i++)
        {
            if (char.IsUpper(name[i]) && i != 0 && name[i - 1] != ' ' && name[i - 1] != '(')
            {
                sb.Insert(i, ' ');
                name = sb.ToString();
            }
        }

        stats.Name = sb.ToString();

        ArmourH.SetStats(stats);

        return armour;
    }
}
