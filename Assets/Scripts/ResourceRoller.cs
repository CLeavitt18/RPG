using UnityEngine;

public class ResourceRoller : MonoBehaviour
{
    [SerializeField] private ResourceList[] Resources;

    [SerializeField] private DropTable[] OreTable;
    [SerializeField] private DropTable[] IngotTable;
    [SerializeField] private DropTable[] OtherTable;
    [SerializeField] private DropTable[] AmountTable;

    public Item RollResource()
    {
        int Re_Type = 0;
        int Re_Id = 0;
        int Amount = 1;

        int temp;
        temp = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        if (temp < 500)
        {
            //Ore was selected
            temp = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

            Re_Type = 0;

            switch (temp)
            {
                case int x when (x <= OreTable[0].Chances[0]):
                    Re_Id = 0;
                    break;
                case int x when (x > OreTable[0].Chances[0] && x <= OreTable[0].Chances[1]):
                    Re_Id = 1;
                    break;
                case int x when (x > OreTable[0].Chances[1] && x <= OreTable[0].Chances[2]):
                    Re_Id = 2;
                    break;
                case int x when (x > OreTable[0].Chances[2] && x <= OreTable[0].Chances[3]):
                    Re_Id = 4;
                    break;
                case int x when (x > OreTable[0].Chances[4] && x <= OreTable[0].Chances[5]):
                    Re_Id = 5;
                    break;
                case int x when (x > OreTable[0].Chances[5] && x <= OreTable[0].Chances[6]):
                    Re_Id = 6;
                    break;
            }

        }
        else if (temp >= 500 && temp < 800)
        {
            //Ingot was selected
            temp = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

            Re_Type = 1;

            switch (temp)
            {
                case int x when (x <= IngotTable[0].Chances[0]):
                    Re_Id = 0;
                    break;
                case int x when (x > IngotTable[0].Chances[0] && x <= IngotTable[0].Chances[1]):
                    Re_Id = 1;
                    break;
                case int x when (x > IngotTable[0].Chances[1] && x <= IngotTable[0].Chances[2]):
                    Re_Id = 2;
                    break;
                case int x when (x > IngotTable[0].Chances[2] && x <= IngotTable[0].Chances[3]):
                    Re_Id = 4;
                    break;
                case int x when (x > IngotTable[0].Chances[4] && x <= IngotTable[0].Chances[5]):
                    Re_Id = 5;
                    break;
                case int x when (x > IngotTable[0].Chances[5] && x <= IngotTable[0].Chances[6]):
                    Re_Id = 6;
                    break;
                case int x when (x > IngotTable[0].Chances[6] && x <= IngotTable[0].Chances[7]):
                    Re_Id = 7;
                    break;
                case int x when (x > IngotTable[0].Chances[7] && x <= IngotTable[0].Chances[8]):
                    Re_Id = 8;
                    break;
            }
        }
        else if (temp >= 800 && temp < 999)
        {
            //Ingot was selected
            temp = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

            Re_Type = 2;

            switch (temp)
            {
                case int x when (x <= OtherTable[0].Chances[0]):
                    Re_Id = 0;
                    break;
                case int x when (x > OtherTable[0].Chances[0] && x <= OtherTable[0].Chances[1]):
                    Re_Id = 1;
                    break;
                case int x when (x > OtherTable[0].Chances[1] && x <= OtherTable[0].Chances[2]):
                    Re_Id = 2;
                    break;
                case int x when (x > OtherTable[0].Chances[2] && x <= OtherTable[0].Chances[3]):
                    Re_Id = 4;
                    break;
            }
        }

        //Amount roll
        temp = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        switch (temp)
        {
            case int x when (x <= AmountTable[0].Chances[0]):
                Amount = 1;
                break;
            case int x when (x > AmountTable[0].Chances[0] && x <= AmountTable[0].Chances[1]):
                Amount = 2;
                break;
            case int x when (x > AmountTable[0].Chances[1] && x <= AmountTable[0].Chances[2]):
                Amount = 3;
                break;
        }

        Item Item = Instantiate(Resources[Re_Type].Resources[Re_Id]).GetComponent<Item>();

        Item.Amount = Amount;

        return Item;
    }
}
