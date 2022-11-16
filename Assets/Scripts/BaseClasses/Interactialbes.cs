using UnityEngine;
using TMPro;

public class Interactialbes : MonoBehaviour
{
    public TextMeshProUGUI PUIInsruction;

    public bool UIOpen;

    public float WaitTime = 0.1f;

    public string Name;

    [HideInInspector]
    public float NextTime;

    public void Update()
    {
        if (UIOpen && Time.time >= NextTime || Player.player.GetMode() == PlayerState.InContainer)
        {
            UIOpen = false;
            PUIInsruction.gameObject.SetActive(false);
        }
    }

    public void SetPlayerState(bool State)
    {
        InventoryUi.playerUi.PlayerCanvas.SetActive(!State);

        if (State)
        {
            Player.player.SetPlayerStateInContainer();
        }
    }

    public virtual void SetUiOpen()
    {
        PUIInsruction.gameObject.SetActive(true);
        UIOpen = true;

        NextTime = Time.time + WaitTime;
    }
}
