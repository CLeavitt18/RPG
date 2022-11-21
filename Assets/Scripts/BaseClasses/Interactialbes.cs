using UnityEngine;
using TMPro;

public class Interactialbes : MonoBehaviour
{
    public TextMeshProUGUI PlayerInstructionText;

    public bool UIOpen;

    public float WaitTime = 0.1f;

    public string Name;

    [HideInInspector]
    public float NextTime;

    public void OnEnable()
    {
        PlayerInstructionText = PlayerUi.playerUi.PlayerInstuctionText;
        gameObject.name = Name;
    }

    public void Update()
    {
        if (UIOpen && Time.time >= NextTime || Player.player.GetMode() == PlayerState.InContainer)
        {
            UIOpen = false;
            PlayerInstructionText.gameObject.SetActive(false);
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
        PlayerInstructionText.gameObject.SetActive(true);
        UIOpen = true;

        NextTime = Time.time + WaitTime;
    }
}
