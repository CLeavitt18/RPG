using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefineryUi : MonoBehaviour
{
    public int R_Id;
    public int ResourceAmount = 1;

    [SerializeField] private int[] maxResources;

    [SerializeField] private bool canCraft;

    public BaseRecipes ResourceRecipes;

    [SerializeField] public Transform ResourceCostHolder;
    [SerializeField] private Transform contentHolder;

    [SerializeField] public GameObject ConfirmUi;
    [SerializeField] private GameObject amountUi;
    [SerializeField] private GameObject refinerySlot;

    public GameObject[] Resources;

    public Slider ResourceSlider;

    public Text ResourceAmountText;

    //public ItemAmount ItemsRequired;

    [SerializeField] private DictionaryOfStringAndInt requiredItems = new DictionaryOfStringAndInt(10);


    public void SetResourceId(int Id)
    {
        R_Id = Id;

        if (maxResources[R_Id] > 1)
        {
            ResourceSlider.maxValue = maxResources[R_Id];
            ResourceSlider.value = 1;
            amountUi.SetActive(true);
        }
        else
        {
            ResourceAmount = 1;
            DisplayResourceCost();
        }
    }

    public void SetRefineryToDefault(bool reset)
    {
        R_Id = 0;
        ResourceAmount = 1;
        ResourceSlider.value = 1;

        ConfirmUi.SetActive(false);

        if (ResourceCostHolder.childCount > 0)
        {
            Destroy(ResourceCostHolder.GetChild(0).gameObject);
        }

        if (reset == false)
        {
            return;
        }

        if (contentHolder.childCount > 0)
        {
            int children = contentHolder.childCount;

            for (int i = 0; i < children; i++)
            {
                Destroy(contentHolder.GetChild(i).gameObject);
            }

            contentHolder.DetachChildren();
            return;
        }

        maxResources = new int[Resources.Length];

        for (int i = 0; i < Resources.Length; i++)
        {
            //DO NOT REMOVE
            int id = i;

            Button button = Helper.helper.CreateCraftingButton(refinerySlot, Resources[i].GetComponent<Item>().GetName(), contentHolder);
            button.onClick.AddListener(delegate { SetResourceId(id); });

            Text text = button.transform.GetChild(0).GetComponent<Text>();

            Inventory pInventory = Player.player.Inventory;
            Item item;

            string itemName;
            int itemAmount;

            int[] numItems = new int[ResourceRecipes.ItemsRequired[i].Item.Length];

            for (int x = 0; x < ResourceRecipes.ItemsRequired[i].Item.Length; x++)
            {
                itemName = ResourceRecipes.ItemsRequired[i].Item[x];
                itemAmount = ResourceRecipes.ItemsRequired[i].Amount[x];

                item = pInventory.Find(itemName, GlobalValues.ResourceTag);

                if (item != null && item.GetAmount() >= itemAmount)
                {
                    numItems[x] = item.GetAmount() / ResourceRecipes.ItemsRequired[i].Amount[x];
                }
                else
                {
                    numItems[x] = 0;
                }

                if (x != numItems.Length - 1)
                {
                    continue;
                }

                int currentNum = int.MaxValue;

                for (int y = 0; y < x + 1; y++)
                {
                    if (numItems[y] <= currentNum)
                    {
                        currentNum = numItems[y];
                    }
                }

                maxResources[i] = currentNum;

                if (maxResources[i] > 0)
                {
                    text.color = Color.black;
                }
                else
                {
                    text.color = Color.red;
                }
            }
        }
    }

    public void SelectAllResource()
    {
        ResourceAmount = maxResources[R_Id];
        DisplayResourceCost();
    }

    public void AmountUp()
    {
        ResourceSlider.value++;

        SetResourceAmountId();
    }

    public void AmountDown()
    {
        ResourceSlider.value--;

        SetResourceAmountId();
    }

    public void SetResourceAmountId()
    {
        ResourceAmount = (int)ResourceSlider.value;
        ResourceAmountText.text = "How many?\n" + ResourceAmount;
    }

    public void DisplayResourceCost()
    {
        amountUi.SetActive(false);
        
        if (ResourceCostHolder.childCount > 0)
        {
            Destroy(ResourceCostHolder.GetChild(0).gameObject);
            
            ResourceCostHolder.DetachChildren();
        }

        ItemAmount temp = ResourceRecipes.ItemsRequired[R_Id];

        if (requiredItems.Count > 0)
        {
            requiredItems.Clear();
        }

        for (int i = 0; i < temp.Amount.Length; i++)
        {
            requiredItems.Add(temp.Item[i], temp.Amount[i] * ResourceAmount);
        }

        canCraft = Helper.helper.CreateResourceCostDetails(requiredItems, ResourceCostHolder);
    }

    public void CheckRequirements()
    {
        if (canCraft)
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

        foreach (KeyValuePair<string, int> item in requiredItems)
        {
            Inventory.RemoveItem(item.Key, item.Value, GlobalValues.ResourceTag);
        }

        Item RH = Instantiate(Resources[R_Id]).GetComponent<Item>();
        RH.name = Resources[R_Id].name;

        RH.SetAmount(ResourceAmount);

        Player.player.Inventory.AddItem(RH, true, ResourceAmount);
        InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.GetMode());

        maxResources[R_Id] -= ResourceAmount;
        
        if (maxResources[R_Id] == 0)
        {
            contentHolder.GetChild(R_Id).GetChild(0).GetComponent<Text>().color = Color.red;
        }

        SetRefineryToDefault(false);
    }
}
