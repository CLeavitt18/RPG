using System.Text;
using UnityEngine;
using TMPro;

public class Bed : Interactialbes, IInteractable
{
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

        PUIInsruction.gameObject.SetActive(true);
        UIOpen = true;

        NextTime = Time.time + WaitTime;
    }

    public void Interact(bool State)
    {
        PlayerUi.playerUi.CallSetSleepUi();

        SetPlayerState(State);
    }
}
