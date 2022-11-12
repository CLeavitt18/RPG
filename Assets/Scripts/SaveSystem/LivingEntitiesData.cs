
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
        int index = 0;

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

        index = inventory.FindIndex(rightHand);

        if (index > -1)
        {
            RightHand = true;
            RightHandId = index;
        }

        index = inventory.FindIndex(leftHand);

        if (index > -1 && rightHand != leftHand)
        {
            LeftHand = true;
            LeftHandId = index;
        }
    }
}
