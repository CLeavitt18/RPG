
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
        int id;

        NumOfWeapons = inventory.StartIds[0];
        Weapons = new WeaponStats[NumOfWeapons];
        id = 0;

        for (int i = 0; i < inventory.StartIds[0]; i++)
        {
            Weapons[id] = new WeaponStats();

            WeaponHolder WeaponH = inventory.AllItems[i].GetComponent<WeaponHolder>();

            LoadSystem.LoadItem(WeaponH, Weapons[id]);

            id++;
        }

        NumOfArmour = inventory.StartIds[1] - inventory.StartIds[0];
        Armour = new ArmourStats[NumOfArmour];
        id = 0;

        for (int i = inventory.StartIds[0]; i < inventory.StartIds[1]; i++)
        {
            Armour[id] = new ArmourStats();

            ArmourHolder ArmourH = inventory.AllItems[i].GetComponent<ArmourHolder>();

            LoadSystem.LoadItem(ArmourH, Armour[id]);

            id++;
        }

        NumOfSpells = inventory.StartIds[2] - inventory.StartIds[1];
        Spells = new SpellHolderData[NumOfSpells];
        id = 0;

        for (int i = inventory.StartIds[1]; i < inventory.StartIds[2]; i++)
        {
            Spells[id] = new SpellHolderData();

            SpellHolder SpellH = inventory.AllItems[i].GetComponent<SpellHolder>();

            LoadSystem.LoadItem(SpellH, Spells[id]);

            id++;
        }

        //Runes
        //2 = start of runes
        //3 = end of runes

        NumOfRunes = inventory.StartIds[3] - inventory.StartIds[2];
        Runes = new RuneHolderData[NumOfRunes];
        id = 0;

        for (int i = inventory.StartIds[2]; i < inventory.StartIds[3]; i++)
        {
            Runes[id] = new RuneHolderData();

            RuneHolder runeRef = inventory.AllItems[i].GetComponent<RuneHolder>();

            LoadSystem.LoadItem(runeRef, Runes[id]);

            id++;
        }

        NumOfPotions = inventory.StartIds[4] - inventory.StartIds[3];
        Potions = new CraftingMaterials[NumOfPotions];
        id = 0;

        for (int i = inventory.StartIds[3]; i < inventory.StartIds[4]; i++)
        {
            Consumable Ref = inventory.AllItems[i].GetComponent<Consumable>();

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

        NumOfResources = inventory.StartIds[5] - inventory.StartIds[4];
        Resources = new CraftingMaterials[NumOfResources];
        id = 0;

        for (int i = inventory.StartIds[4]; i < inventory.StartIds[5]; i++)
        {
            ResourceHolder Ref = inventory.AllItems[i].GetComponent<ResourceHolder>();

            for (int x = 0; x < PrefabIDs.prefabIDs.CraftingMaterials.Length; x++)
            {
                if (PrefabIDs.prefabIDs.CraftingMaterials[x].name == Ref.Name)
                {
                    Resources[id].ResourceId = x;
                    break;
                }
            }
            
            Resources[id].Amount = Ref.Amount;
            id++;
        }

        NumOfMisc = inventory.AllItems.Count - inventory.StartIds[5];
        Misc = new CraftingMaterials[NumOfMisc];
        id = 0;

        for (int i = inventory.StartIds[5]; i < inventory.AllItems.Count; i++)
        {
            Item Ref = inventory.AllItems[i].GetComponent<Item>();

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
