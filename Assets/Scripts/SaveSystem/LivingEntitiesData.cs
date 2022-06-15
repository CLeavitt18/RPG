
[System.Serializable]
public class LivingEntitiesData
{
    public AttributeStruct[] Attributes = new AttributeStruct[3];
    
    public int Level;
    public int RightHandId;
    public int LeftHandId;
    public int NumOfShrines;
    public int NumOfMinions;

    public float[] Position = new float[3];
    public float[] Rotation = new float[3];

    public MinionData[] Minions;

    public bool FirstOpen = true;

    //false = nothing equiped true = something is equiped
    public bool RightHand = false;
    public bool LeftHand = false;

    public InventoryData inventoryData;


    public LivingEntitiesData(LivingEntities entity)
    {
        int start;
        int end;
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

        start = 0;
        end = inventory.GetStart(GlobalValues.ArmourStart);

        for (int i = start; i < end; i++)
        {
            WeaponHolder WeaponH = inventory[i].GetComponent<WeaponHolder>();

            if (inventory[i] == rightHand)
            {
                RightHand = true;
                RightHandId = i;
            }

            if (inventory[i] == leftHand && rightHand != leftHand)
            {
                LeftHand = true;
                LeftHandId = i;
            }

            id++;
        }

        start = inventory.GetStart(GlobalValues.ArmourStart);
        end = inventory.GetStart(GlobalValues.SpellStart);

        for (int i = start; i < end; i++)
        {
            ArmourHolder ArmourH = inventory[i].GetComponent<ArmourHolder>();   

            if (inventory[i] == rightHand)
            {
                RightHand = true;
                RightHandId = i;
            }
            else if (inventory[i] == leftHand)
            {
                LeftHand = true;
                LeftHandId = i;
            }
        }

        start = inventory.GetStart(GlobalValues.SpellStart);
        end = inventory.GetStart(GlobalValues.RuneStart);

        id = 0;

        for (int i = start; i < end; i++)
        {
            SpellHolder SpellH = inventory[i].GetComponent<SpellHolder>();

            if (inventory[i] == rightHand)
            {
                RightHand = true;
                RightHandId = i;
            }

            if (inventory[i] == leftHand && rightHand != leftHand)
            {
                LeftHand = true;
                LeftHandId = i;
            }

            id++;
        }

        start = inventory.GetStart(GlobalValues.MiscStart);
        end = inventory.Count;

        for(int i = start; i < end; i++)
        {
            if(inventory[i] is TorchHolder torch && torch.GetEquiped())
            {
                if(torch == rightHand)
                {
                    RightHand = true;
                    RightHandId = i;
                }
                else
                {
                    LeftHand = true;
                    LeftHandId = i;
                }
            }
        }
    }
}
