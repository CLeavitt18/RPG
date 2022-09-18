using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Containers : Interactialbes, IInteractable
{
    public UiState Mode;

    public bool FirstOpen = true;

    public void Awake()
    {
        PUIInsruction = GameObject.Find("Player UI").transform.GetChild(2).transform.GetChild(1).gameObject;
        gameObject.name = Name;
        Mode = GetComponent<Inventory>().GetMode();
    }

    public override void SetUiOpen()
    {
        Inventory Inventory = GetComponent<Inventory>();

        StringBuilder sb = new StringBuilder();

        switch (Mode)
        {
            case UiState.Container:
                if (Inventory.Count == 0)
                {
                    PUIInsruction.GetComponent<Text>().text = "E: Open (Empty)";
                }
                else
                {
                    sb.Append("E: Open ");
                    sb.Append(Name);

                    PUIInsruction.GetComponent<Text>().text = sb.ToString();
                }
                break;
            case UiState.Store:
                    sb.Append("E: Talk To ");
                    sb.Append(Name);

                    PUIInsruction.GetComponent<Text>().text = sb.ToString();
                break;
        }

        base.SetUiOpen();
    }

    public void Interact(bool State)
    {
        switch (Mode)
        {
            case UiState.Container:
                InventoryUi.containerUi.OpenCloseInventory(State);

                InventoryUi.playerUi.OpenCloseInventory(false);
                break;
            case UiState.Store:
                if (gameObject.GetComponent<AIController>().GetDead())
                {
                    Mode = UiState.Container;
                    Interact(State);
                    return;
                }
                if (Player.player.GetMode() == PlayerState.InStore)
                {
                    InventoryUi.containerUi.OpenCloseInventory(State);

                    InventoryUi.playerUi.OpenCloseInventory(false);
                }

                NPCDialogue.npcD.StartDialogue(State);
                break;
        }

        SetPlayerState(State);

        FirstOpen = false;
    }
}
