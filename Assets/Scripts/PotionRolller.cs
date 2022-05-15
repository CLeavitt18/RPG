using UnityEngine;

public class PotionRolller : MonoBehaviour
{
    [SerializeField] private GameObject[] Potions;

    [SerializeField] private DropTable[] Tables;
    [SerializeField] private DropTable[] AmountTables;

    public Item RollPotion()
    {
        int PotionId = 0;
        int Amount = 0;

        int chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        switch (chance)
        {
            case int x when (x <= Tables[0].Chances[0]):
                PotionId = 0;
                break;
            case int x when (x > Tables[0].Chances[0] && x <= Tables[0].Chances[1]):
                PotionId = 1;
                break;
            case int x when (x > Tables[0].Chances[1]):
                PotionId = 2;
                break;
        }

        chance = Random.Range(GlobalValues.MinRoll, GlobalValues.MaxRoll);

        switch (chance)
        {
            case int x when (x <= AmountTables[0].Chances[0]):
                Amount = 1;
                break;case int x when (x > AmountTables[0].Chances[0] && x <= AmountTables[0].Chances[1]):
                Amount = 2;
                break;case int x when (x > AmountTables[0].Chances[1]):
                Amount = 3;
                break;
        }

        Item potion = Instantiate(Potions[PotionId]).GetComponent<Item>();
        potion.Amount = Amount;

        return potion;
    }
}
