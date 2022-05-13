
[System.Serializable]
public class LivingEntitiesData
{
    public AttributeStruct[] Attributes = new AttributeStruct[3];
    
    public int Level;
    public int CurrentWeaponID;
    public int CurrentOffWeaponID;
    public int CurrentMainHandID;
    public int CurrentOffHandID;
    public int NumOfShrines;
    public int NumOfMinions;

    public float[] Position = new float[3];
    public float[] Rotation = new float[3];

    public MinionData[] Minions;

    public bool FirstOpen = true;

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
                CurrentMainHandID = 1;
                CurrentWeaponID = i;
            }

            if (inventory[i] == leftHand && rightHand != leftHand)
            {
                CurrentOffHandID = 1;
                CurrentOffWeaponID = i;
            }

            id++;
        }
        
        start = inventory.GetStart(GlobalValues.SpellStart);
        end = inventory.GetStart(GlobalValues.RuneStart);

        id = 0;

        for (int i = start; i < end; i++)
        {
            SpellHolder SpellH = inventory[i].GetComponent<SpellHolder>();

            if (inventory[i] == rightHand)
            {
                CurrentMainHandID = 2;
                CurrentWeaponID = i;
            }

            if (inventory[i] == leftHand && rightHand != leftHand)
            {
                CurrentOffHandID = 2;
                CurrentOffWeaponID = i;
            }

            id++;
        }
    }
}
