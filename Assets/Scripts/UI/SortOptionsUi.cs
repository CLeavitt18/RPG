using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SortOptionsUi : IUi
{
    [SerializeField] private GameObject ui;
    [SerializeField] private InventoryUi inventoryUi;

    [SerializeField] private Inventory inventory;

    [SerializeField] private TMP_Dropdown sortOrderDropDown;
    
    [SerializeField] private Toggle sortOrderNormalToggle;

    public override void Set()
    {
        if (isActive)
        {
            return;
        }

        isActive = true;

        ui.SetActive(true);
        
        if (InventoryUi.playerUi != null)
        {
            inventory = Player.player.Inventory;
            inventoryUi = InventoryUi.playerUi;
        }
        else
        {
            inventory = Player.player.GetHit().GetComponent<Inventory>();
            inventoryUi = InventoryUi.containerUi;
        }

        for (int sortOrder = 0; sortOrder < 3; sortOrder++)
        {
            sortOrderDropDown.options.Add(new TMP_Dropdown.OptionData(((SortOrder)sortOrder).ToString()));
        }
        sortOrderDropDown.value = inventory.GetSrotOrdor();

        sortOrderNormalToggle.isOn = !inventory.GetSortOrdorNormal();
    }

    public override void Clear()
    {
        if (!isActive)
        {
            return;
        }

        isActive = false;
        inventory.Sort();
        inventoryUi.Set();

        sortOrderDropDown.ClearOptions();

        ui.SetActive(false);
    }

    public void SetSortOrder()
    {
        inventory.SetSortOrder(sortOrderDropDown.value);
    }

    public void SetSortOrderNormal()
    {
        inventory.SetSortOrderNormal(!sortOrderNormalToggle.isOn);
    }
}
