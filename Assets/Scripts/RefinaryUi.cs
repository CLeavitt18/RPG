using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefinaryUi : MonoBehaviour
{
    public int R_Id;
    public int ResourceAmount = 1;

    public BaseRecipes ResourceRecipes;

    public Transform ResourceUiContentHolder;

    public GameObject ConfirmUi;
    public GameObject ResourceSlot;

    public GameObject[] Resources;

    public Slider ResourceSlider;

    public Text ResourceAmountText;

    public ItemAmount ItemsRequired;
    //public ItemAmount Temp;

    public void SetResourceId(int Id)
    {
        R_Id = Id;
        DisplayResourceCost();
    }

    public void SetRefinaryToDefault()
    {
        R_Id = 0;
        ResourceAmount = 1;
        ResourceSlider.value = 1;

        ConfirmUi.SetActive(false);

        SetResourceAmountId();
    }

    public void SetResourceAmountId()
    {
        ResourceAmount = (int)ResourceSlider.value;
        ResourceAmountText.text = "" + ResourceAmount;

        DisplayResourceCost();
    }
    public void DisplayResourceCost()
    {
        if (ResourceUiContentHolder.childCount > 0)
        {
            //Debug.Log("Deleteting Resource Texts");
            int Loops = ResourceUiContentHolder.childCount;
            //Debug.Log("Number of Loops " + Loops);

            for (int i = 0; i < Loops; i++)
            {
                //Debug.Log("Number of Loops " + ResourceUiContentHolder.childCount);

                Destroy(ResourceUiContentHolder.GetChild(i).gameObject);

            }

            ResourceUiContentHolder.DetachChildren();
        }

        ItemsRequired = new ItemAmount(0);

        for (int i = 0; i < ResourceRecipes.ItemsRequired[R_Id].Amount.Length; i++)
        {
            if (ItemsRequired.Amount.Length > 0)
            {
                ItemAmount Temp = new ItemAmount(ItemsRequired.Amount.Length + 1);

                for (int x = 0; x < ItemsRequired.Amount.Length; x++)
                {
                    Temp.Amount[x] = ResourceRecipes.ItemsRequired[R_Id].Amount[x] * ResourceAmount;
                    Temp.Item[x] = ResourceRecipes.ItemsRequired[R_Id].Item[x];

                }

                Temp.Amount[ItemsRequired.Amount.Length] = ResourceRecipes.ItemsRequired[R_Id].Amount[i] * ResourceAmount;
                Temp.Item[ItemsRequired.Item.Length] = ResourceRecipes.ItemsRequired[R_Id].Item[i];

                ItemsRequired = Temp;

            }
            else
            {
                ItemsRequired = new ItemAmount(1);

                ItemsRequired.Amount[0] = ResourceRecipes.ItemsRequired[R_Id].Amount[i] * ResourceAmount;
                ItemsRequired.Item[0] = ResourceRecipes.ItemsRequired[R_Id].Item[i];

            }

            Instantiate(ResourceSlot, ResourceUiContentHolder);
        }

        for (int i = 0; i < ResourceUiContentHolder.childCount; i++)
        {
            ResourceUiContentHolder.GetChild(i).gameObject.GetComponent<Text>().text = ItemsRequired.Item[i] + " x " + ItemsRequired.Amount[i];
            ResourceUiContentHolder.GetChild(i).gameObject.GetComponent<Text>().color = Color.red;

            for (int x = Player.player.Inventory.StartIds[3]; x < Player.player.Inventory.StartIds[4]; x++)
            {

                if (Player.player.Inventory.AllItems[x].name == ItemsRequired.Item[i] &&
                    Player.player.Inventory.AllItems[x].GetComponent<ResourceHolder>().Amount >= ItemsRequired.Amount[i])
                {
                    ResourceUiContentHolder.GetChild(i).gameObject.GetComponent<Text>().color = Color.black;
                    break;
                }
            }
        }
    }

    public void CheckRequirements()
    {
        int Target = 0;
        int Count = 0;


        for (int i = 0; i < ResourceRecipes.ItemsRequired[R_Id].Amount.Length; i++)
        {
            Target++;

            for (int x = Player.player.Inventory.StartIds[3]; x < Player.player.Inventory.StartIds[4]; x++)
            {
                if (Player.player.Inventory.AllItems[x].name == ResourceRecipes.ItemsRequired[R_Id].Item[i] &&
                    Player.player.Inventory.AllItems[x].GetComponent<ResourceHolder>().Amount >= ResourceRecipes.ItemsRequired[R_Id].Amount[i] * ResourceAmount)
                {
                    Count++;
                }
            }
        }

        if (Count == Target)
        {
            ConfirmUi.SetActive(true);
        }
    }

    public void CancelRefine()
    {
        ConfirmUi.SetActive(false);
    }

    public void Refine()
    {
        Inventory Inventory = Player.player.Inventory;
        List<Item> AllItems = Inventory.AllItems;
        int[] StartIds = Inventory.StartIds;

        for (int i = 0; i < ItemsRequired.Amount.Length; i++)
        {
            for (int x = StartIds[3]; x < StartIds[4]; x++)
            {
                if (AllItems[x].name == ItemsRequired.Item[i])
                {
                    Inventory.RemoveItem(Player.player.Inventory.AllItems[x], ItemsRequired.Amount[i] * ResourceAmount);
                }
            }
        }

        Item RH = Instantiate(Resources[R_Id]).GetComponent<Item>();
        RH.name = Resources[R_Id].name;

        RH.Amount = ResourceAmount;

        Player.player.Inventory.AddItem(RH, true, ResourceAmount);
        InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.Mode);

        SetRefinaryToDefault();
    }
}
