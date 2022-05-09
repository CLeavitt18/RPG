using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LivingEntitiesData
{
    public AttributeStruct[] Attributes = new AttributeStruct[3];
    
    public int Level;
    public int NumOfWeapons;
    public int NumOfArmour;
    public int NumOfSpells;
    public int NumOfRunes;
    public int NumOfPotions;
    public int NumOfResources;
    public int NumOfMisc;
    public int CurrentWeaponID;
    public int CurrentOffWeaponID;
    public int CurrentMainHandID;
    public int CurrentOffHandID;
    public int NumOfShrines;
    public int NumOfMinions;

    public float[] Position = new float[3];
    public float[] Rotation = new float[3];

    public WeaponStats[] Weapons;

    public ArmourStats[] Armour;

    public SpellHolderData[] Spells;

    public RuneHolderData[] Runes;

    public MinionData[] Minions;

    public CraftingMaterials[] Potions;
    public CraftingMaterials[] Resources;
    public CraftingMaterials[] Misc;

    public bool FirstOpen = true;

    public List<int>[] Powers = new List<int>[6];

    public LivingEntitiesData(LivingEntities entity)
    {
        int id = 0;
        Inventory inventory = entity.Inventory;

        Item rightHand = entity.GetHeldItem(0);
        Item leftHand = entity.GetHeldItem(1);
       
        Position[0] = entity.transform.position.x;
        Position[1] = entity.transform.position.y;
        Position[2] = entity.transform.position.z;

        Rotation[0] = entity.transform.rotation.eulerAngles.x;
        Rotation[1] = entity.transform.rotation.eulerAngles.y;
        Rotation[2] = entity.transform.rotation.eulerAngles.z;

        Level = entity.GetLevel();

        for (int i = 0; i < 3; i++)
        {
            Attributes[i].Current = entity.GetCurrentAttribute(i);
            Attributes[i].Max = entity.GetMaxAttribute(i);
            Attributes[i].Reserved = entity.GetResrvedAttribute(i);
        }

        NumOfWeapons = inventory.StartIds[0];
        Weapons = new WeaponStats[NumOfWeapons];

        for (int i = 0; i < inventory.StartIds[0]; i++)
        {
            Weapons[i] = new WeaponStats();

            WeaponHolder WeaponH = inventory.AllItems[i].GetComponent<WeaponHolder>();

            LoadSystem.LoadItem(WeaponH, Weapons[id]);

            if (inventory.AllItems[i] == rightHand)
            {
                CurrentMainHandID = 1;
                CurrentWeaponID = i;
            }

            if (inventory.AllItems[i] == leftHand && rightHand != leftHand)
            {
                CurrentOffHandID = 1;
                CurrentOffWeaponID = i;
            }

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

            if (inventory.AllItems[i] == rightHand)
            {
                CurrentMainHandID = 2;
                CurrentWeaponID = i;
            }

            if (inventory.AllItems[i] == leftHand && rightHand != leftHand)
            {
                CurrentOffHandID = 2;
                CurrentOffWeaponID = i;
            }

            id++;
        }

        //Runes
        //3 - 2
        NumOfRunes = inventory.StartIds[3] - inventory.StartIds[2];
        Runes = new RuneHolderData[NumOfRunes];
        id = 0;

        for(int i = inventory.StartIds[2]; i < inventory.StartIds[3]; i++)
        {
            Runes[id] = new RuneHolderData();

            RuneHolder runeRef = inventory.AllItems[i].GetComponent<RuneHolder>();

            LoadSystem.LoadItem(runeRef, Runes[id]);

            id++;
        }

        NumOfPotions = inventory.StartIds[4] - inventory.StartIds[3];
        Potions = new CraftingMaterials[NumOfPotions];
        id = 0;

        for (int i = inventory.StartIds[4]; i < inventory.StartIds[3]; i++)
        {
            Consumable Ref = inventory.AllItems[i].GetComponent<Consumable>();

            for (int x = 0; x < PrefabIDs.prefabIDs.Potions.Length; x++)
            {
                if (Ref.name == PrefabIDs.prefabIDs.Potions[x].name)
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
                if (Ref.Name == PrefabIDs.prefabIDs.CraftingMaterials[x].GetComponent<ResourceHolder>().Name)
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
            //Debug.Log("Name is " + Ref.name);

            for (int x = 0; x < PrefabIDs.prefabIDs.Items.Length; x++)
            {
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
