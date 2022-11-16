using UnityEngine;
using UnityEngine.UI;

public class ItemInteractiable : Interactialbes, IInteractable
{
    public void OnEnable()
    {
        PUIInsruction = GameObject.Find("Player UI").transform.GetChild(0).transform.GetChild(1).gameObject;
    }

    public void Interact(bool State)
    {
        Player.player.Inventory.AddItem(gameObject.GetComponent<Item>(), true, 1);
        InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.GetMode());
        gameObject.GetComponent<Item>().StoreItem();
    }

    public override void SetUiOpen()
    {
        PUIInsruction.GetComponent<Text>().text = "E: Take";

        PUIInsruction.SetActive(true);
        UIOpen = true;

        NextTime = Time.time + WaitTime;
    }
}
