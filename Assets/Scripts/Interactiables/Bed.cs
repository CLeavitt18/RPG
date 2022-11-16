using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Bed : Interactialbes, IInteractable
{
    public void OnEnable()
    {
        PUIInsruction = PlayerUi.playerUi.transform.GetChild(0).transform.GetChild(1).gameObject;
        gameObject.name = Name;
    }

    public override void SetUiOpen()
    {
        StringBuilder sb = new StringBuilder("E: Use ");
        sb.Append(Name);

        PUIInsruction.GetComponent<Text>().text = sb.ToString();

        PUIInsruction.SetActive(true);
        UIOpen = true;

        NextTime = Time.time + WaitTime;
    }

    public void Interact(bool State)
    {
        PlayerUi.playerUi.CallSetSleepUi();

        SetPlayerState(State);
    }
}
