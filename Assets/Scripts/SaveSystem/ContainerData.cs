using UnityEngine;

[System.Serializable]
public class ContainerData
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

    public ContainerData(Inventory ContainerInventory)
    {
        int id;

        NumOfWeapons = ContainerInventory.StartIds[0];
        Weapons = new WeaponStats[NumOfWeapons];
        id = 0;

        for (int i = 0; i < ContainerInventory.StartIds[0]; i++)
        {
            Weapons[id] = new WeaponStats();

            WeaponHolder WeaponH = ContainerInventory.AllItems[i].GetComponent<WeaponHolder>();

            LoadSystem.LoadItem(WeaponH, Weapons[id]);

            id++;
        }

        NumOfArmour = ContainerInventory.StartIds[1] - ContainerInventory.StartIds[0];
        Armour = new ArmourStats[NumOfArmour];
        id = 0;

        for (int i = ContainerInventory.StartIds[0]; i < ContainerInventory.StartIds[1]; i++)
        {
            Armour[id] = new ArmourStats();

            ArmourHolder ArmourH = ContainerInventory.AllItems[i].GetComponent<ArmourHolder>();

            LoadSystem.LoadItem(ArmourH, Armour[id]);

            id++;
        }

        NumOfSpells = ContainerInventory.StartIds[2] - ContainerInventory.StartIds[1];
        Spells = new SpellHolderData[NumOfSpells];
        id = 0;

        for (int i = ContainerInventory.StartIds[1]; i < ContainerInventory.StartIds[2]; i++)
        {
            Spells[id] = new SpellHolderData();

            SpellHolder SpellH = ContainerInventory.AllItems[i].GetComponent<SpellHolder>();

            LoadSystem.LoadItem(SpellH, Spells[id]);

            id++;
        }

        //Runes
        //3 - 2

        NumOfRunes = ContainerInventory.StartIds[3] - ContainerInventory.StartIds[2];
        Runes = new RuneHolderData[NumOfRunes];
        id = 0;

        for (int i = ContainerInventory.StartIds[2]; i < ContainerInventory.StartIds[3]; i++)
        {
            Runes[id] = new RuneHolderData();

            RuneHolder runeRef = ContainerInventory.AllItems[i].GetComponent<RuneHolder>();

            LoadSystem.LoadItem(runeRef, Runes[id]);

            id++;
        }

        NumOfPotions = ContainerInventory.StartIds[4] - ContainerInventory.StartIds[3];
        Potions = new CraftingMaterials[NumOfPotions];
        id = 0;

        for (int i = ContainerInventory.StartIds[3]; i < ContainerInventory.StartIds[4]; i++)
        {
            Consumable Ref = ContainerInventory.AllItems[i].GetComponent<Consumable>();

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

        NumOfResources = ContainerInventory.StartIds[5] - ContainerInventory.StartIds[4];
        Resources = new CraftingMaterials[NumOfResources];
        id = 0;

        for (int i = ContainerInventory.StartIds[4]; i < ContainerInventory.StartIds[5]; i++)
        {
            ResourceHolder Ref = ContainerInventory.AllItems[i].GetComponent<ResourceHolder>();

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

        NumOfMisc = ContainerInventory.AllItems.Count - ContainerInventory.StartIds[5];
        Misc = new CraftingMaterials[NumOfMisc];
        id = 0;

        for (int i = ContainerInventory.StartIds[5]; i < ContainerInventory.AllItems.Count; i++)
        {
            Item Ref = ContainerInventory.AllItems[i].GetComponent<Item>();

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
