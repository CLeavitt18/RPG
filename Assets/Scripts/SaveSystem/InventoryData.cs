using UnityEngine;

[System.Serializable]
public class InventoryData
{
    public int NumOfWeapons;
    public int NumOfArmour;
    public int NumOfSpells;
    public int NumOfRunes;
    public int NumOfPotions;
    public int NumOfResources;
    public int NumOfMisc;

    public WeaponStats[] Weapons;

    public ArmourStats[] Armour;

    public SpellHolderData[] Spells;

    public RuneHolderData[] Runes;

    public CraftingMaterials[] Potions;
    public CraftingMaterials[] Resources;
    public CraftingMaterials[] Misc;


    public InventoryData(Inventory inventory)
    {
        int start;
        int end;
        int id;

        start = 0;
        end = inventory.GetStart(GlobalValues.ArmourStart);

        NumOfWeapons = end;
        Weapons = new WeaponStats[NumOfWeapons];
        id = 0;

        for (int i = start; i < end; i++)
        {
            Weapons[id] = new WeaponStats();

            WeaponHolder WeaponH = inventory[i].GetComponent<WeaponHolder>();

            LoadSystem.LoadItem(WeaponH, Weapons[id]);

            id++;
        }

        start = inventory.GetStart(GlobalValues.ArmourStart);
        end = inventory.GetStart(GlobalValues.SpellStart);

        NumOfArmour = end - start;
        Armour = new ArmourStats[NumOfArmour];
        id = 0;

        for (int i = start; i < end; i++)
        {
            Armour[id] = new ArmourStats();

            ArmourHolder ArmourH = inventory[i].GetComponent<ArmourHolder>();

            LoadSystem.LoadItem(ArmourH, Armour[id]);

            id++;
        }

        start = inventory.GetStart(GlobalValues.SpellStart);
        end = inventory.GetStart(GlobalValues.RuneStart);

        NumOfSpells = end - start;
        Spells = new SpellHolderData[NumOfSpells];
        id = 0;

        for (int i = start; i < end; i++)
        {
            Spells[id] = new SpellHolderData();

            SpellHolder SpellH = inventory[i].GetComponent<SpellHolder>();

            LoadSystem.LoadItem(SpellH, Spells[id]);

            id++;
        }

        //Runes
        start = inventory.GetStart(GlobalValues.RuneStart);
        end = inventory.GetStart(GlobalValues.PotionStart);

        NumOfRunes = end - start;
        Runes = new RuneHolderData[NumOfRunes];
        id = 0;

        for (int i = start; i < end; i++)
        {
            Runes[id] = new RuneHolderData();

            RuneHolder runeRef = inventory[i].GetComponent<RuneHolder>();

            LoadSystem.LoadItem(runeRef, Runes[id]);

            id++;
        }

        start = inventory.GetStart(GlobalValues.PotionStart);
        end = inventory.GetStart(GlobalValues.ResourceStart);
        
        NumOfPotions = end - start;
        Potions = new CraftingMaterials[NumOfPotions];
        id = 0;

        for (int i = start; i < end; i++)
        {
            Consumable Ref = inventory[i].GetComponent<Consumable>();

            for (int x = 0; x < PrefabIDs.prefabIDs.Potions.Length; x++)
            {
                if (PrefabIDs.prefabIDs.Potions[x].name == Ref.Name)
                {
                   Potions[id].ResourceId = x;
                    break;
                }
            }

            Potions[id].Amount = Ref.Amount;
            id++;
        }

        start = inventory.GetStart(GlobalValues.ResourceStart);
        end = inventory.GetStart(GlobalValues.MiscStart);

        NumOfResources = end - start;
        Resources = new CraftingMaterials[NumOfResources];
        id = 0;

        for (int i = start; i < end; i++)
        {
            Item item = inventory[i];

            for (int x = 0; x < PrefabIDs.prefabIDs.CraftingMaterials.Length; x++)
            {
                if (PrefabIDs.prefabIDs.CraftingMaterials[x].name == item.Name)
                {
                    Resources[id].ResourceId = x;
                    break;
                }
            }
            
            Resources[id].Amount = item.Amount;
            id++;
        }

        start = inventory.GetStart(GlobalValues.MiscStart);
        end = inventory.Count;

        NumOfMisc = end - start;
        Misc = new CraftingMaterials[NumOfMisc];
        id = 0;

        for (int i = start; i < end; i++)
        {
            Item Ref = inventory[i];

            for (int x = 0; x < PrefabIDs.prefabIDs.Items.Length; x++)
            {
                if (PrefabIDs.prefabIDs.Items[x] == null)
                {
                    break;
                }

                if (Ref.Name == PrefabIDs.prefabIDs.Items[x].name)
                {
                    Misc[id].ResourceId = x;
                    break;
                }
            }

            Misc[id].Amount = Ref.Amount;

            id++;
        }
    }
}
