using System.Text;
using UnityEngine;

public class ArmourRoller : MonoBehaviour
{
    [SerializeField] private BaseArmour[] Armour;

    [SerializeField] private MatMults[] Multis;

    [SerializeField] private Catalyst[] Cats;

    [SerializeField] private DropTable[] TypeTable;
    [SerializeField] private DropTable[] ClassTable;
    [SerializeField] private DropTable[] MatTable;
    [SerializeField] private DropTable[] CatTable;

    [SerializeField] private char[] ArmourClasses;

    public Item RollArmour()
    {
        int Mat_Id;
        int Cat_Id;

        SkillType skill;

        ArmourType State;

        int Chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        switch (Chance)
        {
            case int x when x <= MatTable[0].Chances[0]:
                Mat_Id = 0;
                break;     
            case int x when x > MatTable[0].Chances[0] && x <= MatTable[0].Chances[1]:
                Mat_Id = 1;
                break;
            case int x when x > MatTable[0].Chances[1] && x <= MatTable[0].Chances[2]:
                Mat_Id = 2;
                break;            
            case int x when x > MatTable[0].Chances[2] && x <= MatTable[0].Chances[3]:
                Mat_Id = 3;
                break;  
            case int x when x > MatTable[0].Chances[3] && x <= MatTable[0].Chances[4]:
                Mat_Id = 4;
                break;            
            case int x when x > MatTable[0].Chances[4] && x <= MatTable[0].Chances[5]:
                Mat_Id = 5;
                break;
            case int x when x > MatTable[0].Chances[5] && x <= MatTable[0].Chances[6]:
                Mat_Id = 6;
                break;            
            case int x when x > MatTable[0].Chances[6] && x <= MatTable[0].Chances[7]:
                Mat_Id = 7;
                break;        
            case int x when x > MatTable[0].Chances[7] && x <= MatTable[0].Chances[8]:
                Mat_Id = 8;
                break;
            case int x when x > MatTable[0].Chances[8] && x <= MatTable[0].Chances[9]:
                Mat_Id = 9;
                break;            
            case int x when x > MatTable[0].Chances[9] && x <= MatTable[0].Chances[10]:
                Mat_Id = 10;
                break;
            default: //case int x when x > MatTable[0].Chances[10]:
                Mat_Id = 11;
                break;
        }

        Chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        switch (Chance)
        {
            case int x when x <= TypeTable[0].Chances[0]:
                State = ArmourType.Helmat;
                break;
            case int x when x > TypeTable[0].Chances[0] && x <= TypeTable[0].Chances[1]:
                State = ArmourType.RightPauldron;
                break;
            case int x when x > TypeTable[0].Chances[1] && x <= TypeTable[0].Chances[2]:
                State = ArmourType.LeftPauldron;
                break;
            case int x when x > TypeTable[0].Chances[2] && x <= TypeTable[0].Chances[3]:
                State = ArmourType.ChestPlate;
                break;
            case int x when x > TypeTable[0].Chances[3] && x <= TypeTable[0].Chances[4]:
                State = ArmourType.RightGauntlet;
                break;
            case int x when x > TypeTable[0].Chances[4] && x <= TypeTable[0].Chances[5]:
                State = ArmourType.LeftGauntlet;
                break;            
            case int x when x > TypeTable[0].Chances[5] && x <= TypeTable[0].Chances[6]:
                State = ArmourType.Leggings;
                break;
            case int x when x > TypeTable[0].Chances[6] && x <= TypeTable[0].Chances[7]:
                State = ArmourType.RightBoot;
                break;
            default: //case int x when x > TypeTable[0].Chances[7]:
                State = ArmourType.LeftBoot;
                break;
        }

        Chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        switch (Chance)
        {
            case int x when x <= ClassTable[0].Chances[0]:
                skill = SkillType.LightArmour;
                break;
            case int x when x > ClassTable[0].Chances[0] && x <= ClassTable[0].Chances[1]:
                    skill = SkillType.MediumArmour;
                break;
            default: //x > ClassTable[0].Chances[1]:
                skill = SkillType.HeavyArmour;
                break;
        }

        Chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        switch (Chance)
        {
            case int x when x < CatTable[0].Chances[0]:
                Cat_Id = 0;
                break;
            case int x when (x >= CatTable[0].Chances[0] && x < CatTable[0].Chances[1]):
                Cat_Id = 1;
                break;
            case int x when (x >= CatTable[0].Chances[1] && x < CatTable[0].Chances[2]):
                Cat_Id = 2;
                break;
            case int x when (x >= CatTable[0].Chances[2] && x < CatTable[0].Chances[3]):
                Cat_Id = 3;
                break;
            case int x when (x >= CatTable[0].Chances[3] && x < CatTable[0].Chances[4]):
                Cat_Id = 4;
                break;
            default: //case int x when x >= CatTable[0].Chances[4]:
                Cat_Id = 5;
                break;
        }

        return CreateArmour(Mat_Id, State, skill, Cat_Id);
    }

    public Item CreateArmour(int Mat_Id, ArmourType Type, SkillType SkillType, int Cat_Id)
    {
        Item armour = Instantiate(PrefabIDs.prefabIDs.ArmourHolder).GetComponent<Item>();

        BaseArmour ArmourBase;

        int Catogory;

        switch (SkillType)
        {
            case SkillType.HeavyArmour:
                Catogory = 2;
                break;
            case SkillType.MediumArmour:
                Catogory = 1;
                break;
            default: // Light Armour
                Catogory = 0;
                break;
        }

        if (Type == ArmourType.Helmat)
        {
            ArmourBase = Armour[0];
        }
        else if (Type == ArmourType.RightPauldron || Type == ArmourType.LeftPauldron)
        {
            ArmourBase= Armour[1];
        }
        else if (Type == ArmourType.ChestPlate)
        {
            ArmourBase = Armour[2];
        }
        else if (Type == ArmourType.RightGauntlet || Type == ArmourType.LeftGauntlet)
        {
            ArmourBase = Armour[3];
        }
        else if (Type == ArmourType.Leggings)
        {
            ArmourBase = Armour[4];
        }
        else //Boots
        {
            ArmourBase = Armour[5];
        }

        ArmourHolder ArmourH = armour as ArmourHolder;

        ArmourH.Armour = ArmourBase.Armour[Catogory] * Multis[Mat_Id].Multi * Cats[Cat_Id].CatMultis[0];

        ArmourH.CurrentDurability = ArmourBase.Durability;
        ArmourH.MaxDurability = ArmourH.CurrentDurability;

        ArmourH.Weight = ArmourBase.Weight[Catogory];
        ArmourH.ArmourType = Type;

        ArmourH.SkillType = SkillType;

        StringBuilder sb = new StringBuilder();

        if (Type == ArmourType.Helmat || Type == ArmourType.ChestPlate || Type == ArmourType.Leggings)
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

        ArmourH.Name = sb.ToString();

        ArmourH.SetArmourState();

        return armour;
    }
}
