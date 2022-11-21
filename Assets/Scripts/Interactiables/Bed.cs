using System.Text;
using UnityEngine;
using TMPro;

public class Bed : Interactialbes, IInteractable
{
    public override void SetUiOpen()
    {
        StringBuilder sb = new StringBuilder(GlobalValues.InterationKey);
        sb.Append(": ");
        sb.Append(GlobalValues.UseText);
        sb.Append(' ');
        sb.Append(Name);

        PlayerInstructionText.text = sb.ToString();

        PlayerInstructionText.gameObject.SetActive(true);
        UIOpen = true;

        NextTime = Time.time + WaitTime;
    }

    public void Interact(bool State)
    {
        PlayerUi.playerUi.CallSetSleepUi();

        SetPlayerState(State);
    }
}
