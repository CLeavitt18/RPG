using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LivingEntitiesData
{
    public AttributeStruct[] Attributes = new AttributeStruct[3];
    
    public int Level;
   /* public int NumOfWeapons;
    public int NumOfArmour;
    public int NumOfSpells;
    public int NumOfRunes;
    public int NumOfPotions;
    public int NumOfResources;
    public int NumOfMisc;*/
    public int CurrentWeaponID;
    public int CurrentOffWeaponID;
    public int CurrentMainHandID;
    public int CurrentOffHandID;
    public int NumOfShrines;
    public int NumOfMinions;

    public float[] Position = new float[3];
    public float[] Rotation = new float[3];

    //public WeaponStats[] Weapons;

    //public ArmourStats[] Armour;

    //public SpellHolderData[] Spells;

    //public RuneHolderData[] Runes;

    public MinionData[] Minions;

    //public CraftingMaterials[] Potions;
    //public CraftingMaterials[] Resources;
    //public CraftingMaterials[] Misc;

    public bool FirstOpen = true;

    public InventoryData inventoryData;


    public LivingEntitiesData(LivingEntities entity)
    {
        int id = 0;
        Inventory inventory = entity.Inventory;

        inventoryData = new InventoryData(inventory);

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

        for (int i = 0; i < inventory.StartIds[0]; i++)
        {
            WeaponHolder WeaponH = inventory.AllItems[i].GetComponent<WeaponHolder>();

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

        //Uncomment out when ready to check for waht armor is equoped

        /*id = 0;

        for (int i = inventory.StartIds[0]; i < inventory.StartIds[1]; i++)
        {
            ArmourHolder ArmourH = inventory.AllItems[i].GetComponent<ArmourHolder>();

            id++;
        }*/
        
        id = 0;

        for (int i = inventory.StartIds[1]; i < inventory.StartIds[2]; i++)
        {
            SpellHolder SpellH = inventory.AllItems[i].GetComponent<SpellHolder>();

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
    }
}
