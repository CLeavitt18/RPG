using UnityEngine;
using TMPro;

public class ItemInteractiable : Interactialbes, IInteractable
{
    public void OnEnable()
    {
        PlayerInstructionText = PlayerUi.playerUi.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void Interact(bool State)
    {
        Player.player.Inventory.AddItem(gameObject.GetComponent<Item>(), true, 1);
        InventoryUi.playerUi.CallSetInventory(InventoryUi.playerUi.GetMode());
        gameObject.GetComponent<Item>().StoreItem();
    }

    public override void SetUiOpen()
    {
        PlayerInstructionText.text = "E: Take";

        PlayerInstructionText.gameObject.SetActive(true);
        UIOpen = true;

        NextTime = Time.time + WaitTime;
    }
}
