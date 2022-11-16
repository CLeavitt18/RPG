using UnityEngine;
using TMPro;

public class Refinery : Interactialbes, IInteractable
{
    public GameObject RefinaryUI;

    public void OnEnable()
    {
        PUIInsruction = GameObject.Find("Player UI").transform.GetChild(0).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        RefinaryUI = GameObject.Find("RefinaryUi").transform.GetChild(0).gameObject;
        gameObject.name = Name;
    }

    public void Interact(bool State)
    {
        RefinaryUI.SetActive(State);
        RefinaryUI.transform.parent.gameObject.GetComponent<RefineryUi>().SetRefineryToDefault(true);
        SetPlayerState(State);
    }
    
    public override void SetUiOpen()
    {
        PUIInsruction.text = "E: Use " + Name;

        base.SetUiOpen();
    }

}
