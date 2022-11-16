using System.Text;
using UnityEngine;
using TMPro;

public class RuneTable : Interactialbes, IInteractable
{
    [SerializeField] private GameObject UI;

    public void OnEnable()
    {
        PUIInsruction = PlayerUi.playerUi.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        gameObject.name = Name;
    }

    public override void SetUiOpen()
    {
        StringBuilder sb = new StringBuilder("E: Use ");
        sb.Append(Name);

        PUIInsruction.text = sb.ToString();


        base.SetUiOpen();
    }

    public void Interact(bool State)
    {
        RuneTableUI.table.SetState(State);
        SetPlayerState(State);
    }
}
