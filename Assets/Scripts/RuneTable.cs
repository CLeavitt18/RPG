using System.Text;
using UnityEngine;
using TMPro;

public class RuneTable : Interactialbes, IInteractable
{
    public override void SetUiOpen()
    {
        StringBuilder sb = new StringBuilder(GlobalValues.InterationKey);
        sb.Append(": ");
        sb.Append(GlobalValues.UseText);
        sb.Append(' ');
        sb.Append(Name);

        PlayerInstructionText.text = sb.ToString();

        base.SetUiOpen();
    }

    public void Interact(bool State)
    {
        RuneTableUI.table.SetState(State);
        SetPlayerState(State);
    }
}
