using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SpellCraftingTable : Interactialbes, IInteractable
{
    [SerializeField] private GameObject Ui;

    public void OnEnable()
    {
        PUIInsruction = PlayerUi.playerUi.transform.GetChild(2).transform.GetChild(1).gameObject;
        gameObject.name = Name;
    }

    public override void SetUiOpen()
    {
        StringBuilder sb = new StringBuilder("E: Use ");
        sb.Append(Name);

        PUIInsruction.GetComponent<Text>().text = sb.ToString();


        base.SetUiOpen();
    }

    public void Interact(bool State)
    {
        SpellCraftingTableUi.table.SetState(State);
        SetPlayerState(State);
    }
}
